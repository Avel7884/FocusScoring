namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class FsaValue : IParameterValue
    {
        /// <summary>
        /// Ссылка на карточку юридического лица (ИП) в Контур.Фокусе (для работы требуется подписка
        /// на Контур.Фокус и дополнительная авторизация)
        /// </summary>
        [JsonProperty("focusHref", NullValueHandling = NullValueHandling.Ignore)]
        public string FocusHref { get; set; }

        /// <summary>
        /// Сертификаты и декларации соответствия юр. лица (ИП), выступающего в роли заявителя
        /// </summary>
        [JsonProperty("fsa", NullValueHandling = NullValueHandling.Ignore)]
        public Fsa[] Fsa { get; set; }

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
    }

    public partial class Fsa
    {
        /// <summary>
        /// Роль заявителя
        /// </summary>
        [JsonProperty("applicantRole", NullValueHandling = NullValueHandling.Ignore)]
        public string ApplicantRole { get; set; }

        /// <summary>
        /// Вид сертификата или декларации соответствия
        /// </summary>
        [JsonProperty("detailedType", NullValueHandling = NullValueHandling.Ignore)]
        public string DetailedType { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        [JsonProperty("endDate", NullValueHandling = NullValueHandling.Ignore)]
        public string EndDate { get; set; }

        /// <summary>
        /// Сведения об изготовителе
        /// </summary>
        [JsonProperty("manufacturer", NullValueHandling = NullValueHandling.Ignore)]
        public Manufacturer Manufacturer { get; set; }

        /// <summary>
        /// Регистрационный номер сертификата или декларации соответствия
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// Информация о продукции
        /// </summary>
        [JsonProperty("product", NullValueHandling = NullValueHandling.Ignore)]
        public Product Product { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }

        /// <summary>
        /// Неформализованное описание статуса сертификата или декларации соответствия
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Тип объекта: Сertificate - сертификат соответствия, Declaration - декларация о
        /// соответствии
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeEnum? Type { get; set; }
    }

    /// <summary>
    /// Сведения об изготовителе
    /// </summary>
    public partial class Manufacturer
    {
        /// <summary>
        /// Адрес места нахождения ЮЛ/ИнЮЛ или жительства ИП
        /// </summary>
        [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Полное наименование (ФИО у ИП)
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
    /// Информация о продукции
    /// </summary>
    public partial class Product
    {
        /// <summary>
        /// Полное наименование
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Код ОКП
        /// </summary>
        [JsonProperty("okp", NullValueHandling = NullValueHandling.Ignore)]
        public string Okp { get; set; }

        /// <summary>
        /// Код ОКПД 2
        /// </summary>
        [JsonProperty("okpd2", NullValueHandling = NullValueHandling.Ignore)]
        public string Okpd2 { get; set; }

        /// <summary>
        /// Код ТН ВЭД
        /// </summary>
        [JsonProperty("tnVed", NullValueHandling = NullValueHandling.Ignore)]
        public string TnVed { get; set; }
    }

    /// <summary>
    /// Тип объекта: Сertificate - сертификат соответствия, Declaration - декларация о
    /// соответствии
    /// </summary>
    public enum TypeEnum { Certificate, Declaration };
    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Certificate":
                    return TypeEnum.Certificate;
                case "Declaration":
                    return TypeEnum.Declaration;
            }
            throw new Exception("Cannot unmarshal type TypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Certificate:
                    serializer.Serialize(writer, "Certificate");
                    return;
                case TypeEnum.Declaration:
                    serializer.Serialize(writer, "Declaration");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }
}
