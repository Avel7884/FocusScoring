namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ByCompanyDetailsValue : IParameterValue
    {
        /// <summary>
        /// Адрес
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [JsonProperty("dateReg", NullValueHandling = NullValueHandling.Ignore)]
        public string DateReg { get; set; }

        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) Контур.Фокусе (для работы требуется подписка на
        /// Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Полное наименование ЮЛ (ФИО ИП)
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Сведения о постановке на учет в налоговом органе
        /// </summary>
        [JsonProperty("nalogRegBody", NullValueHandling = NullValueHandling.Ignore)]
        public RegBody NalogRegBody { get; set; }

        /// <summary>
        /// Сведения регистрирующего органа
        /// </summary>
        [JsonProperty("regBody", NullValueHandling = NullValueHandling.Ignore)]
        public RegBody RegBody { get; set; }

        /// <summary>
        /// Краткое наименование ЮЛ
        /// </summary>
        [JsonProperty("shortName", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortName { get; set; }

        /// <summary>
        /// Статус организации
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public ByStatus Status { get; set; }

        /// <summary>
        /// Тип субъекта хозяйствования. IP - индивидуальный предприниматель, UL - юридическое лицо
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public ByEnum? Type { get; set; }

        /// <summary>
        /// УНП. Регистрационный номер субъекта хозяйствования
        /// </summary>
        [JsonProperty("unp", NullValueHandling = NullValueHandling.Ignore)]
        public string Unp { get; set; }
    }

    /// <summary>
    /// Сведения о постановке на учет в налоговом органе
    ///
    /// Сведения регистрирующего органа
    /// </summary>
    public partial class RegBody
    {
        /// <summary>
        /// Код органа
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Наименование органа
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Статус организации
    /// </summary>
    public class ByStatus
    {
        /// <summary>
        /// Дата исключения из Единого государственного регистра ЮЛ и ИП
        /// </summary>
        [JsonProperty("dateDissolved", NullValueHandling = NullValueHandling.Ignore)]
        public string DateDissolved { get; set; }

        /// <summary>
        /// Исключен из Единого государственного регистра ЮЛ и ИП
        /// </summary>
        [JsonProperty("dissolved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolved { get; set; }

        /// <summary>
        /// Находится в процессе ликвидации
        /// </summary>
        [JsonProperty("dissolving", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolving { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("statusString", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusString { get; set; }
    }

    /// <summary>
    /// Тип субъекта хозяйствования. IP - индивидуальный предприниматель, UL - юридическое лицо
    /// </summary>
    public enum ByEnum { Ip, Ul };
    
    internal class ByEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ByEnum) || t == typeof(ByEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "IP":
                    return ByEnum.Ip;
                case "UL":
                    return ByEnum.Ul;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ByEnum)untypedValue;
            switch (value)
            {
                case ByEnum.Ip:
                    serializer.Serialize(writer, "IP");
                    return;
                case ByEnum.Ul:
                    serializer.Serialize(writer, "UL");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
