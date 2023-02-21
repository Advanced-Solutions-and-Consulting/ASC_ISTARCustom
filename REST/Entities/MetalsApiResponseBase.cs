using Newtonsoft.Json;

namespace MetalsAPI
{
    public class BaseResponse
    {
        [JsonProperty("success")]
        public bool success { get { return _success; } set { _success = value; } }
        private bool _success = true;
    }

}

public class MetalsAPIError
{

}

public class Root
{
    public bool success { get; set; }
    public MetalsAPIError error { get; set; }
}
