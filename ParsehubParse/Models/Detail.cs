using Newtonsoft.Json;

namespace ParsehubParse.Models
{
    public class Detail
    {
        [JsonProperty("detail_title")]
        public string Detail_title { get; set; }

        [JsonProperty("detail_value")]
        public string Detail_value { get; set; }
    }
}
