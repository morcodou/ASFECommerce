﻿using Microsoft.JSInterop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Model
{
    public class ApiBasket
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("items")]
        public ApiBasketItem[] Items { get; set; }

    }

    public class ApiBasketItem
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }
        [JsonProperty("quantity")]
        public int  Quantity { get; set; }
    }

    public class ApiBasketAddRequest
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
}