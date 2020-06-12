using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class Format
    {
        [JsonProperty("format_name")]

        public string FormatName { get; set; }

        [JsonProperty("format_value")]

        public string FormatValue { get; set; }
    }
}
