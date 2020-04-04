namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class PersonBankruptcyValue : IParameterValue
    {
        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonProperty("birthDate", NullValueHandling = NullValueHandling.Ignore)]
        public string BirthDate { get; set; }

        /// <summary>
        /// Номер арбитражного дела о банкротстве
        /// </summary>
        [JsonProperty("caseNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string CaseNumber { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ИННФЛ
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Дата последнего сообщения в Едином федеральном реестре сведений о банкротстве
        /// </summary>
        [JsonProperty("lastMessageDate", NullValueHandling = NullValueHandling.Ignore)]
        public string LastMessageDate { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        [JsonProperty("snils", NullValueHandling = NullValueHandling.Ignore)]
        public string Snils { get; set; }

        /// <summary>
        /// Текущая стадия банкротства
        /// </summary>
        [JsonProperty("stage", NullValueHandling = NullValueHandling.Ignore)]
        public Stage? Stage { get; set; }

        /// <summary>
        /// Дата решения суда о введении текущей стадии банкротства
        /// </summary>
        [JsonProperty("stageDate", NullValueHandling = NullValueHandling.Ignore)]
        public string StageDate { get; set; }
    }

    /// <summary>
    /// Текущая стадия банкротства
    /// </summary>
    public enum Stage { ВнешнееУправление, КонкурсноеПроизводство, КонкурсноеПроизводствоЗавершено, Наблюдение, НеУдалосьОпределитьСтадию, ОтказаноВПризнанииДолжникаБанкротом, ПроизводствоПоДелуПрекращено, РеализацияИмущества, РеализацияИмуществаЗавершена, РеструктуризацияДолгов, РеструктуризацияДолговЗавершена, ФинансовоеОздоровление };
    

    internal class StageConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Stage) || t == typeof(Stage?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Внешнее управление":
                    return Stage.ВнешнееУправление;
                case "Конкурсное производство":
                    return Stage.КонкурсноеПроизводство;
                case "Конкурсное производство завершено":
                    return Stage.КонкурсноеПроизводствоЗавершено;
                case "Наблюдение":
                    return Stage.Наблюдение;
                case "Не удалось определить стадию":
                    return Stage.НеУдалосьОпределитьСтадию;
                case "Отказано в признании должника банкротом":
                    return Stage.ОтказаноВПризнанииДолжникаБанкротом;
                case "Производство по делу прекращено":
                    return Stage.ПроизводствоПоДелуПрекращено;
                case "Реализация имущества":
                    return Stage.РеализацияИмущества;
                case "Реализация имущества завершена":
                    return Stage.РеализацияИмуществаЗавершена;
                case "Реструктуризация долгов":
                    return Stage.РеструктуризацияДолгов;
                case "Реструктуризация долгов завершена":
                    return Stage.РеструктуризацияДолговЗавершена;
                case "Финансовое оздоровление":
                    return Stage.ФинансовоеОздоровление;
            }
            throw new Exception("Cannot unmarshal type Stage");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Stage)untypedValue;
            switch (value)
            {
                case Stage.ВнешнееУправление:
                    serializer.Serialize(writer, "Внешнее управление");
                    return;
                case Stage.КонкурсноеПроизводство:
                    serializer.Serialize(writer, "Конкурсное производство");
                    return;
                case Stage.КонкурсноеПроизводствоЗавершено:
                    serializer.Serialize(writer, "Конкурсное производство завершено");
                    return;
                case Stage.Наблюдение:
                    serializer.Serialize(writer, "Наблюдение");
                    return;
                case Stage.НеУдалосьОпределитьСтадию:
                    serializer.Serialize(writer, "Не удалось определить стадию");
                    return;
                case Stage.ОтказаноВПризнанииДолжникаБанкротом:
                    serializer.Serialize(writer, "Отказано в признании должника банкротом");
                    return;
                case Stage.ПроизводствоПоДелуПрекращено:
                    serializer.Serialize(writer, "Производство по делу прекращено");
                    return;
                case Stage.РеализацияИмущества:
                    serializer.Serialize(writer, "Реализация имущества");
                    return;
                case Stage.РеализацияИмуществаЗавершена:
                    serializer.Serialize(writer, "Реализация имущества завершена");
                    return;
                case Stage.РеструктуризацияДолгов:
                    serializer.Serialize(writer, "Реструктуризация долгов");
                    return;
                case Stage.РеструктуризацияДолговЗавершена:
                    serializer.Serialize(writer, "Реструктуризация долгов завершена");
                    return;
                case Stage.ФинансовоеОздоровление:
                    serializer.Serialize(writer, "Финансовое оздоровление");
                    return;
            }
            throw new Exception("Cannot marshal type Stage");
        }

        public static readonly StageConverter Singleton = new StageConverter();
    }
}
