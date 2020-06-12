using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class Book
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("image_1")]
        public string Image_1 { get; set; }

        [JsonProperty("formats")]
        public List<Format> Formats { get; set; }

        [JsonProperty("description_1")]
        public string Description_1 { get; set; }

        [JsonProperty("details_1")]
        public List<Dictionary<string, string>> Details { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
    }
}
