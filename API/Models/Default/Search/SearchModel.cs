using Newtonsoft.Json;

namespace API.Models.Default.Search
{
    public class SearchModel
    {
        [JsonProperty("ruc")]
        public string Ruc { get; set; }

        [JsonProperty("rucs")]
        public string[] Rucs { get; set; }
    }
}