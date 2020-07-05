using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ILogger<ProductsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ApiProduct>> GetAsync()
        {
            return new[] { new ApiProduct { Id = Guid.NewGuid(), Description ="fakeApiprod" } };
        }

        [HttpPost]
        public async Task PostAsync([FromBody] ApiProduct product)
        {
        }
    }
}
