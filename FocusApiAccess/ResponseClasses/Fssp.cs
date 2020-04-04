namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FsspValue : IParameterValue
    {
        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) в Контур.Фокусе (для работы требуется подписка
        /// на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Исполнительные производства
        /// </summary>
        [JsonProperty("fssp", NullValueHandling = NullValueHandling.Ignore)]
        public Fssp[] Fssp { get; set; }

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
        /// Дата актуальности данных по исполнительным производствам лица
        /// </summary>
        [JsonProperty("updateDate", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdateDate { get; set; }
    }

    public partial class Fssp
    {
        /// <summary>
        /// Отдел судебных приставов
        /// </summary>
        [JsonProperty("bailiffDepartment", NullValueHandling = NullValueHandling.Ignore)]
        public string BailiffDepartment { get; set; }

        /// <summary>
        /// Адрес отдела судебных приставов
        /// </summary>
        [JsonProperty("bailiffDepartmentAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string BailiffDepartmentAddress { get; set; }

        /// <summary>
        /// Дата окончания исполнительного производства (по причине возврата взыскателю, банкротства
        /// или ликвидации)
        /// </summary>
        [JsonProperty("cancelDate", NullValueHandling = NullValueHandling.Ignore)]
        public string CancelDate { get; set; }

        /// <summary>
        /// Окончено в связи с банкротством
        /// </summary>
        [JsonProperty("cancelledBecauseOfBancruptcy", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CancelledBecauseOfBancruptcy { get; set; }

        /// <summary>
        /// Окончено в связи с ликвидацией
        /// </summary>
        [JsonProperty("cancelledBecauseOfDissolvement", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CancelledBecauseOfDissolvement { get; set; }

        /// <summary>
        /// Адрес должника (если известен)
        /// </summary>
        [JsonProperty("debtorAddress", NullValueHandling = NullValueHandling.Ignore)]
        public string DebtorAddress { get; set; }

        /// <summary>
        /// Наименование должника (если известен)
        /// </summary>
        [JsonProperty("debtorName", NullValueHandling = NullValueHandling.Ignore)]
        public string DebtorName { get; set; }

        /// <summary>
        /// Описание исполнительного документа
        /// </summary>
        [JsonProperty("executoryDocument", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecutoryDocument { get; set; }

        /// <summary>
        /// Дата исполнительного документа
        /// </summary>
        [JsonProperty("executoryDocumentDate", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecutoryDocumentDate { get; set; }

        /// <summary>
        /// Номер исполнительного документа
        /// </summary>
        [JsonProperty("executoryDocumentNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string ExecutoryDocumentNumber { get; set; }

        /// <summary>
        /// Номер сводного исполнительного производства (если известен)
        /// </summary>
        [JsonProperty("officialAggregatedNum", NullValueHandling = NullValueHandling.Ignore)]
        public string OfficialAggregatedNum { get; set; }

        /// <summary>
        /// Номер исполнительного производства (если известен)
        /// </summary>
        [JsonProperty("officialNum", NullValueHandling = NullValueHandling.Ignore)]
        public string OfficialNum { get; set; }

        /// <summary>
        /// Исполнительное производство возвращено взыскателю по причине отсутствия имущества и т.п.
        /// </summary>
        [JsonProperty("returnedToClaimer", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReturnedToClaimer { get; set; }

        /// <summary>
        /// Дата возбуждения исполнительного производства (если известна)
        /// </summary>
        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }

        /// <summary>
        /// Сумма в рублях (если известна)
        /// </summary>
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double? Sum { get; set; }

        /// <summary>
        /// Предмет (если известен)
        /// </summary>
        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public string Topic { get; set; }
    }
}
