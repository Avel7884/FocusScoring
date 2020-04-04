namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class StatValue : IParameterValue
    {
        /// <summary>
        /// Лимит
        /// </summary>
        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Limit { get; set; }

        /// <summary>
        /// Тип лимита: Requests - количество запросов, LegalEntities - количество контрагентов
        /// (организаций), Persons - количество персон
        /// </summary>
        [JsonProperty("limitType", NullValueHandling = NullValueHandling.Ignore)]
        public LimitType? LimitType { get; set; }

        /// <summary>
        /// Название метода
        /// </summary>
        [JsonProperty("methodName", NullValueHandling = NullValueHandling.Ignore)]
        public string MethodName { get; set; }

        /// <summary>
        /// Дата окончания периода использования
        /// </summary>
        [JsonProperty("periodEndDate", NullValueHandling = NullValueHandling.Ignore)]
        public string PeriodEndDate { get; set; }

        /// <summary>
        /// Дата начала периода использования
        /// </summary>
        [JsonProperty("periodStartDate", NullValueHandling = NullValueHandling.Ignore)]
        public string PeriodStartDate { get; set; }

        /// <summary>
        /// Истрачено
        /// </summary>
        [JsonProperty("spent", NullValueHandling = NullValueHandling.Ignore)]
        public long? Spent { get; set; }
    }

    /// <summary>
    /// Тип лимита: Requests - количество запросов, LegalEntities - количество контрагентов
    /// (организаций), Persons - количество персон
    /// </summary>
    public enum LimitType { LegalEntities, Persons, Requests };

    internal class LimitTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LimitType) || t == typeof(LimitType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "LegalEntities":
                    return LimitType.LegalEntities;
                case "Persons":
                    return LimitType.Persons;
                case "Requests":
                    return LimitType.Requests;
            }
            throw new Exception("Cannot unmarshal type LimitType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LimitType)untypedValue;
            switch (value)
            {
                case LimitType.LegalEntities:
                    serializer.Serialize(writer, "LegalEntities");
                    return;
                case LimitType.Persons:
                    serializer.Serialize(writer, "Persons");
                    return;
                case LimitType.Requests:
                    serializer.Serialize(writer, "Requests");
                    return;
            }
            throw new Exception("Cannot marshal type LimitType");
        }

        public static readonly LimitTypeConverter Singleton = new LimitTypeConverter();
    }
}
