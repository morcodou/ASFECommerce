using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using ECommerce.CheckoutService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using UserActor.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private static readonly Random random = new Random(DateTime.UtcNow.Second);

        // GET api/<CheckoutController>/5
        [HttpGet("{userId}")]
        public async Task<ApiCheckoutSummary> CheckoutAsync(string userId)
        {
            var summary = await GetCheckoutService().CheckoutAsync(userId);
            return ToApiCheckoutSummary(summary);
        }

        // GET api/<CheckoutController>/5
        [HttpGet("history/{userId}")]
        public async Task<IEnumerable<ApiCheckoutSummary>> GetHistoryAsync(string userId)
        {
            var history = await GetCheckoutService().GetOrderHistoryAsync(userId);
            return history.Select(ToApiCheckoutSummary);
        }


        private ApiCheckoutSummary ToApiCheckoutSummary(CheckoutSummary summary)
        {
            return new ApiCheckoutSummary
            {
                Products = summary.Products.Select(p => new ApiCheckoutProduct
                {
                    ProductId = p.Product.Id,
                    ProductName = p.Product.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                }).ToList(),
                Date = summary.Date,
                TotalPrice = summary.TotalPrice
            };
        }

        private ICheckoutService GetCheckoutService()
        {
            long key = LongRandom();
            var proxyFactory = new ServiceProxyFactory(c => new FabricTransportServiceRemotingClientFactory());
            
            return proxyFactory.CreateServiceProxy<ICheckoutService>(
                new Uri("fabric:/ASFECommerce/ECommerce.CheckoutService"),
                new ServicePartitionKey(key));
        }

        private long LongRandom()
        {
            byte[] buffer = new byte[8];
            random.NextBytes(buffer);
            long longRand = BitConverter.ToInt64(buffer, 0);
            return longRand;
        }
    }
}
