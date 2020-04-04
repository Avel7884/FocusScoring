namespace FocusApiAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BeneficialOwnersValue : IParameterValue
    {
        /// <summary>
        /// Предполагаемые конечные владельцы
        /// </summary>
        [JsonProperty("beneficialOwners", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwners BeneficialOwners { get; set; }

        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) в Контур.Фокусе (для работы требуется подписка
        /// на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Исторические конечные владельцы
        /// </summary>
        [JsonProperty("historicalBeneficialOwners", NullValueHandling = NullValueHandling.Ignore)]
        public HistoricalBeneficialOwners HistoricalBeneficialOwners { get; set; }

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
        /// Уставный капитал
        /// </summary>
        [JsonProperty("statedCapital", NullValueHandling = NullValueHandling.Ignore)]
        public StatedCapital StatedCapital { get; set; }
    }

    /// <summary>
    /// Предполагаемые конечные владельцы
    /// </summary>
    public partial class BeneficialOwners
    {
        /// <summary>
        /// Конечные владельцы - физлица
        /// </summary>
        [JsonProperty("beneficialOwnersFL", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerFl[] BeneficialOwnersFl { get; set; }

        /// <summary>
        /// Конечные владельцы - иностранные компании
        /// </summary>
        [JsonProperty("beneficialOwnersForeign", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerForeign[] BeneficialOwnersForeign { get; set; }

        /// <summary>
        /// Конечные владельцы - без категории. Это могут быть юрлица, физлица и иностранные лица
        /// </summary>
        [JsonProperty("beneficialOwnersOther", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerOther[] BeneficialOwnersOther { get; set; }

        /// <summary>
        /// Конечные владельцы - юрлица
        /// </summary>
        [JsonProperty("beneficialOwnersUL", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerUl[] BeneficialOwnersUl { get; set; }
    }

    public partial class BeneficialOwnerFl
    {
        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ИННФЛ
        /// </summary>
        [JsonProperty("innfl", NullValueHandling = NullValueHandling.Ignore)]
        public string Innfl { get; set; }

        /// <summary>
        /// Признак точной доли
        /// </summary>
        [JsonProperty("isAccurate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAccurate { get; set; }

        /// <summary>
        /// Размер доли в процентах. Доля вычисляется по цепочке учредителей и акционеров.
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public double? Share { get; set; }
    }

    public partial class BeneficialOwnerForeign
    {
        /// <summary>
        /// Страна
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// Полное наименование юридического лица
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Признак точной доли
        /// </summary>
        [JsonProperty("isAccurate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAccurate { get; set; }

        /// <summary>
        /// Размер доли в процентах. Доля вычисляется по цепочке учредителей и акционеров.
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public double? Share { get; set; }
    }

    public partial class BeneficialOwnerOther
    {
        /// <summary>
        /// Полное наименование лица
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Признак точной доли
        /// </summary>
        [JsonProperty("isAccurate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAccurate { get; set; }

        /// <summary>
        /// Размер доли в процентах. Доля вычисляется по цепочке учредителей и акционеров.
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public double? Share { get; set; }
    }

    public partial class BeneficialOwnerUl
    {
        /// <summary>
        /// Полное наименование юридического лица
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Признак точной доли
        /// </summary>
        [JsonProperty("isAccurate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAccurate { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Размер доли в процентах. Доля вычисляется по цепочке учредителей и акционеров.
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public double? Share { get; set; }
    }

    /// <summary>
    /// Исторические конечные владельцы
    /// </summary>
    public partial class HistoricalBeneficialOwners
    {
        /// <summary>
        /// Конечные владельцы - физлица
        /// </summary>
        [JsonProperty("beneficialOwnersFL", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerFl[] BeneficialOwnersFl { get; set; }

        /// <summary>
        /// Конечные владельцы - иностранные компании
        /// </summary>
        [JsonProperty("beneficialOwnersForeign", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerForeign[] BeneficialOwnersForeign { get; set; }

        /// <summary>
        /// Конечные владельцы - без категории. Это могут быть юрлица, физлица и иностранные лица
        /// </summary>
        [JsonProperty("beneficialOwnersOther", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerOther[] BeneficialOwnersOther { get; set; }

        /// <summary>
        /// Конечные владельцы - юрлица
        /// </summary>
        [JsonProperty("beneficialOwnersUL", NullValueHandling = NullValueHandling.Ignore)]
        public BeneficialOwnerUl[] BeneficialOwnersUl { get; set; }
    }
}
