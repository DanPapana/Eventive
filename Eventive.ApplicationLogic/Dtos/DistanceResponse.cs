using Newtonsoft.Json;
using System.Collections.Generic;

namespace Eventive.ApplicationLogic.Dtos
{
    public class DistanceResponse
    {
        [JsonProperty("destination_addresses")]
        public List<string> DestinationAddresses { get; set; }

        [JsonProperty("origin_addresses")]
        public List<string> OriginAddresses { get; set; }

        [JsonProperty("rows")]
        public List<Row> Rows { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
