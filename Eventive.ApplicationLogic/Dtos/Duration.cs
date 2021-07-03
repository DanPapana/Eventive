using Newtonsoft.Json;

namespace Eventive.ApplicationLogic.Dtos
{
    public class Duration
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
