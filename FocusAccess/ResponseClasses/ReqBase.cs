namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ReqBaseValue : IParameterValue
    {
        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) в Контур.Фокусе (для работы требуется подписка
        /// на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// ИНН(ИП)
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Информация об индивидуальном предпринимателе
        /// </summary>
        [JsonProperty("IP", NullValueHandling = NullValueHandling.Ignore)]
        public IpBase Ip { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Информация о юридическом лице
        /// </summary>
        [JsonProperty("UL", NullValueHandling = NullValueHandling.Ignore)]
        public UlBase Ul { get; set; }
    }

    /// <summary>
    /// Информация об индивидуальном предпринимателе
    /// </summary>
    public partial class IpBase
    {
        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }
    }

    /// <summary>
    /// Информация о юридическом лице
    /// </summary>
    public partial class UlBase
    {
        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [JsonProperty("legalAddress", NullValueHandling = NullValueHandling.Ignore)]
        public LegalAddress LegalAddress { get; set; }

        /// <summary>
        /// Наименование юридического лица
        /// </summary>
        [JsonProperty("legalName", NullValueHandling = NullValueHandling.Ignore)]
        public LegalName LegalName { get; set; }
    }

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public partial class LegalAddress
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Разобранный на составляющие адрес в РФ
        /// </summary>
        [JsonProperty("parsedAddressRF", NullValueHandling = NullValueHandling.Ignore)]
        public ParsedAddressRf ParsedAddressRf { get; set; }
    }

    /// <summary>
    /// Разобранный на составляющие адрес в РФ
    /// </summary>
    public partial class ParsedAddressRf
    {
        /// <summary>
        /// Корпус
        /// </summary>
        [JsonProperty("bulk", NullValueHandling = NullValueHandling.Ignore)]
        public Bulk Bulk { get; set; }

        /// <summary>
        /// Полное значение поля 'Корпус' из ЕГРЮЛ
        /// </summary>
        [JsonProperty("bulkRaw", NullValueHandling = NullValueHandling.Ignore)]
        public string BulkRaw { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public City City { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public District District { get; set; }

        /// <summary>
        /// Офис/квартира/комната
        /// </summary>
        [JsonProperty("flat", NullValueHandling = NullValueHandling.Ignore)]
        public Flat Flat { get; set; }

        /// <summary>
        /// Полное значение поля 'Квартира' из ЕГРЮЛ
        /// </summary>
        [JsonProperty("flatRaw", NullValueHandling = NullValueHandling.Ignore)]
        public string FlatRaw { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        [JsonProperty("house", NullValueHandling = NullValueHandling.Ignore)]
        public House House { get; set; }

        /// <summary>
        /// Полное значение поля 'Дом' из ЕГРЮЛ
        /// </summary>
        [JsonProperty("houseRaw", NullValueHandling = NullValueHandling.Ignore)]
        public string HouseRaw { get; set; }

        /// <summary>
        /// Код КЛАДР
        /// </summary>
        [JsonProperty("kladrCode", NullValueHandling = NullValueHandling.Ignore)]
        public string KladrCode { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        [JsonProperty("regionCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionCode { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        [JsonProperty("regionName", NullValueHandling = NullValueHandling.Ignore)]
        public RegionName RegionName { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        [JsonProperty("settlement", NullValueHandling = NullValueHandling.Ignore)]
        public Settlement Settlement { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        [JsonProperty("street", NullValueHandling = NullValueHandling.Ignore)]
        public Street Street { get; set; }

        /// <summary>
        /// Индекс
        /// </summary>
        [JsonProperty("zipCode", NullValueHandling = NullValueHandling.Ignore)]
        public string ZipCode { get; set; }
    }

    /// <summary>
    /// Наименование юридического лица
    /// </summary>
    public partial class LegalName
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Полное наименование организации
        /// </summary>
        [JsonProperty("full", NullValueHandling = NullValueHandling.Ignore)]
        public string Full { get; set; }

        /// <summary>
        /// Краткое наименование организации
        /// </summary>
        [JsonProperty("short", NullValueHandling = NullValueHandling.Ignore)]
        public string Short { get; set; }
    }
}
