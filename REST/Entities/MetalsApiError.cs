using Newtonsoft.Json;

namespace AcumaticaMetalsAPI
{
    public class MetalsApiError : MetalsAPIResponse
    {
        [JsonProperty("error")]
        public MetalsApiErrorDetail error { get; set; }

    }

    public class MetalsApiErrorDetail
    {
        [JsonProperty("code")]
        public int code { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("info")]
        public string info { get; set; }
  
    }

}
