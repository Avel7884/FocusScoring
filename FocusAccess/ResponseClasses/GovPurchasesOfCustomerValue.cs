namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GovPurchasesOfCustomerValue : IParameterValue
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
        /// ОГРН(ИП)
        /// </summary>
        [JsonProperty("ogrn", NullValueHandling = NullValueHandling.Ignore)]
        public string Ogrn { get; set; }

        /// <summary>
        /// Закупки заказчика
        /// </summary>
        [JsonProperty("purchasesOfCustomer", NullValueHandling = NullValueHandling.Ignore)]
        public PurchasesOfCustomer[] PurchasesOfCustomer { get; set; }
    }

    public partial class PurchasesOfCustomer
    {
        /// <summary>
        /// Причина расторжения контракта/договора
        /// </summary>
        [JsonProperty("contractCanceledReason", NullValueHandling = NullValueHandling.Ignore)]
        public string ContractCanceledReason { get; set; }

        /// <summary>
        /// Дата контракта/договора
        /// </summary>
        [JsonProperty("contractDate", NullValueHandling = NullValueHandling.Ignore)]
        public string ContractDate { get; set; }

        /// <summary>
        /// Вид обеспечения исполнения контракта: денежная сумма или банковская гарантия.
        /// </summary>
        [JsonProperty("contractEnforcement", NullValueHandling = NullValueHandling.Ignore)]
        public ContractEnforcement? ContractEnforcement { get; set; }

        /// <summary>
        /// Срок исполнения контракта/договора
        /// </summary>
        [JsonProperty("contractExecutionDate", NullValueHandling = NullValueHandling.Ignore)]
        public string ContractExecutionDate { get; set; }

        /// <summary>
        /// Номер контракта/договора
        /// </summary>
        [JsonProperty("contractNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string ContractNumber { get; set; }

        /// <summary>
        /// Цена контракта/договора
        /// </summary>
        [JsonProperty("contractPrice", NullValueHandling = NullValueHandling.Ignore)]
        public Price ContractPrice { get; set; }

        /// <summary>
        /// Статус контракта/договора
        /// </summary>
        [JsonProperty("contractStage", NullValueHandling = NullValueHandling.Ignore)]
        public ContractStage? ContractStage { get; set; }

        /// <summary>
        /// Цена контракта/договора. Поле оставлено для сохранения обратной совместимости.
        /// Воспользуйтесь полем contractPrice
        /// </summary>
        [JsonProperty("contractSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? ContractSum { get; set; }

        /// <summary>
        /// Список заказчиков
        /// </summary>
        [JsonProperty("customers", NullValueHandling = NullValueHandling.Ignore)]
        public Customer[] Customers { get; set; }

        /// <summary>
        /// Признак того, что контракт/договор частично исполнен
        /// </summary>
        [JsonProperty("partiallyExecuted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PartiallyExecuted { get; set; }

        /// <summary>
        /// Список участников
        /// </summary>
        [JsonProperty("participants", NullValueHandling = NullValueHandling.Ignore)]
        public Participant[] Participants { get; set; }

        /// <summary>
        /// Дата публикации
        /// </summary>
        [JsonProperty("publicationDate", NullValueHandling = NullValueHandling.Ignore)]
        public string PublicationDate { get; set; }

        /// <summary>
        /// Номер закупки
        /// </summary>
        [JsonProperty("purchaseNumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PurchaseNumber { get; set; }

        /// <summary>
        /// Объекты закупки (не более 50)
        /// </summary>
        [JsonProperty("purchaseObjects", NullValueHandling = NullValueHandling.Ignore)]
        public PurchaseObject[] PurchaseObjects { get; set; }

        /// <summary>
        /// Неформализованные описание статуса закупки
        /// </summary>
        [JsonProperty("purchaseStateDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string PurchaseStateDescription { get; set; }

        /// <summary>
        /// Тип закупки - 94ФЗ, 44ФЗ или 223ФЗ
        /// </summary>
        [JsonProperty("purchaseType", NullValueHandling = NullValueHandling.Ignore)]
        public PurchaseType? PurchaseType { get; set; }

        /// <summary>
        /// Неформализованные описание способа отбора
        /// </summary>
        [JsonProperty("selectionTypeDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectionTypeDescription { get; set; }

        /// <summary>
        /// Начальная цена
        /// </summary>
        [JsonProperty("startPrice", NullValueHandling = NullValueHandling.Ignore)]
        public Price StartPrice { get; set; }

        /// <summary>
        /// Начальная цена. Поле оставлено для сохранения обратной совместимости. Воспользуйтесь
        /// полем startPrice
        /// </summary>
        [JsonProperty("startSum", NullValueHandling = NullValueHandling.Ignore)]
        public double? StartSum { get; set; }

        /// <summary>
        /// Описание предмета закупки
        /// </summary>
        [JsonProperty("topicDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string TopicDescription { get; set; }

        /// <summary>
        /// Цена, предложенная победителем
        /// </summary>
        [JsonProperty("winnerPrice", NullValueHandling = NullValueHandling.Ignore)]
        public Price WinnerPrice { get; set; }
    }

    /// <summary>
    /// Цена контракта/договора
    ///
    /// Начальная цена
    ///
    /// Цена, предложенная победителем
    /// </summary>
    public partial class Price
    {
        /// <summary>
        /// Код валюты. Указан по общероссийскому классификатору валют. Российский рубль - RUB, Евро
        /// - EUR, Доллар США - USD.
        /// </summary>
        [JsonProperty("currencyCode", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("sum", NullValueHandling = NullValueHandling.Ignore)]
        public double? Sum { get; set; }
    }

    public partial class Customer
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


    public partial class PurchaseObject
    {
        /// <summary>
        /// Коды позиции КТРУ
        /// </summary>
        [JsonProperty("ktrus", NullValueHandling = NullValueHandling.Ignore)]
        public ProductCode[] Ktrus { get; set; }

        /// <summary>
        /// Наименование объекта закупки
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        /// <summary>
        /// Коды объекта по справочнику ОКДП
        /// </summary>
        [JsonProperty("okdps", NullValueHandling = NullValueHandling.Ignore)]
        public ProductCode[] Okdps { get; set; }

        /// <summary>
        /// Коды объекта по справочнику ОКПД2
        /// </summary>
        [JsonProperty("okpd2s", NullValueHandling = NullValueHandling.Ignore)]
        public ProductCode[] Okpd2S { get; set; }

        /// <summary>
        /// Коды объекта по справочнику ОКПД
        /// </summary>
        [JsonProperty("okpds", NullValueHandling = NullValueHandling.Ignore)]
        public ProductCode[] Okpds { get; set; }
    }

    public partial class ProductCode
    {
        /// <summary>
        /// Код в соответствии со справочником
        /// </summary>
        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        /// <summary>
        /// Наименование вида деятельности
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
    }
    internal class ContractEnforcementConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContractEnforcement) || t == typeof(ContractEnforcement?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "bankGuarantee":
                    return ContractEnforcement.BankGuarantee;
                case "cashAccount":
                    return ContractEnforcement.CashAccount;
            }
            throw new Exception("Cannot unmarshal type ContractEnforcement");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ContractEnforcement)untypedValue;
            switch (value)
            {
                case ContractEnforcement.BankGuarantee:
                    serializer.Serialize(writer, "bankGuarantee");
                    return;
                case ContractEnforcement.CashAccount:
                    serializer.Serialize(writer, "cashAccount");
                    return;
            }
            throw new Exception("Cannot marshal type ContractEnforcement");
        }

        public static readonly ContractEnforcementConverter Singleton = new ContractEnforcementConverter();
    }

    internal class ContractStageConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(ContractStage) || t == typeof(ContractStage?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "execution":
                    return ContractStage.Execution;
                case "executionTerminated":
                    return ContractStage.ExecutionTerminated;
                case "executionTerminatedForEndExecutionDatePassed":
                    return ContractStage.ExecutionTerminatedForEndExecutionDatePassed;
                case "executionСompleted":
                    return ContractStage.ExecutionСompleted;
                case "invalidated":
                    return ContractStage.Invalidated;
            }
            throw new Exception("Cannot unmarshal type ContractStage");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (ContractStage)untypedValue;
            switch (value)
            {
                case ContractStage.Execution:
                    serializer.Serialize(writer, "execution");
                    return;
                case ContractStage.ExecutionTerminated:
                    serializer.Serialize(writer, "executionTerminated");
                    return;
                case ContractStage.ExecutionTerminatedForEndExecutionDatePassed:
                    serializer.Serialize(writer, "executionTerminatedForEndExecutionDatePassed");
                    return;
                case ContractStage.ExecutionСompleted:
                    serializer.Serialize(writer, "executionСompleted");
                    return;
                case ContractStage.Invalidated:
                    serializer.Serialize(writer, "invalidated");
                    return;
            }
            throw new Exception("Cannot marshal type ContractStage");
        }

        public static readonly ContractStageConverter Singleton = new ContractStageConverter();
    }

    internal class PurchaseTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PurchaseType) || t == typeof(PurchaseType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "Fz223":
                    return PurchaseType.Fz223;
                case "Fz44":
                    return PurchaseType.Fz44;
                case "Fz94":
                    return PurchaseType.Fz94;
            }
            throw new Exception("Cannot unmarshal type PurchaseType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PurchaseType)untypedValue;
            switch (value)
            {
                case PurchaseType.Fz223:
                    serializer.Serialize(writer, "Fz223");
                    return;
                case PurchaseType.Fz44:
                    serializer.Serialize(writer, "Fz44");
                    return;
                case PurchaseType.Fz94:
                    serializer.Serialize(writer, "Fz94");
                    return;
            }
            throw new Exception("Cannot marshal type PurchaseType");
        }

        public static readonly PurchaseTypeConverter Singleton = new PurchaseTypeConverter();
    }
}