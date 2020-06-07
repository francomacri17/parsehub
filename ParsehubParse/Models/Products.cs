using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class Products
    {
        [JsonProperty("products")]
        public ICollection<Product> ProductsCollection { get; set; }
    }
}
