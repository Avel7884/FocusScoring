namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class EgrDetailsValue : IParameterValue
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
        public IpEgr Ip { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Информация о юридическом лице
        /// </summary>
        [JsonProperty("UL", NullValueHandling = NullValueHandling.Ignore)]
        public UlEgr Ul { get; set; }
    }

    /// <summary>
    /// Информация об индивидуальном предпринимателе
    /// </summary>
    public partial class IpEgr
    {
        /// <summary>
        /// Виды деятельности
        /// </summary>
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public IpActivities Activities { get; set; }

        /// <summary>
        /// Записи в ЕГРИП
        /// </summary>
        [JsonProperty("egrRecords", NullValueHandling = NullValueHandling.Ignore)]
        public IpEgrRecord[] EgrRecords { get; set; }

        /// <summary>
        /// Регистрационный номер ФОМС
        /// </summary>
        [JsonProperty("fomsRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string FomsRegNumber { get; set; }

        /// <summary>
        /// Регистрационный номер ФСС
        /// </summary>
        [JsonProperty("fssRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string FssRegNumber { get; set; }

        /// <summary>
        /// Сведения о постановке на учет в налоговом органе
        /// </summary>
        [JsonProperty("nalogRegBody", NullValueHandling = NullValueHandling.Ignore)]
        public NalogRegBody NalogRegBody { get; set; }

        /// <summary>
        /// ОКАТО (может отсутствовать или устареть)
        /// </summary>
        [JsonProperty("okato", NullValueHandling = NullValueHandling.Ignore)]
        public string Okato { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        [JsonProperty("okpo", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpo { get; set; }

        /// <summary>
        /// Регистрационный номер ПФР
        /// </summary>
        [JsonProperty("pfrRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PfrRegNumber { get; set; }

        /// <summary>
        /// Сведения регистрирующего органа
        /// </summary>
        [JsonProperty("regBody", NullValueHandling = NullValueHandling.Ignore)]
        public NalogRegBody RegBody { get; set; }

        /// <summary>
        /// Сведения о регистрации
        /// </summary>
        [JsonProperty("regInfo", NullValueHandling = NullValueHandling.Ignore)]
        public IpRegInfo RegInfo { get; set; }

        /// <summary>
        /// Информация о местонахождении ИП (может отсутствовать или устареть)
        /// </summary>
        [JsonProperty("shortenedAddress", NullValueHandling = NullValueHandling.Ignore)]
        public ShortenedAddress ShortenedAddress { get; set; }
    }

    /// <summary>
    /// Виды деятельности
    /// </summary>
    public partial class IpActivities
    {
        /// <summary>
        /// Дополнительные виды деятельности
        /// </summary>
        [JsonProperty("complementaryActivities", NullValueHandling = NullValueHandling.Ignore)]
        public Activity[] ComplementaryActivities { get; set; }

        /// <summary>
        /// Версия справочника ОКВЭД
        /// </summary>
        [JsonProperty("okvedVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string OkvedVersion { get; set; }

        /// <summary>
        /// Основной вид деятельности
        /// </summary>
        [JsonProperty("principalActivity", NullValueHandling = NullValueHandling.Ignore)]
        public Activity PrincipalActivity { get; set; }
    }

    /// <summary>
    /// Основной вид деятельности
    /// </summary>
    public partial class Activity
    {
        /// <summary>
        /// Код вида деятельности
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Название вида деятельности
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public partial class IpEgrRecord
    {
        /// <summary>
        /// Свидетельства, подтверждающие факт внесения записи
        /// </summary>
        [JsonProperty("certificates", NullValueHandling = NullValueHandling.Ignore)]
        public Certificate[] Certificates { get; set; }

        /// <summary>
        /// Дата внесения записи
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Документы, представленные при внесении записи
        /// </summary>
        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public Document[] Documents { get; set; }

        /// <summary>
        /// ГРН записи
        /// </summary>
        [JsonProperty("grn", NullValueHandling = NullValueHandling.Ignore)]
        public string Grn { get; set; }

        /// <summary>
        /// Дата, когда запись стала недействительной
        /// </summary>
        [JsonProperty("invalidSince", NullValueHandling = NullValueHandling.Ignore)]
        public string InvalidSince { get; set; }

        /// <summary>
        /// Признак недействительности записи
        /// </summary>
        [JsonProperty("isInvalid", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsInvalid { get; set; }

        /// <summary>
        /// Причина внесения записи
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Код регистрирующего органа, который внес запись
        /// </summary>
        [JsonProperty("regCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RegCode { get; set; }

        /// <summary>
        /// Имя регистрирующего органа, который внес запись
        /// </summary>
        [JsonProperty("regName", NullValueHandling = NullValueHandling.Ignore)]
        public string RegName { get; set; }
    }

    public partial class Certificate
    {
        /// <summary>
        /// Дата выдачи
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Серия и номер
        /// </summary>
        [JsonProperty("serialNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string SerialNumber { get; set; }
    }

    public partial class Document
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Имя документа
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Сведения о постановке на учет в налоговом органе
    ///
    /// Сведения регистрирующего органа
    /// </summary>
    public partial class NalogRegBody
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Код налогового органа
        /// </summary>
        [JsonProperty("nalogCode", NullValueHandling = NullValueHandling.Ignore)]
        public string NalogCode { get; set; }

        /// <summary>
        /// Наименование налогового органа
        /// </summary>
        [JsonProperty("nalogName", NullValueHandling = NullValueHandling.Ignore)]
        public string NalogName { get; set; }

        /// <summary>
        /// Дата постановки на учет
        /// </summary>
        [JsonProperty("nalogRegDate", NullValueHandling = NullValueHandling.Ignore)]
        public string NalogRegDate { get; set; }
    }

    /// <summary>
    /// Сведения о регистрации
    /// </summary>
    public partial class IpRegInfo
    {
        /// <summary>
        /// Дата присвоения ОГРНИП
        /// </summary>
        [JsonProperty("ogrnDate", NullValueHandling = NullValueHandling.Ignore)]
        public string OgrnDate { get; set; }

        /// <summary>
        /// Наименование органа, зарегистрировавшего индивидуального предпринимателя до 1 января 2004
        /// года
        /// </summary>
        [JsonProperty("regName", NullValueHandling = NullValueHandling.Ignore)]
        public string RegName { get; set; }
    }

    /// <summary>
    /// Информация о местонахождении ИП (может отсутствовать или устареть)
    /// </summary>
    public partial class ShortenedAddress
    {
        /// <summary>
        /// Город
        /// </summary>
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public Toponym City { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        [JsonProperty("district", NullValueHandling = NullValueHandling.Ignore)]
        public Toponym District { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        [JsonProperty("regionCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RegionCode { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        [JsonProperty("regionName", NullValueHandling = NullValueHandling.Ignore)]
        public Toponym RegionName { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        [JsonProperty("settlement", NullValueHandling = NullValueHandling.Ignore)]
        public Toponym Settlement { get; set; }
    }

    /// <summary>
    /// Город
    ///
    /// Район
    ///
    /// Регион
    ///
    /// Населенный пункт
    /// </summary>
    public partial class Toponym
    {
        /// <summary>
        /// Полное наименование вида топонима
        /// </summary>
        [JsonProperty("topoFullName", NullValueHandling = NullValueHandling.Ignore)]
        public string TopoFullName { get; set; }

        /// <summary>
        /// Краткое наименование вида топонима
        /// </summary>
        [JsonProperty("topoShortName", NullValueHandling = NullValueHandling.Ignore)]
        public string TopoShortName { get; set; }

        /// <summary>
        /// Значение топонима
        /// </summary>
        [JsonProperty("topoValue", NullValueHandling = NullValueHandling.Ignore)]
        public string TopoValue { get; set; }
    }

    /// <summary>
    /// Информация о юридическом лице
    /// </summary>
    public partial class UlEgr
    {
        /// <summary>
        /// Виды деятельности
        /// </summary>
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public UlActivities Activities { get; set; }

        /// <summary>
        /// Записи в ЕГРЮЛ
        /// </summary>
        [JsonProperty("egrRecords", NullValueHandling = NullValueHandling.Ignore)]
        public UlEgrRecord[] EgrRecords { get; set; }

        /// <summary>
        /// Регистрационный номер ФОМС
        /// </summary>
        [JsonProperty("fomsRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string FomsRegNumber { get; set; }

        /// <summary>
        /// Учредители - физлица
        /// </summary>
        [JsonProperty("foundersFL", NullValueHandling = NullValueHandling.Ignore)]
        public FounderFl[] FoundersFl { get; set; }

        /// <summary>
        /// Учредители - иностранные компании
        /// </summary>
        [JsonProperty("foundersForeign", NullValueHandling = NullValueHandling.Ignore)]
        public FounderForeign[] FoundersForeign { get; set; }

        /// <summary>
        /// Учредители - юрлица
        /// </summary>
        [JsonProperty("foundersUL", NullValueHandling = NullValueHandling.Ignore)]
        public FounderUl[] FoundersUl { get; set; }

        /// <summary>
        /// Регистрационный номер ФСС
        /// </summary>
        [JsonProperty("fssRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string FssRegNumber { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public History History { get; set; }

        /// <summary>
        /// Сведения о постановке на учет в налоговом органе
        /// </summary>
        [JsonProperty("nalogRegBody", NullValueHandling = NullValueHandling.Ignore)]
        public NalogRegBody NalogRegBody { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        [JsonProperty("okpo", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpo { get; set; }

        /// <summary>
        /// Регистрационный номер ПФР
        /// </summary>
        [JsonProperty("pfrRegNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PfrRegNumber { get; set; }

        /// <summary>
        /// Предшественники
        /// </summary>
        [JsonProperty("predecessors", NullValueHandling = NullValueHandling.Ignore)]
        public Predecessor[] Predecessors { get; set; }

        /// <summary>
        /// Сведения регистрирующего органа
        /// </summary>
        [JsonProperty("regBody", NullValueHandling = NullValueHandling.Ignore)]
        public NalogRegBody RegBody { get; set; }

        /// <summary>
        /// Сведения о регистрации
        /// </summary>
        [JsonProperty("regInfo", NullValueHandling = NullValueHandling.Ignore)]
        public UlRegInfo RegInfo { get; set; }

        /// <summary>
        /// Акционеры, являющиеся аффилированными лицами
        /// </summary>
        [JsonProperty("shareholders", NullValueHandling = NullValueHandling.Ignore)]
        public Shareholders Shareholders { get; set; }

        /// <summary>
        /// Уставный капитал
        /// </summary>
        [JsonProperty("statedCapital", NullValueHandling = NullValueHandling.Ignore)]
        public StatedCapital StatedCapital { get; set; }

        /// <summary>
        /// Преемники
        /// </summary>
        [JsonProperty("successors", NullValueHandling = NullValueHandling.Ignore)]
        public Successor[] Successors { get; set; }
    }

    /// <summary>
    /// Виды деятельности
    /// </summary>
    public partial class UlActivities
    {
        /// <summary>
        /// Дополнительные виды деятельности
        /// </summary>
        [JsonProperty("complementaryActivities", NullValueHandling = NullValueHandling.Ignore)]
        public Activity[] ComplementaryActivities { get; set; }

        /// <summary>
        /// Версия справочника ОКВЭД. Значение '2' соответствует ОК 029-2014 (КДЕС Ред. 2),
        /// отсутствие поля версии соответствует ОК 029-2001 (КДЕС Ред.1)
        /// </summary>
        [JsonProperty("okvedVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string OkvedVersion { get; set; }

        /// <summary>
        /// Основной вид деятельности
        /// </summary>
        [JsonProperty("principalActivity", NullValueHandling = NullValueHandling.Ignore)]
        public Activity PrincipalActivity { get; set; }
    }

    public partial class UlEgrRecord
    {
        /// <summary>
        /// Свидетельства, подтверждающие факт внесения записи
        /// </summary>
        [JsonProperty("certificates", NullValueHandling = NullValueHandling.Ignore)]
        public Certificate[] Certificates { get; set; }

        /// <summary>
        /// Дата внесения записи
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Документы, представленные при внесении записи
        /// </summary>
        [JsonProperty("documents", NullValueHandling = NullValueHandling.Ignore)]
        public Document[] Documents { get; set; }

        /// <summary>
        /// ГРН записи
        /// </summary>
        [JsonProperty("grn", NullValueHandling = NullValueHandling.Ignore)]
        public string Grn { get; set; }

        /// <summary>
        /// Дата, когда запись стала недействительной
        /// </summary>
        [JsonProperty("invalidSince", NullValueHandling = NullValueHandling.Ignore)]
        public string InvalidSince { get; set; }

        /// <summary>
        /// Признак недействительности записи
        /// </summary>
        [JsonProperty("isInvalid", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsInvalid { get; set; }

        /// <summary>
        /// Причина внесения записи
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Код регистрирующего органа, который внес запись
        /// </summary>
        [JsonProperty("regCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RegCode { get; set; }

        /// <summary>
        /// Имя регистрирующего органа, который внес запись
        /// </summary>
        [JsonProperty("regName", NullValueHandling = NullValueHandling.Ignore)]
        public string RegName { get; set; }
    }

    public partial class FounderFl
    {
        /// <summary>
        /// Дата последнего внесения изменений
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// Дата первого внесения сведений
        /// </summary>
        [JsonProperty("firstDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstDate { get; set; }

        /// <summary>
        /// ИННФЛ
        /// </summary>
        [JsonProperty("innfl", NullValueHandling = NullValueHandling.Ignore)]
        public string Innfl { get; set; }

        /// <summary>
        /// Сведения об обременении доли участника
        /// </summary>
        [JsonProperty("pledges", NullValueHandling = NullValueHandling.Ignore)]
        public PledgeInfo[] Pledges { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public Share Share { get; set; }
    }

    public partial class PledgeInfo
    {
        /// <summary>
        /// Срок обременения или порядок определения срока
        /// </summary>
        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public string Duration { get; set; }

        /// <summary>
        /// Залогодержатель — физлицо
        /// </summary>
        [JsonProperty("pledgeeFL", NullValueHandling = NullValueHandling.Ignore)]
        public PledgeeFl PledgeeFl { get; set; }

        /// <summary>
        /// Залогодержатель — юрлицо
        /// </summary>
        [JsonProperty("pledgeeUL", NullValueHandling = NullValueHandling.Ignore)]
        public PledgeeUl PledgeeUl { get; set; }
    }

    /// <summary>
    /// Залогодержатель — физлицо
    /// </summary>
    public partial class PledgeeFl
    {
        /// <summary>
        /// ФИО физического лица
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ИННФЛ
        /// </summary>
        [JsonProperty("innfl", NullValueHandling = NullValueHandling.Ignore)]
        public string Innfl { get; set; }

        /// <summary>
        /// Сведения о нотариальном удостоверении договора залога
        /// </summary>
        [JsonProperty("notarization", NullValueHandling = NullValueHandling.Ignore)]
        public Notarization Notarization { get; set; }
    }

    /// <summary>
    /// Сведения о нотариальном удостоверении договора залога
    /// </summary>
    public partial class Notarization
    {
        /// <summary>
        /// Номер договора залога
        /// </summary>
        [JsonProperty("contractNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Дата договора залога
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// ФИО нотариуса
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ИННФЛ нотариуса
        /// </summary>
        [JsonProperty("innfl", NullValueHandling = NullValueHandling.Ignore)]
        public string Innfl { get; set; }
    }

    /// <summary>
    /// Залогодержатель — юрлицо
    /// </summary>
    public partial class PledgeeUl
    {
        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Краткое наименование юридического лица
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Сведения о нотариальном удостоверении договора залога
        /// </summary>
        [JsonProperty("notarization", NullValueHandling = NullValueHandling.Ignore)]
        public Notarization Notarization { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    /// <summary>
    /// Доля
    /// </summary>
    public partial class Share
    {
        /// <summary>
        /// Размер доли в виде простой дроби (знаменатель)
        /// </summary>
        [JsonProperty("percentageDenominator", NullValueHandling = NullValueHandling.Ignore)]
        public long? PercentageDenominator { get; set; }

        /// <summary>
        /// Размер доли в виде простой дроби (числитель)
        /// </summary>
        [JsonProperty("percentageNominator", NullValueHandling = NullValueHandling.Ignore)]
        public long? PercentageNominator { get; set; }

        /// <summary>
        /// Размер доли в процентах
        /// </summary>
        [JsonProperty("percentagePlain", NullValueHandling = NullValueHandling.Ignore)]
        public double? PercentagePlain { get; set; }

        /// <summary>
        /// Сумма в рублях
        /// </summary>
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double? Sum { get; set; }
    }

    public partial class FounderForeign
    {
        /// <summary>
        /// Страна
        /// </summary>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        /// <summary>
        /// Дата последнего внесения изменений
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Дата первого внесения сведений
        /// </summary>
        [JsonProperty("firstDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstDate { get; set; }

        /// <summary>
        /// Полное наименование юридического лица
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Сведения об обременении доли участника
        /// </summary>
        [JsonProperty("pledges", NullValueHandling = NullValueHandling.Ignore)]
        public PledgeInfo[] Pledges { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public Share Share { get; set; }
    }

    public partial class FounderUl
    {
        /// <summary>
        /// Дата последнего внесения изменений
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Дата первого внесения сведений
        /// </summary>
        [JsonProperty("firstDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstDate { get; set; }

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
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Сведения об обременении доли участника
        /// </summary>
        [JsonProperty("pledges", NullValueHandling = NullValueHandling.Ignore)]
        public PledgeInfo[] Pledges { get; set; }

        /// <summary>
        /// Доля
        /// </summary>
        [JsonProperty("share", NullValueHandling = NullValueHandling.Ignore)]
        public Share Share { get; set; }
    }

    public partial class History
    {
        /// <summary>
        /// Учредители - физлица (история изменений)
        /// </summary>
        [JsonProperty("foundersFL", NullValueHandling = NullValueHandling.Ignore)]
        public FounderFl[] FoundersFl { get; set; }

        /// <summary>
        /// Учредители - иностранные компании (история изменений)
        /// </summary>
        [JsonProperty("foundersForeign", NullValueHandling = NullValueHandling.Ignore)]
        public FounderForeign[] FoundersForeign { get; set; }

        /// <summary>
        /// Учредители - юрлица (история изменений)
        /// </summary>
        [JsonProperty("foundersUL", NullValueHandling = NullValueHandling.Ignore)]
        public FounderUl[] FoundersUl { get; set; }

        /// <summary>
        /// Бывшие акционеры или акционеры, переставшие относиться к группе аффилированных лиц
        /// </summary>
        [JsonProperty("shareholders", NullValueHandling = NullValueHandling.Ignore)]
        public Shareholders Shareholders { get; set; }

        /// <summary>
        /// Уставный капитал (история изменений)
        /// </summary>
        [JsonProperty("statedCapitals", NullValueHandling = NullValueHandling.Ignore)]
        public StatedCapital[] StatedCapitals { get; set; }
    }

    /// <summary>
    /// Бывшие акционеры или акционеры, переставшие относиться к группе аффилированных лиц
    ///
    /// Акционеры, являющиеся аффилированными лицами
    /// </summary>
    public partial class Shareholders
    {
        /// <summary>
        /// Дата последнего внесения изменений в список акционеров
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Акционеры - физлица
        /// </summary>
        [JsonProperty("shareholdersFL", NullValueHandling = NullValueHandling.Ignore)]
        public ShareholderFl[] ShareholdersFl { get; set; }

        /// <summary>
        /// Акционеры - без категории. Это могут быть юрлица, физлица и иностранные лица
        /// </summary>
        [JsonProperty("shareholdersOther", NullValueHandling = NullValueHandling.Ignore)]
        public ShareholderOther[] ShareholdersOther { get; set; }

        /// <summary>
        /// Акционеры - юрлица
        /// </summary>
        [JsonProperty("shareholdersUL", NullValueHandling = NullValueHandling.Ignore)]
        public ShareholderUl[] ShareholdersUl { get; set; }
    }

    public partial class ShareholderFl
    {
        /// <summary>
        /// Местожительство физлица
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Доля участия в уставном капитале в процентах
        /// </summary>
        [JsonProperty("capitalSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? CapitalSharesPercent { get; set; }

        /// <summary>
        /// Дата последнего изменения в долях
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// Доля обыкновенных акций в процентах
        /// </summary>
        [JsonProperty("votingSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? VotingSharesPercent { get; set; }
    }

    public partial class ShareholderOther
    {
        /// <summary>
        /// Местонахождение юрлица или Местожительство физлица
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Доля участия в уставном капитале в процентах
        /// </summary>
        [JsonProperty("capitalSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? CapitalSharesPercent { get; set; }

        /// <summary>
        /// Дата последнего изменения в долях
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Полное наименование лица
        /// </summary>
        [JsonProperty("fullName", NullValueHandling = NullValueHandling.Ignore)]
        public string FullName { get; set; }

        /// <summary>
        /// Доля обыкновенных акций в процентах
        /// </summary>
        [JsonProperty("votingSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? VotingSharesPercent { get; set; }
    }

    public partial class ShareholderUl
    {
        /// <summary>
        /// Местонахождение юрлица
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// Доля участия в уставном капитале в процентах
        /// </summary>
        [JsonProperty("capitalSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? CapitalSharesPercent { get; set; }

        /// <summary>
        /// Дата последнего изменения в долях
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

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
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Доля обыкновенных акций в процентах
        /// </summary>
        [JsonProperty("votingSharesPercent", NullValueHandling = NullValueHandling.Ignore)]
        public double? VotingSharesPercent { get; set; }
    }

    /// <summary>
    /// Уставный капитал
    /// </summary>
    public partial class StatedCapital
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Сумма в рублях (без учёта долей рублей)
        /// </summary>
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double? Sum { get; set; }
    }

    public partial class Predecessor
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    /// <summary>
    /// Сведения о регистрации
    /// </summary>
    public partial class UlRegInfo
    {
        /// <summary>
        /// Дата присвоения ОГРН
        /// </summary>
        [JsonProperty("ogrnDate", NullValueHandling = NullValueHandling.Ignore)]
        public string OgrnDate { get; set; }

        /// <summary>
        /// Наименование органа, зарегистрировавшего юридическое лицо до 1 июля 2002 года
        /// </summary>
        [JsonProperty("regName", NullValueHandling = NullValueHandling.Ignore)]
        public string RegName { get; set; }
    }

    public partial class Successor
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Наименование организации
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }
}
