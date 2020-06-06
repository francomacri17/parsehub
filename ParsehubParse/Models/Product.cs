using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class Product
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description_1")]
        public string Description_1 { get; set; }

        [JsonProperty("description_2")]
        public Dictionary<string, string> Description_2 { get; set; }

        [JsonProperty("price_1")]
        public string Price_1 { get; set; }

        [JsonProperty("price_2")]
        public string Price_2 { get; set; }

        [JsonProperty("image_1")]
        public string Image_1 { get; set; }

        [JsonProperty("image_2")]
        public string Image_2 { get; set; }

        [JsonProperty("image_3")]
        public string Image_3 { get; set; }

        [JsonProperty("colour")]
        public string Colour { get; set; }

        [JsonProperty("details")]
        public List<Detail> Details { get; set; }
    }
}
