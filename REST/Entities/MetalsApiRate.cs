using Newtonsoft.Json;
using System.Collections.Generic;

namespace AcumaticaMetalsAPI
{
    public class RateResponse : MetalsAPIResponse
    {
        [JsonProperty("rate")]
        public MetalsAPIResponse Rate { get; set; }
    }

    public class Root : MetalsAPIResponse
    {

        [JsonProperty("historical")]
        public bool historical { get; set; }
        [JsonProperty("historical")]
        public string date { get; set; }
        [JsonProperty("date")]
        public string @base { get; set; }
        [JsonProperty("rates")]
        public Dictionary<string, string> rates { get; set; }
        [JsonProperty("unit")]
        public string unit { get; set; }

    }
}
