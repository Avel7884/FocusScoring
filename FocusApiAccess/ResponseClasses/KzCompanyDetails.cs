namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class KzCompanyDetailsValue : IParameterValue
    {
        /// <summary>
        /// Виды деятельности
        /// </summary>
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public Activities Activities { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public Address Address { get; set; }

        /// <summary>
        /// Код БИН
        /// </summary>
        [JsonProperty("bin", NullValueHandling = NullValueHandling.Ignore)]
        public string Bin { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [JsonProperty("dateReg", NullValueHandling = NullValueHandling.Ignore)]
        public string DateReg { get; set; }

        /// <summary>
        /// Ссылка на карточку юридического лица Контур.Фокусе (для работы требуется подписка на
        /// Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Руководители
        /// </summary>
        [JsonProperty("heads", NullValueHandling = NullValueHandling.Ignore)]
        public KzHead[] Heads { get; set; }

        /// <summary>
        /// Классификатор размерности предприятия
        /// </summary>
        [JsonProperty("krp", NullValueHandling = NullValueHandling.Ignore)]
        public Krp Krp { get; set; }

        /// <summary>
        /// Полное наименование на казахском языке
        /// </summary>
        [JsonProperty("nameKaz", NullValueHandling = NullValueHandling.Ignore)]
        public string NameKaz { get; set; }

        /// <summary>
        /// Полное наименование на русском языке
        /// </summary>
        [JsonProperty("nameRus", NullValueHandling = NullValueHandling.Ignore)]
        public string NameRus { get; set; }

        /// <summary>
        /// Код по общему классификатору предприятий и организаций
        /// </summary>
        [JsonProperty("okpo", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpo { get; set; }

        /// <summary>
        /// Статус организации
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public KzStatus Status { get; set; }
    }

    /// <summary>
    /// Виды деятельности
    /// </summary>
    public partial class KzActivities
    {
        /// <summary>
        /// Дополнительные виды деятельности по ОКЭД
        /// </summary>
        [JsonProperty("complementaryActivities", NullValueHandling = NullValueHandling.Ignore)]
        public KzActivity[] ComplementaryActivities { get; set; }

        /// <summary>
        /// Основной вид деятельности по ОКЭД
        /// </summary>
        [JsonProperty("principalActivity", NullValueHandling = NullValueHandling.Ignore)]
        public KzActivity PrincipalActivity { get; set; }
    }

    /// <summary>
    /// Основной вид деятельности по ОКЭД
    /// </summary>
    public partial class KzActivity
    {
        /// <summary>
        /// Код ОКЭД
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Название вида деятельности
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public partial class Address
    {
        /// <summary>
        /// Код КАТО
        /// </summary>
        [JsonProperty("kato", NullValueHandling = NullValueHandling.Ignore)]
        public string Kato { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    public partial class KzHead
    {
        /// <summary>
        /// ФИО руководителя
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }
    }

    /// <summary>
    /// Классификатор размерности предприятия
    /// </summary>
    public partial class Krp
    {
        /// <summary>
        /// Код КРП
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Наименование КРП
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Статус организации
    /// </summary>
    public partial class KzStatus
    {
        /// <summary>
        /// Недействующее
        /// </summary>
        [JsonProperty("dissolved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolved { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("statusString", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusString { get; set; }
    }
}
