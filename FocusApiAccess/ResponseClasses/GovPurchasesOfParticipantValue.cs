namespace FocusApiAccess.ResponseClasses 
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class GovPurchasesOfParticipantValue : IParameterValue
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
        /// Закупки участника
        /// </summary>
        [JsonProperty("purchasesOfParticipant", NullValueHandling = NullValueHandling.Ignore)]
        public PurchasesOfParticipant[] PurchasesOfParticipant { get; set; }
    }

    public partial class PurchasesOfParticipant
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

    public partial class Participant
    {
        /// <summary>
        /// Заключен контракт/договор
        /// </summary>
        [JsonProperty("hasContract", NullValueHandling = NullValueHandling.Ignore)]
        public bool? HasContract { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        [JsonProperty("inn", NullValueHandling = NullValueHandling.Ignore)]
        public string Inn { get; set; }

        /// <summary>
        /// Заявка отклонена
        /// </summary>
        [JsonProperty("isNotAdmitted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsNotAdmitted { get; set; }

        /// <summary>
        /// Признак победителя
        /// </summary>
        [JsonProperty("isWinner", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsWinner { get; set; }

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
    /// Вид обеспечения исполнения контракта: денежная сумма или банковская гарантия.
    /// </summary>
    public enum ContractEnforcement { BankGuarantee, CashAccount };

    /// <summary>
    /// Статус контракта/договора
    /// </summary>
    public enum ContractStage { Execution, ExecutionTerminated, ExecutionTerminatedForEndExecutionDatePassed, ExecutionСompleted, Invalidated };

    /// <summary>
    /// Тип закупки - 94ФЗ, 44ФЗ или 223ФЗ
    /// </summary>
    public enum PurchaseType { Fz223, Fz44, Fz94 };
}
