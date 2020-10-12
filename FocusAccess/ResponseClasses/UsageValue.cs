using Newtonsoft.Json;

namespace FocusAccess.ResponseClasses
{
    public class UsageValue : IParameterValue
    {
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? Count { get; set; }
    }
}