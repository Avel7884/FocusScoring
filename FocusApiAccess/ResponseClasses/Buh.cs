namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BuhValue : IParameterValue
    {
        /// <summary>
        /// Бухгалтерские формы
        /// </summary>
        [JsonProperty("buhForms", NullValueHandling = NullValueHandling.Ignore)]
        public BuhForm[] BuhForms { get; set; }

        /// <summary>
        /// Ссылка на карточку юридического лица в Контур.Фокусе (для работы требуется подписка на
        /// Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    public partial class BuhForm
    {
        /// <summary>
        /// Форма 1
        /// </summary>
        [JsonProperty("form1", NullValueHandling = NullValueHandling.Ignore)]
        public Form1Element[] Form1 { get; set; }

        /// <summary>
        /// Форма 2
        /// </summary>
        [JsonProperty("form2", NullValueHandling = NullValueHandling.Ignore)]
        public Form2Element[] Form2 { get; set; }

        /// <summary>
        /// Тип организации, который определяет набор показателей в формах
        /// </summary>
        [JsonProperty("organizationType", NullValueHandling = NullValueHandling.Ignore)]
        public OrganizationType? OrganizationType { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public long? Year { get; set; }
    }

    public partial class Form1Element
    {
        /// <summary>
        /// Код строки
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }

        /// <summary>
        /// Значение на конец периода
        /// </summary>
        [JsonProperty("endValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? EndValue { get; set; }

        /// <summary>
        /// Название строки
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Значение на начало периода
        /// </summary>
        [JsonProperty("startValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartValue { get; set; }
    }

    public partial class Form2Element
    {
        /// <summary>
        /// Код строки
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public long? Code { get; set; }

        /// <summary>
        /// Значение на конец периода
        /// </summary>
        [JsonProperty("endValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? EndValue { get; set; }

        /// <summary>
        /// Название строки
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Значение на начало периода
        /// </summary>
        [JsonProperty("startValue", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartValue { get; set; }
    }

    /// <summary>
    /// Тип организации, который определяет набор показателей в формах
    /// </summary>
    public enum OrganizationType { Large, Small, Sono };
    internal class OrganizationTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(OrganizationType) || t == typeof(OrganizationType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Large":
                    return OrganizationType.Large;
                case "SONO":
                    return OrganizationType.Sono;
                case "Small":
                    return OrganizationType.Small;
            }
            throw new Exception("Cannot unmarshal type OrganizationType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (OrganizationType)untypedValue;
            switch (value)
            {
                case OrganizationType.Large:
                    serializer.Serialize(writer, "Large");
                    return;
                case OrganizationType.Sono:
                    serializer.Serialize(writer, "SONO");
                    return;
                case OrganizationType.Small:
                    serializer.Serialize(writer, "Small");
                    return;
            }
            throw new Exception("Cannot marshal type OrganizationType");
        }

        public static readonly OrganizationTypeConverter Singleton = new OrganizationTypeConverter();
    }
}
