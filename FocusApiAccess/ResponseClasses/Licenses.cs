using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess.Response
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class LicencesValue : IParameterValue
    {
        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) в Контур.Фокусе (для работы требуется подписка
        /// на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// ИНН(ИП)
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Лицензии
        /// </summary>
        [JsonProperty("licenses", NullValueHandling = NullValueHandling.Ignore)]
        public License[] Licenses { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    public partial class License
    {
        /// <summary>
        /// Описание вида деятельности
        /// </summary>
        [JsonProperty("activity", NullValueHandling = NullValueHandling.Ignore)]
        public string Activity { get; set; }

        /// <summary>
        /// Места действия лицензии (массив неформализованных строк)
        /// </summary>
        [JsonProperty("addresses", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Addresses { get; set; }

        /// <summary>
        /// Дата лицензии (если известна) - чаще всего совпадает с датой начала действия лицензии
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Дата окончания действия лицензии (если известна)
        /// </summary>
        [JsonProperty("dateEnd", NullValueHandling = NullValueHandling.Ignore)]
        public string DateEnd { get; set; }

        /// <summary>
        /// Дата начала действия лицензии (если известна)
        /// </summary>
        [JsonProperty("dateStart", NullValueHandling = NullValueHandling.Ignore)]
        public string DateStart { get; set; }

        /// <summary>
        /// Название органа, выдавшего лицензию (если извествен)
        /// </summary>
        [JsonProperty("issuerName", NullValueHandling = NullValueHandling.Ignore)]
        public string IssuerName { get; set; }

        /// <summary>
        /// Номер лицензии (если извествен)
        /// </summary>
        [JsonProperty("officialNum", NullValueHandling = NullValueHandling.Ignore)]
        public string OfficialNum { get; set; }

        /// <summary>
        /// Описание видов работ/услуг
        /// </summary>
        [JsonProperty("services", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Services { get; set; }

        /// <summary>
        /// Источник сведений о лицензии (конкретный орган управления или ЕГРЮЛ)
        /// </summary>
        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        /// <summary>
        /// Строковое описание статуса (если известно)
        /// </summary>
        [JsonProperty("statusDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusDescription { get; set; }
    }
}
