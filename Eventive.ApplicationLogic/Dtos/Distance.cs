using Newtonsoft.Json;

namespace Eventive.ApplicationLogic.Dtos
{
    public class Distance
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }
}
