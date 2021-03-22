using Newtonsoft.Json;

namespace FunctionApp.Models
{
    public class Bookmark
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}