namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class SanctionedPersonsValue : IParameterValue
    {
        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonProperty("birthDate", NullValueHandling = NullValueHandling.Ignore)]
        public string BirthDate { get; set; }

        /// <summary>
        /// Место рождения
        /// </summary>
        [JsonProperty("birthPlace", NullValueHandling = NullValueHandling.Ignore)]
        public string BirthPlace { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// Название санкционного списка
        /// </summary>
        [JsonProperty("listName", NullValueHandling = NullValueHandling.Ignore)]
        public string ListName { get; set; }

        /// <summary>
        /// Список санкционных программ
        /// </summary>
        [JsonProperty("sanctionsPrograms", NullValueHandling = NullValueHandling.Ignore)]
        public string[] SanctionsPrograms { get; set; }
    }
}
