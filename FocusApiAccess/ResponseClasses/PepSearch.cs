namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PepSearchValue : IParameterValue
    {
        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonProperty("birthday", NullValueHandling = NullValueHandling.Ignore)]
        public Birthday Birthday { get; set; }

        /// <summary>
        /// Источник информации. Декларация о доходах или документ о назначении и прекращении
        /// полномочий
        /// </summary>
        [JsonProperty("document", NullValueHandling = NullValueHandling.Ignore)]
        public PepDocument Document { get; set; }

        /// <summary>
        /// Событие, связанное с публичным должностным лицом
        /// </summary>
        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public Event? Event { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }
    }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public partial class Birthday
    {
        /// <summary>
        /// День рождения
        /// </summary>
        [JsonProperty("day", NullValueHandling = NullValueHandling.Ignore)]
        public string Day { get; set; }

        /// <summary>
        /// Месяц рождения
        /// </summary>
        [JsonProperty("month", NullValueHandling = NullValueHandling.Ignore)]
        public string Month { get; set; }

        /// <summary>
        /// Год рождения
        /// </summary>
        [JsonProperty("year", NullValueHandling = NullValueHandling.Ignore)]
        public string Year { get; set; }
    }

    /// <summary>
    /// Источник информации. Декларация о доходах или документ о назначении и прекращении
    /// полномочий
    /// </summary>
    public partial class PepDocument
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Ссылка на документ
        /// </summary>
        [JsonProperty("href", NullValueHandling = NullValueHandling.Ignore)]
        public string Href { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
    }

    /// <summary>
    /// Событие, связанное с публичным должностным лицом
    /// </summary>
    public enum Event { AppointmentToPost, FilingTheDeclaration, TerminationOfAuthority };

    internal class EventConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Event) || t == typeof(Event?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "AppointmentToPost":
                    return Event.AppointmentToPost;
                case "FilingTheDeclaration":
                    return Event.FilingTheDeclaration;
                case "TerminationOfAuthority":
                    return Event.TerminationOfAuthority;
            }
            throw new Exception("Cannot unmarshal type Event");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Event)untypedValue;
            switch (value)
            {
                case Event.AppointmentToPost:
                    serializer.Serialize(writer, "AppointmentToPost");
                    return;
                case Event.FilingTheDeclaration:
                    serializer.Serialize(writer, "FilingTheDeclaration");
                    return;
                case Event.TerminationOfAuthority:
                    serializer.Serialize(writer, "TerminationOfAuthority");
                    return;
            }
            throw new Exception("Cannot marshal type Event");
        }

        public static readonly EventConverter Singleton = new EventConverter();
    }
}
