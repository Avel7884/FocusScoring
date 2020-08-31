using System.Linq;
using System.Runtime.Remoting;
using System.Text;

namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class ReqValue : IParameterValue
    {
        /// <summary>
        /// Экспресс-отчет по контрагенту
        /// </summary>
        [JsonProperty("briefReport", NullValueHandling = NullValueHandling.Ignore)]
        public BriefReport BriefReport { get; set; }

        /// <summary>
        /// Контактные телефоны из Контур.Справочника (для получения контактов требуется отдельная
        /// подписка и вызов отдельного метода)
        /// </summary>
        [JsonProperty("contactPhones", NullValueHandling = NullValueHandling.Ignore)]
        public ContactPhones ContactPhones { get; set; }

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
        public Ip Ip { get; set; }

        /// <summary>
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Информация о юридическом лице
        /// </summary>
        [JsonProperty("UL", NullValueHandling = NullValueHandling.Ignore)]
        public Ul Ul { get; set; }
        
        [JsonIgnore]
        public string Address
        {
            get
            {
                var addr = Ul?.LegalAddress.ParsedAddressRf; //?? Ip.
                var parsed = new List<string>();
                
                if (addr.RegionName != null)
                {
                    parsed.Add(addr.RegionName.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.RegionName.TopoValue);
                }
                if (addr.City != null)
                {
                    parsed.Add(addr.City.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.City.TopoValue);
                }
                if (addr.Settlement != null)
                {
                    parsed.Add(addr.Settlement.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.Settlement.TopoValue);
                }
                if (addr.District != null)
                {
                    parsed.Add(addr.District.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.District.TopoValue);
                }
                if (addr.Street != null)
                {
                    parsed.Add(addr.Street.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.Street.TopoValue);
                }
                if (addr.House != null)
                {
                    parsed.Add(addr.House.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.House.TopoValue);
                }
                if (addr.Bulk != null)
                {
                    parsed.Add(addr.Bulk.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.Bulk.TopoValue);
                }
                if (addr.Flat != null)
                {
                    parsed.Add(addr.Flat.TopoShortName);
                    parsed.Add(". ");
                    parsed.Add(addr.Flat.TopoValue);
                }

                return string.Join(" ", parsed);
            }
        }

        public string Status() =>
            Ip?.Status.Value.String ?? Ul?.Status.Value.String; 
        
        public DateTime RegistrationDate => 
            DateTime.Parse(Ul?.RegistrationDate ?? Ip.RegistrationDate ?? throw new ServerException("Bad thing happened!"));
    }

    /// <summary>
    /// Экспресс-отчет по контрагенту
    /// </summary>
    public partial class BriefReport
    {
        /// <summary>
        /// Сводная информация из экспресс-отчета
        /// </summary>
        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public Summary Summary { get; set; }
    }

    /// <summary>
    /// Сводная информация из экспресс-отчета
    /// </summary>
    public partial class Summary
    {
        /// <summary>
        /// Наличие информации, помеченной зеленым цветом
        /// </summary>
        [JsonProperty("greenStatements", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GreenStatements { get; set; }

        /// <summary>
        /// Наличие информации, помеченной красным цветом
        /// </summary>
        [JsonProperty("redStatements", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RedStatements { get; set; }

        /// <summary>
        /// Наличие информации, помеченной желтым цветом
        /// </summary>
        [JsonProperty("yellowStatements", NullValueHandling = NullValueHandling.Ignore)]
        public bool? YellowStatements { get; set; }
    }

    /// <summary>
    /// Контактные телефоны из Контур.Справочника (для получения контактов требуется отдельная
    /// подписка и вызов отдельного метода)
    /// </summary>

    /// <summary>
    /// Информация об индивидуальном предпринимателе
    /// </summary>
    public partial class Ip
    {
        /// <summary>
        /// Дата прекращения деятельности
        /// </summary>
        [JsonProperty("dissolutionDate", NullValueHandling = NullValueHandling.Ignore)]
        public string DissolutionDate { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        [JsonProperty("fio", NullValueHandling = NullValueHandling.Ignore)]
        public string Fio { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        [JsonProperty("okato", NullValueHandling = NullValueHandling.Ignore)]
        public string Okato { get; set; }

        /// <summary>
        /// ОКФС
        /// </summary>
        [JsonProperty("okfs", NullValueHandling = NullValueHandling.Ignore)]
        public string Okfs { get; set; }

        /// <summary>
        /// ОКОГУ
        /// </summary>
        [JsonProperty("okogu", NullValueHandling = NullValueHandling.Ignore)]
        public string Okogu { get; set; }

        /// <summary>
        /// Код ОКОПФ
        /// </summary>
        [JsonProperty("okopf", NullValueHandling = NullValueHandling.Ignore)]
        public string Okopf { get; set; }

        /// <summary>
        /// ОКПО
        /// </summary>
        [JsonProperty("okpo", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpo { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        [JsonProperty("oktmo", NullValueHandling = NullValueHandling.Ignore)]
        public string Oktmo { get; set; }

        /// <summary>
        /// Наименование организационно-правовой формы
        /// </summary>
        [JsonProperty("opf", NullValueHandling = NullValueHandling.Ignore)]
        public string Opf { get; set; }

        /// <summary>
        /// Дата образования
        /// </summary>
        [JsonProperty("registrationDate", NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Статус ИП
        /// </summary>
        [JsonProperty("status")]
        public IpStatus? Status { get; set; }
    }

    public partial class PurpleStatus
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Недействующий
        /// </summary>
        [JsonProperty("dissolved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolved { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("statusString", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusString { get; set; }
    }

    /// <summary>
    /// Информация о юридическом лице
    /// </summary>
    public partial class Ul
    {
        /// <summary>
        /// Филиалы и представительства
        /// </summary>
        [JsonProperty("branches", NullValueHandling = NullValueHandling.Ignore)]
        public Branch[] Branches { get; set; }

        /// <summary>
        /// Дата прекращения деятельности в результате ликвидации, реорганизации или других событий
        /// </summary>
        [JsonProperty("dissolutionDate", NullValueHandling = NullValueHandling.Ignore)]
        public string DissolutionDate { get; set; }

        /// <summary>
        /// Лица, имеющие право подписи без доверенности (руководители)
        /// </summary>
        [JsonProperty("heads", NullValueHandling = NullValueHandling.Ignore)]
        public Head[] Heads { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public History History { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        [JsonProperty("legalAddress", NullValueHandling = NullValueHandling.Ignore)]
        public UlLegalAddress LegalAddress { get; set; }

        /// <summary>
        /// Наименование юридического лица
        /// </summary>
        [JsonProperty("legalName", NullValueHandling = NullValueHandling.Ignore)]
        public UlLegalName LegalName { get; set; }

        /// <summary>
        /// Управляющие компании
        /// </summary>
        [JsonProperty("managementCompanies", NullValueHandling = NullValueHandling.Ignore)]
        public ManagementCompany[] ManagementCompanies { get; set; }

        /// <summary>
        /// Код ОКАТО
        /// </summary>
        [JsonProperty("okato", NullValueHandling = NullValueHandling.Ignore)]
        public string Okato { get; set; }

        /// <summary>
        /// Код ОКФС
        /// </summary>
        [JsonProperty("okfs", NullValueHandling = NullValueHandling.Ignore)]
        public string Okfs { get; set; }

        /// <summary>
        /// Код ОКОГУ
        /// </summary>
        [JsonProperty("okogu", NullValueHandling = NullValueHandling.Ignore)]
        public string Okogu { get; set; }

        /// <summary>
        /// Код ОКОПФ
        /// </summary>
        [JsonProperty("okopf", NullValueHandling = NullValueHandling.Ignore)]
        public string Okopf { get; set; }

        /// <summary>
        /// Код ОКПО
        /// </summary>
        [JsonProperty("okpo", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpo { get; set; }

        /// <summary>
        /// Код ОКТМО
        /// </summary>
        [JsonProperty("oktmo", NullValueHandling = NullValueHandling.Ignore)]
        public string Oktmo { get; set; }

        /// <summary>
        /// Наименование организационно-правовой формы
        /// </summary>
        [JsonProperty("opf", NullValueHandling = NullValueHandling.Ignore)]
        public string Opf { get; set; }

        /// <summary>
        /// Дата образования
        /// </summary>
        [JsonProperty("registrationDate", NullValueHandling = NullValueHandling.Ignore)]
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Статус организации
        /// </summary>
        [JsonProperty("status")]
        public UlStatus? Status { get; set; }
    }

    public partial class Branch
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Адрес вне РФ
        /// </summary>
        [JsonProperty("foreignAddress", NullValueHandling = NullValueHandling.Ignore)]
        public ForeignAddress ForeignAddress { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp", NullValueHandling = NullValueHandling.Ignore)]
        public string Kpp { get; set; }

        /// <summary>
        /// Наименование филиала или представительства
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Разобранный на составляющие адрес в РФ
        /// </summary>
        [JsonProperty("parsedAddressRF", NullValueHandling = NullValueHandling.Ignore)]
        public BranchParsedAddressRf ParsedAddressRf { get; set; }
    }

    /// <summary>
    /// Адрес вне РФ
    /// </summary>
    public partial class ForeignAddress
    {
        /// <summary>
        /// Строка, содержащая адрес
        /// </summary>
        [JsonProperty("addressString", NullValueHandling = NullValueHandling.Ignore)]
        public string AddressString { get; set; }

        /// <summary>
        /// Наименование страны
        /// </summary>
        [JsonProperty("countryName", NullValueHandling = NullValueHandling.Ignore)]
        public string CountryName { get; set; }
    }

    /// <summary>
    /// Разобранный на составляющие адрес в РФ
    /// </summary>
    public partial class BranchParsedAddressRf
    {
        /// <summary>
        /// Корпус
        /// </summary>
        [JsonProperty("bulk", NullValueHandling = NullValueHandling.Ignore)]
        public Bulk Bulk { get; set; }

        /// <summary>
        /// Полное значение поля 'Корпус ' из ЕГРЮЛ
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
        /// Полное значение поля 'Дом ' из ЕГРЮЛ
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
    /// Корпус
    /// </summary>
    public partial class Bulk
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
    /// Город
    ///
    /// Корпус
    /// </summary>
    public partial class City
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
    /// Район
    ///
    /// Корпус
    /// </summary>
    public partial class District
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
    /// Офис/квартира/комната
    ///
    /// Корпус
    /// </summary>
    public partial class Flat
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
    /// Дом
    ///
    /// Корпус
    /// </summary>
    public partial class House
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
    /// Регион
    ///
    /// Корпус
    /// </summary>
    public partial class RegionName
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
    /// Населенный пункт
    ///
    /// Корпус
    /// </summary>
    public partial class Settlement
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
    /// Улица
    ///
    /// Корпус
    /// </summary>
    public partial class Street
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

    public partial class Head
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
        /// Должность
        /// </summary>
        [JsonProperty("position", NullValueHandling = NullValueHandling.Ignore)]
        public string Position { get; set; }
    }

    public partial class History
    {
        /// <summary>
        /// Филиалы и представительства из истории
        /// </summary>
        [JsonProperty("branches", NullValueHandling = NullValueHandling.Ignore)]
        public Branch[] Branches { get; set; }

        /// <summary>
        /// Лица, имеющие право подписи без доверенности (руководители) из истории
        /// </summary>
        [JsonProperty("heads", NullValueHandling = NullValueHandling.Ignore)]
        public Head[] Heads { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpps", NullValueHandling = NullValueHandling.Ignore)]
        public KppWithDate[] Kpps { get; set; }

        /// <summary>
        /// Список юридических адресов из истории
        /// </summary>
        [JsonProperty("legalAddresses", NullValueHandling = NullValueHandling.Ignore)]
        public LegalAddressElement[] LegalAddresses { get; set; }

        /// <summary>
        /// Наименование юридического лица
        /// </summary>
        [JsonProperty("legalNames", NullValueHandling = NullValueHandling.Ignore)]
        public LegalNameElement[] LegalNames { get; set; }

        /// <summary>
        /// Управляющие компании
        /// </summary>
        [JsonProperty("managementCompanies", NullValueHandling = NullValueHandling.Ignore)]
        public ManagementCompany[] ManagementCompanies { get; set; }
    }

    public partial class KppWithDate
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
    }

    public partial class LegalAddressElement
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Дата первого внесения сведений
        /// </summary>
        [JsonProperty("firstDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstDate { get; set; }

        /// <summary>
        /// Разобранный на составляющие адрес в РФ
        /// </summary>
        [JsonProperty("parsedAddressRF", NullValueHandling = NullValueHandling.Ignore)]
        public LegalAddressParsedAddressRf ParsedAddressRf { get; set; }
    }

    /// <summary>
    /// Разобранный на составляющие адрес в РФ
    /// </summary>
    public partial class LegalAddressParsedAddressRf
    {
        /// <summary>
        /// Корпус
        /// </summary>
        [JsonProperty("bulk", NullValueHandling = NullValueHandling.Ignore)]
        public Bulk Bulk { get; set; }

        /// <summary>
        /// Полное значение поля 'Корпус ' из ЕГРЮЛ
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
        /// Полное значение поля 'Дом ' из ЕГРЮЛ
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

    public partial class LegalNameElement
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

    public partial class ManagementCompany
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
        /// ИНН управляющей организации (если указан)
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Наименование управляющей организации
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// ОГРН управляющей организации (если указан)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }
    }

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public partial class UlLegalAddress
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Дата первого внесения сведений
        /// </summary>
        [JsonProperty("firstDate", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstDate { get; set; }

        /// <summary>
        /// Разобранный на составляющие адрес в РФ
        /// </summary>
        [JsonProperty("parsedAddressRF", NullValueHandling = NullValueHandling.Ignore)]
        public LegalAddressParsedAddressRf ParsedAddressRf { get; set; }
    }

    /// <summary>
    /// Наименование юридического лица
    /// </summary>
    public partial class UlLegalName
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

    public partial class FluffyStatus
    {
        /// <summary>
        /// Дата
        /// </summary>
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public string Date { get; set; }

        /// <summary>
        /// Недействующее
        /// </summary>
        [JsonProperty("dissolved", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolved { get; set; }

        /// <summary>
        /// В стадии ликвидации (либо планируется исключение из ЕГРЮЛ)
        /// </summary>
        [JsonProperty("dissolving", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Dissolving { get; set; }

        /// <summary>
        /// В процессе реорганизации (может прекратить деятельность в результате реорганизации)
        /// </summary>
        [JsonProperty("reorganizing", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Reorganizing { get; set; }

        /// <summary>
        /// Неформализованное описание статуса
        /// </summary>
        [JsonProperty("statusString", NullValueHandling = NullValueHandling.Ignore)]
        public string StatusString { get; set; }
    }

    /// <summary>
    /// Статус ИП
    /// </summary>
    public partial struct IpStatus
    {
        public object[] AnythingArray;
        public bool? Bool;
        public double? Double;
        public long? Integer;
        public PurpleStatus PurpleStatus;
        public string String;

        public static implicit operator IpStatus(object[] AnythingArray) => new IpStatus { AnythingArray = AnythingArray };
        public static implicit operator IpStatus(bool Bool) => new IpStatus { Bool = Bool };
        public static implicit operator IpStatus(double Double) => new IpStatus { Double = Double };
        public static implicit operator IpStatus(long Integer) => new IpStatus { Integer = Integer };
        public static implicit operator IpStatus(PurpleStatus PurpleStatus) => new IpStatus { PurpleStatus = PurpleStatus };
        public static implicit operator IpStatus(string String) => new IpStatus { String = String };
        public bool IsNull => AnythingArray == null && Bool == null && PurpleStatus == null && Double == null && Integer == null && String == null;
    }

    /// <summary>
    /// Статус организации
    /// </summary>
    public partial struct UlStatus
    {
        public object[] AnythingArray;
        public bool? Bool;
        public double? Double;
        public FluffyStatus FluffyStatus;
        public long? Integer;
        public string String;

        public static implicit operator UlStatus(object[] AnythingArray) => new UlStatus { AnythingArray = AnythingArray };
        public static implicit operator UlStatus(bool Bool) => new UlStatus { Bool = Bool };
        public static implicit operator UlStatus(double Double) => new UlStatus { Double = Double };
        public static implicit operator UlStatus(FluffyStatus FluffyStatus) => new UlStatus { FluffyStatus = FluffyStatus };
        public static implicit operator UlStatus(long Integer) => new UlStatus { Integer = Integer };
        public static implicit operator UlStatus(string String) => new UlStatus { String = String };
        public bool IsNull => AnythingArray == null && Bool == null && FluffyStatus == null && Double == null && Integer == null && String == null;
    }

    internal class IpStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(IpStatus) || t == typeof(IpStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return new IpStatus { };
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new IpStatus { Integer = integerValue };
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new IpStatus { Double = doubleValue };
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new IpStatus { Bool = boolValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new IpStatus { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<PurpleStatus>(reader);
                    return new IpStatus { PurpleStatus = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<object[]>(reader);
                    return new IpStatus { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type IpStatus");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (IpStatus)untypedValue;
            if (value.IsNull)
            {
                serializer.Serialize(writer, null);
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.PurpleStatus != null)
            {
                serializer.Serialize(writer, value.PurpleStatus);
                return;
            }
            throw new Exception("Cannot marshal type IpStatus");
        }

        public static readonly IpStatusConverter Singleton = new IpStatusConverter();
    }

    internal class UlStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(UlStatus) || t == typeof(UlStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return new UlStatus { };
                case JsonToken.Integer:
                    var integerValue = serializer.Deserialize<long>(reader);
                    return new UlStatus { Integer = integerValue };
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new UlStatus { Double = doubleValue };
                case JsonToken.Boolean:
                    var boolValue = serializer.Deserialize<bool>(reader);
                    return new UlStatus { Bool = boolValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new UlStatus { String = stringValue };
                case JsonToken.StartObject:
                    var objectValue = serializer.Deserialize<FluffyStatus>(reader);
                    return new UlStatus { FluffyStatus = objectValue };
                case JsonToken.StartArray:
                    var arrayValue = serializer.Deserialize<object[]>(reader);
                    return new UlStatus { AnythingArray = arrayValue };
            }
            throw new Exception("Cannot unmarshal type UlStatus");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (UlStatus)untypedValue;
            if (value.IsNull)
            {
                serializer.Serialize(writer, null);
                return;
            }
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value);
                return;
            }
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.Bool != null)
            {
                serializer.Serialize(writer, value.Bool.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            if (value.AnythingArray != null)
            {
                serializer.Serialize(writer, value.AnythingArray);
                return;
            }
            if (value.FluffyStatus != null)
            {
                serializer.Serialize(writer, value.FluffyStatus);
                return;
            }
            throw new Exception("Cannot marshal type UlStatus");
        }

        public static readonly UlStatusConverter Singleton = new UlStatusConverter();
    }
}