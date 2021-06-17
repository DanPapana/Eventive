using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.Dtos
{
    public class Row
    {
        [JsonProperty("elements")]
        public List<Element> Elements { get; set; }
    }
}
