using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.API.Model
{
    public class ApiProduct
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string    Name { get; set; }
        [JsonProperty("description")]
        public string    Description { get; set; }
        [JsonProperty("price")]
        public double Price{ get; set; }
        [JsonProperty("isAvailability")]
        public bool IsAvailability { get; set; }
    }
}
