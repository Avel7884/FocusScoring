namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SuggestValue : IParameterValue
    {
        /// <summary>
        /// Юридический адрес юрлица или адрес индивидуального предпринимателя
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// ФИО ИП
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// Порядковый номер результата
        /// </summary>
        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// КПП юрлица
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Полное наименование юрлица
        /// </summary>
        [JsonProperty("longName", NullValueHandling = NullValueHandling.Ignore)]
        public string LongName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Краткое наименование юрлица
        /// </summary>
        [JsonProperty("shortName", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortName { get; set; }

        /// <summary>
        /// Статус юрлица или индивидуального предпринимателя
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public Status Status { get; set; }

        /// <summary>
        /// Тип найденного лица
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? Type { get; set; }
    }

    /// <summary>
    /// Статус юрлица или индивидуального предпринимателя
    /// </summary>
    public partial class Status
    {
        /// <summary>
        /// Недействующее
        /// </summary>
        [JsonProperty("dissolved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolved { get; set; }

        /// <summary>
        /// В стадии ликвидации (либо планируется исключение из ЕГРЮЛ)
        /// </summary>
        [JsonProperty("dissolving", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolving { get; set; }

        /// <summary>
        /// В процессе реорганизации (может прекратить деятельность в результате реорганизации)
        /// </summary>
        [JsonProperty("reorganizing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Reorganizing { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("statusString", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusString { get; set; }
    }

    /// <summary>
    /// Тип найденного лица
    /// </summary>
    public enum SuggestEnum { Ip, Ul };

    internal class SuggestEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "IP":
                    return SuggestEnum.Ip;
                case "UL":
                    return SuggestEnum.Ul;
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
            var value = (SuggestEnum)untypedValue;
            switch (value)
            {
                case SuggestEnum.Ip:
                    serializer.Serialize(writer, "IP");
                    return;
                case SuggestEnum.Ul:
                    serializer.Serialize(writer, "UL");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
