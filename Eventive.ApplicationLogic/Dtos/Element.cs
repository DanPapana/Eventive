using Newtonsoft.Json;

namespace Eventive.ApplicationLogic.Dtos
{
    public class Element
    {
        [JsonProperty("distance")]
        public Distance Distance { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
