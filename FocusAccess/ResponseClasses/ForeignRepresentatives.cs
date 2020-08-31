namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ForeignRepresentativesValue : IParameterValue
    {
        /// <summary>
        /// Информация об аккредитации
        /// </summary>
        [JsonProperty("accreditation", NullValueHandling = NullValueHandling.Ignore)]
        public Accreditation Accreditation { get; set; }

        /// <summary>
        /// Виды деятельности
        /// </summary>
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public Activities Activities { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public ParsedAddressRf Address { get; set; }

        /// <summary>
        /// Ссылка на карточку в Контур.Фокусе (для работы требуется подписка на Контур.Фокус и
        /// дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Численность иностранных сотрудников
        /// </summary>
        [JsonProperty("foreignStaff", NullValueHandling = NullValueHandling.Ignore)]
        public long? ForeignStaff { get; set; }

        /// <summary>
        /// Полное наименование филиала/представительства
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Головная организация
        /// </summary>
        [JsonProperty("headOrganization", NullValueHandling = NullValueHandling.Ignore)]
        public HeadOrganization HeadOrganization { get; set; }

        /// <summary>
        /// Руководители
        /// </summary>
        [JsonProperty("heads", NullValueHandling = NullValueHandling.Ignore)]
        public ForeignHead[] Heads { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Сведения о постановке на учет в налоговом органе
        /// </summary>
        [JsonProperty("nalogRegBody", NullValueHandling = NullValueHandling.Ignore)]
        public ForeignNalogRegBody NalogRegBody { get; set; }

        /// <summary>
        /// НЗА
        /// </summary>
        [JsonProperty("nza", NullValueHandling = NullValueHandling.Ignore)]
        public string Nza { get; set; }

        /// <summary>
        /// Краткое наименование филиала/представительства
        /// </summary>
        [JsonProperty("shortName", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortName { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }

    /// <summary>
    /// Информация об аккредитации
    /// </summary>
    public partial class Accreditation
    {
        /// <summary>
        /// Аккредитирующий орган
        /// </summary>
        [JsonProperty("accreditationBody", NullValueHandling = NullValueHandling.Ignore)]
        public AccreditationBody AccreditationBody { get; set; }

        /// <summary>
        /// Дата прекращения аккредитации
        /// </summary>
        [JsonProperty("endDate", NullValueHandling = NullValueHandling.Ignore)]
        public string EndDate { get; set; }

        /// <summary>
        /// Дата аккредитации
        /// </summary>
        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }
    }

    /// <summary>
    /// Аккредитирующий орган
    /// </summary>
    public partial class AccreditationBody
    {
        /// <summary>
        /// Код аккредитирующего органа
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }
    }

    /// <summary>
    /// Виды деятельности
    /// </summary>
    public partial class Activities
    {
        /// <summary>
        /// Дополнительные виды деятельности по ОКВЭД
        /// </summary>
        [JsonProperty("complementaryActivities", NullValueHandling = NullValueHandling.Ignore)]
        public ForeignActivity[] ComplementaryActivities { get; set; }

        /// <summary>
        /// Основной вид деятельности по ОКВЭД
        /// </summary>
        [JsonProperty("principalActivity", NullValueHandling = NullValueHandling.Ignore)]
        public ForeignActivity PrincipalActivity { get; set; }
    }

    /// <summary>
    /// Основной вид деятельности по ОКВЭД
    /// </summary>
    public partial class ForeignActivity
    {
        /// <summary>
        /// Код вида деятельности
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Название вида деятельности
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    /// <summary>
    /// Головная организация
    /// </summary>
    public partial class HeadOrganization
    {
        /// <summary>
        /// Страна головной организации
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// Наименование головной организации
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }
    }

    public partial class ForeignHead
    {
        /// <summary>
        /// ФИО руководителя
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ИННФЛ
        /// </summary>
        [JsonProperty("innfl", NullValueHandling = NullValueHandling.Ignore)]
        public string Innfl { get; set; }
    }

    /// <summary>
    /// Сведения о постановке на учет в налоговом органе
    /// </summary>
    public partial class ForeignNalogRegBody
    {
        /// <summary>
        /// Код налогового органа
        /// </summary>
        [JsonProperty("nalogCode", NullValueHandling = NullValueHandling.Ignore)]
        public string NalogCode { get; set; }

        /// <summary>
        /// Дата постановки на учет
        /// </summary>
        [JsonProperty("nalogRegDate", NullValueHandling = NullValueHandling.Ignore)]
        public string NalogRegDate { get; set; }
    }
}
