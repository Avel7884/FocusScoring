namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CheckPassportValue : IParameterValue
    {
        /// <summary>
        /// Дата, когда номер паспорта впервые был найден в списке недействительных паспортов
        /// (работает для записей, добавленных после 12.09.2016)
        /// </summary>
        [JsonProperty("invalidSince", NullValueHandling = NullValueHandling.Ignore)]
        public string InvalidSince { get; set; }

        /// <summary>
        /// Паспорт найден в списке недействительных паспортов
        /// </summary>
        [JsonProperty("isInvalid", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsInvalid { get; set; }

        /// <summary>
        /// Номер паспорта
        /// </summary>
        [JsonProperty("passportNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PassportNumber { get; set; }
    }
}
