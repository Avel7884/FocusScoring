namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class BankGuaranteesValue : IParameterValue
    {
        /// <summary>
        /// Банковские гарантии
        /// </summary>
        [JsonProperty("bankGuarantees", NullValueHandling = NullValueHandling.Ignore)]
        public BankGuarantee[] BankGuarantees { get; set; }

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
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Сторона банковской гарантии: Principal - принципал, Bank - банк-гарант
        /// </summary>
        [JsonProperty("role", NullValueHandling = NullValueHandling.Ignore)]
        public Role? Role { get; set; }
    }

    public partial class BankGuarantee
    {
        /// <summary>
        /// Вид обеспечения банковской гарантии: Purchase - обеспечение заявки, Contract -
        /// обеспечение исполнения контракта
        /// </summary>
        [JsonProperty("assuranceType", NullValueHandling = NullValueHandling.Ignore)]
        public AssuranceType? AssuranceType { get; set; }

        /// <summary>
        /// Банк-гарант
        /// </summary>
        [JsonProperty("bank", NullValueHandling = NullValueHandling.Ignore)]
        public Requisites Bank { get; set; }

        /// <summary>
        /// Заказчик-бенефициар
        /// </summary>
        [JsonProperty("beneficiary", NullValueHandling = NullValueHandling.Ignore)]
        public Requisites Beneficiary { get; set; }

        /// <summary>
        /// Дата окончания срока действия банковской гарантии
        /// </summary>
        [JsonProperty("endDate", NullValueHandling = NullValueHandling.Ignore)]
        public string EndDate { get; set; }

        /// <summary>
        /// Сумма обеспечения по банковской гарантии
        /// </summary>
        [JsonProperty("guaranteeSum", NullValueHandling = NullValueHandling.Ignore)]
        public GuaranteeSum GuaranteeSum { get; set; }

        /// <summary>
        /// Реестровый номер банковской гарантии
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }

        /// <summary>
        /// Поставщик (подрядчик, исполнитель) - принципал
        /// </summary>
        [JsonProperty("principal", NullValueHandling = NullValueHandling.Ignore)]
        public Requisites Principal { get; set; }

        /// <summary>
        /// Номер закупки
        /// </summary>
        [JsonProperty("purchaseNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PurchaseNumber { get; set; }

        /// <summary>
        /// Дата выдачи банковской гарантии
        /// </summary>
        [JsonProperty("releaseDate", NullValueHandling = NullValueHandling.Ignore)]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Дата вступления в силу банковской гарантии
        /// </summary>
        [JsonProperty("startDate", NullValueHandling = NullValueHandling.Ignore)]
        public string StartDate { get; set; }

        /// <summary>
        /// Неформализованное описание статуса банковской гарантии
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }

    /// <summary>
    /// Банк-гарант
    ///
    /// Заказчик-бенефициар
    ///
    /// Поставщик (подрядчик, исполнитель) - принципал
    /// </summary>
    public partial class Requisites
    {
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
        /// Наименование
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }

    /// <summary>
    /// Сумма обеспечения по банковской гарантии
    /// </summary>
    public partial class GuaranteeSum
    {
        /// <summary>
        /// Код валюты. Указан по общероссийскому классификатору валют. Российский рубль - RUB, Евро
        /// - EUR, Доллар США - USD.
        /// </summary>
        [JsonProperty("currencyCode", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Сумма обеспечения по банковской гарантии
        /// </summary>
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double? Sum { get; set; }
    }

    /// <summary>
    /// Вид обеспечения банковской гарантии: Purchase - обеспечение заявки, Contract -
    /// обеспечение исполнения контракта
    /// </summary>
    public enum AssuranceType { Contract, Purchase };

    /// <summary>
    /// Сторона банковской гарантии: Principal - принципал, Bank - банк-гарант
    /// </summary>
    public enum Role { Bank, Principal };

    internal class AssuranceTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AssuranceType) || t == typeof(AssuranceType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Contract":
                    return AssuranceType.Contract;
                case "Purchase":
                    return AssuranceType.Purchase;
            }
            throw new Exception("Cannot unmarshal type AssuranceType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AssuranceType)untypedValue;
            switch (value)
            {
                case AssuranceType.Contract:
                    serializer.Serialize(writer, "Contract");
                    return;
                case AssuranceType.Purchase:
                    serializer.Serialize(writer, "Purchase");
                    return;
            }
            throw new Exception("Cannot marshal type AssuranceType");
        }

        public static readonly AssuranceTypeConverter Singleton = new AssuranceTypeConverter();
    }

    internal class RoleConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Role) || t == typeof(Role?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Bank":
                    return Role.Bank;
                case "Principal":
                    return Role.Principal;
            }
            throw new Exception("Cannot unmarshal type Role");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Role)untypedValue;
            switch (value)
            {
                case Role.Bank:
                    serializer.Serialize(writer, "Bank");
                    return;
                case Role.Principal:
                    serializer.Serialize(writer, "Principal");
                    return;
            }
            throw new Exception("Cannot marshal type Role");
        }

        public static readonly RoleConverter Singleton = new RoleConverter();
    }
}
