namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ContactsValue : IParameterValue
    {
        /// <summary>
        /// Контактные телефоны из Контур.Справочника (для получения контактов требуется отдельная
        /// подписка и вызов отдельного метода)
        /// </summary>
        [JsonProperty("contactPhones", NullValueHandling = NullValueHandling.Ignore)]
        public ContactPhones ContactPhones { get; set; }

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
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    /// <summary>
    /// Контактные телефоны из Контур.Справочника (для получения контактов требуется отдельная
    /// подписка и вызов отдельного метода)
    /// </summary>
    public partial class ContactPhones
    {
        /// <summary>
        /// Количество найденных контактых телефонов
        /// </summary>
        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public long? Count { get; set; }

        /// <summary>
        /// Список номеров телефонов
        /// </summary>
        [JsonProperty("phones", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Phones { get; set; }
    }
}