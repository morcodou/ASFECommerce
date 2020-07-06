using ECommerce.ProductCatalog.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.ProductCatalog
{
    class ServiceFabricProductRepository : IProductRepository
    {
        private readonly IReliableStateManager _stateManager;
        const string PRODUCTS = @"products";
        public ServiceFabricProductRepository(IReliableStateManager stateManager)
        {
            this._stateManager = stateManager;
        }
        public async Task AddProduct(Product product)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(PRODUCTS);
            using (var transaction = _stateManager.CreateTransaction())
            {
                await products.AddOrUpdateAsync(transaction, product.Id, product, (id, value) => product);
                await transaction.CommitAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(PRODUCTS);
            var result = new List<Product>();

            using (var transaction = _stateManager.CreateTransaction())
            {
                var allProducts = await products.CreateEnumerableAsync(transaction, EnumerationMode.Unordered);
                using (var enumrator = allProducts.GetAsyncEnumerator())
                {
                    while (await enumrator.MoveNextAsync(CancellationToken.None))
                    {
                        result.Add(enumrator.Current.Value);
                    }
                }
            }

            return result;
        }

        public async Task<Product> GetProduct(Guid productId)
        {
            var products = await _stateManager.GetOrAddAsync<IReliableDictionary<Guid, Product>>(PRODUCTS);
            using (var transaction = _stateManager.CreateTransaction())
            {
                var conditionalProduct = await products.TryGetValueAsync(transaction, productId);
                return conditionalProduct.HasValue ? conditionalProduct.Value : null;
            }
        }
    }
}
