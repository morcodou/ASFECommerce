using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using UserActor.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {


        // GET api/<BasketController>/5
        [HttpGet("{userId}")]
        public async Task<ApiBasket> GetAsync(string userId)
        {
            var actor = GetActor(userId);
            var products = await actor.GetBasket();

            return new ApiBasket()
            {
                UserId = userId,
                Items = products.Select(product => new ApiBasketItem
                {
                    ProductId = $"{product.ProductId}",
                    Quantity = product.Quantity
                }).ToArray()
            };
        }

        // POST api/<BasketController>
        [HttpPost("{userId}")]
        public async void Post(string userId, [FromBody] ApiBasketAddRequest request)
        {
            var actor = GetActor(userId);
            await actor.AddToBasket(request.ProductId, request.Quantity);
        }


        // DELETE api/<BasketController>/5
        [HttpDelete("{userId}")]
        public async void Delete(string userId)
        {
            var actor = GetActor(userId);
            await actor.ClearBasket();
        }

        private IUserActor GetActor(string userId)
        {
            return ActorProxy.Create<IUserActor>(new ActorId(userId), new Uri("fabric:/ASFECommerce/UserActorService"));
        }
    }
}
