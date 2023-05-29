using System.Text.Json.Serialization;

namespace SimpleBlogMauiApp.Models
{
    class Message
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("content")]
        public object Content { get; set; }
    }
}
