namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class TrademarksValue : IParameterValue
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
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Товарные знаки
        /// </summary>
        [JsonProperty("trademarks", NullValueHandling = NullValueHandling.Ignore)]
        public Trademark[] Trademarks { get; set; }
    }

    public partial class Trademark
    {
        /// <summary>
        /// Дата истечения срока действия регистрации
        /// </summary>
        [JsonProperty("dateEnd", NullValueHandling = NullValueHandling.Ignore)]
        public string DateEnd { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [JsonProperty("docNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string DocNumber { get; set; }

        /// <summary>
        /// Картинка в формате PNG в виде строки base64
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image { get; set; }

        /// <summary>
        /// Тип товарного знака: RUAT, RUTM или RUWK
        /// </summary>
        [JsonProperty("trademarkType", NullValueHandling = NullValueHandling.Ignore)]
        public TrademarkType TrademarkType { get; set; }
    }

    /// <summary>
    /// Тип товарного знака: RUAT, RUTM или RUWK
    /// </summary>
    public partial class TrademarkType
    {
        /// <summary>
        /// Код типа
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Наименование типа
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
}
