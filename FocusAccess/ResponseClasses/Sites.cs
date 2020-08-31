namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SitesValue : IParameterValue
    {
        /// <summary>
        /// Ссылка на карточку юридического лица (индивидуального предпринимателя) в Контур.Фокусе
        /// (для работы требуется подписка на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// ИНН(ИП)
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Список потенциальных сайтов компании
        /// </summary>
        [JsonProperty("sites", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Sites { get; set; }
    }
}
