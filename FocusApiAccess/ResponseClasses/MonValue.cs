namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MonValue : IParameterValue
    {
        /// <summary>
        /// ОГРН или ОГРНИП
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }
    
    public partial class MonListValue : IParameterValue
    {
        /// <summary>
        /// ИНН юридического лица или предпринимателя
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН или ОГРНИП
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }
}
