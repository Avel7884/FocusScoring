namespace FocusAccess.ResponseClasses
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class AnalyticsValue : IParameterValue
    {
        /// <summary>
        /// Маркеры автоматической проверки и числовые индикаторы
        /// </summary>
        [JsonProperty("analytics", NullValueHandling = NullValueHandling.Ignore)]
        public Analytics Analytics { get; set; }

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
    }

    /// <summary>
    /// Маркеры автоматической проверки и числовые индикаторы
    /// </summary>
    public partial class Analytics
    {
        /// <summary>
        /// Юр. признаки. Дата, когда в последний раз осуществлялась проверка на наличие ограничений
        /// на операции по банковским счетам организации, установленных ФНС
        /// </summary>
        [JsonProperty("d7010", NullValueHandling = NullValueHandling.Ignore)]
        public string D7010 { get; set; }

        /// <summary>
        /// Юр. признаки. Дата включения лица в реестр субъектов малого и среднего
        /// предпринимательства (ФНС)
        /// </summary>
        [JsonProperty("d7023", NullValueHandling = NullValueHandling.Ignore)]
        public string D7023 { get; set; }

        /// <summary>
        /// Юр. признаки. Текущая стадия банкротства (вычисляется на основе сообщений о банкротстве)
        /// </summary>
        [JsonProperty("e7014", NullValueHandling = NullValueHandling.Ignore)]
        public E7014? E7014 { get; set; }

        /// <summary>
        /// Исп. пр-ва. У организации со схожими реквизитами были найдены исполнительные
        /// производства, предметом которых является заработная плата
        /// </summary>
        [JsonProperty("m1003", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M1003 { get; set; }

        /// <summary>
        /// Исп. пр-ва. У организации со схожими реквизитами были найдены исполнительные
        /// производства, предметом которых является наложение ареста
        /// </summary>
        [JsonProperty("m1004", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M1004 { get; set; }

        /// <summary>
        /// Исп. пр-ва. У организации со схожими реквизитами были найдены исполнительные
        /// производства, предметом которых является кредитные платежи
        /// </summary>
        [JsonProperty("m1005", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M1005 { get; set; }

        /// <summary>
        /// Исп. пр-ва. У организации со схожими реквизитами были найдены исполнительные
        /// производства, предметом которых является обращение взыскания на заложенное имущество
        /// </summary>
        [JsonProperty("m1006", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M1006 { get; set; }

        /// <summary>
        /// Госконтракты. Организация была найдена в реестре недобросовестных поставщиков (ФАС,
        /// Федеральное Казначейство)
        /// </summary>
        [JsonProperty("m4001", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M4001 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. ФИО руководителей или учредителей были найдены в реестре
        /// дисквалифицированных лиц (ФНС). Поле оставлено для сохранения обратной совместимости.
        /// Воспользуйтесь маркером m5008.
        /// </summary>
        [JsonProperty("m5001", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5001 { get; set; }

        /// <summary>
        /// В связи с отказом ФНС от реестра организаций, связь с которыми по указанному адресу
        /// отсутствует, данные о недостоверности адреса берутся из ЕГРЮЛ. Рекомендуем использовать
        /// маркер m5006
        /// </summary>
        [JsonProperty("m5002", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5002 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. Адрес организации был найден в списке 'адресов массовой регистрации '
        /// (ФНС)
        /// </summary>
        [JsonProperty("m5003", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5003 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. Организация была найдена в списке юридических лиц, имеющих
        /// задолженность по уплате налогов более 1000 руб, которая направлялась на взыскание
        /// судебному приставу-исполнителю (ФНС)
        /// </summary>
        [JsonProperty("m5004", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5004 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. Организация была найдена в списке юридических лиц, не представляющих
        /// налоговую отчетность более года (ФНС)
        /// </summary>
        [JsonProperty("m5005", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5005 { get; set; }

        /// <summary>
        /// В ЕГРЮЛ указан признак недостоверности сведений в отношении адреса
        /// </summary>
        [JsonProperty("m5006", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5006 { get; set; }

        /// <summary>
        /// В ЕГРЮЛ указан признак недостоверности сведений в отношении руководителя или учредителей
        /// </summary>
        [JsonProperty("m5007", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5007 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. ФИО руководителей были найдены в реестре дисквалифицированных лиц
        /// (ФНС) или в выписке ЕГРЮЛ
        /// </summary>
        [JsonProperty("m5008", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5008 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. ФИО руководителя было найдено в списке 'массовых' руководителей
        /// </summary>
        [JsonProperty("m5009", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5009 { get; set; }

        /// <summary>
        /// Особые реестры ФНС. ФИО учредителя было найдено в списке 'массовых' учредителей
        /// </summary>
        [JsonProperty("m5010", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M5010 { get; set; }

        /// <summary>
        /// Финансы. Есть бухгалтерская отчетность за последний отчетный год (на момент, когда такая
        /// отчетность становится доступна в Контур.Фокусе)
        /// </summary>
        [JsonProperty("m6002", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M6002 { get; set; }

        /// <summary>
        /// Юр. признаки. Маркер 'Рекомендована дополнительная проверка'
        /// </summary>
        [JsonProperty("m7001", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7001 { get; set; }

        /// <summary>
        /// Юр. признаки. Организация зарегистрирована менее 3 месяцев тому назад
        /// </summary>
        [JsonProperty("m7002", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7002 { get; set; }

        /// <summary>
        /// Юр. признаки. Организация зарегистрирована менее 6 месяцев тому назад
        /// </summary>
        [JsonProperty("m7003", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7003 { get; set; }

        /// <summary>
        /// Юр. признаки. Организация зарегистрирована менее 12 месяцев тому назад
        /// </summary>
        [JsonProperty("m7004", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7004 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие ограничений на операции по банковским счетам организации,
        /// установленных ФНС, по состоянию на d7010
        /// </summary>
        [JsonProperty("m7010", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7010 { get; set; }

        /// <summary>
        /// Юр. признаки. Обнаружены арбитражные дела о банкротстве за последние 3 месяца
        /// </summary>
        [JsonProperty("m7013", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7013 { get; set; }

        /// <summary>
        /// Юр. признаки. Обнаружены сообщения о банкротстве за последние 12 месяцев
        /// </summary>
        [JsonProperty("m7014", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7014 { get; set; }

        /// <summary>
        /// Юр. признаки. Обнаружены сообщения о намерении обратиться в суд с заявлением о
        /// банкротстве за последние 3 месяца
        /// </summary>
        [JsonProperty("m7015", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7015 { get; set; }

        /// <summary>
        /// Юр. признаки. Обнаружены признаки завершенной процедуры банкротства
        /// </summary>
        [JsonProperty("m7016", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7016 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие за последние 12 месяцев сообщений о банкротстве физлица,
        /// являющегося руководителем (лицом с правом подписи без доверенности), учредителем, либо
        /// индивидуальным предпринимателем. Необходимо изучить сообщения
        /// </summary>
        [JsonProperty("m7022", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7022 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие категории микропредприятия в едином реестре субъектов малого и
        /// среднего предпринимательства (ФНС)
        /// </summary>
        [JsonProperty("m7023", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7023 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие категории малого предприятия в едином реестре субъектов малого и
        /// среднего предпринимательства (ФНС)
        /// </summary>
        [JsonProperty("m7024", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7024 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие категории среднего предприятия в едином реестре субъектов малого и
        /// среднего предпринимательства (ФНС)
        /// </summary>
        [JsonProperty("m7025", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7025 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие за последние 12 месяцев сообщений о банкротстве физлица,
        /// являющегося учредителем, либо индивидуальным предпринимателем. Необходимо изучить
        /// сообщения
        /// </summary>
        [JsonProperty("m7026", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7026 { get; set; }

        /// <summary>
        /// Юр. признаки. Применяет упрощенную систему налогообложения — УСН по состоянию на 31
        /// декабря года, предшествующего году размещения таких сведений. Размещение сведений
        /// ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("m7027", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7027 { get; set; }

        /// <summary>
        /// Юр. признаки. Является плательщиком единого налога на вмененный доход — ЕНВД по состоянию
        /// на 31 декабря года, предшествующего году размещения таких сведений. Размещение сведений
        /// ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("m7028", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7028 { get; set; }

        /// <summary>
        /// Юр. признаки. Является плательщиком единого сельскохозяйственного налога — ЕСХН по
        /// состоянию на 31 декабря года, предшествующего году размещения таких сведений. Размещение
        /// сведений ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("m7029", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7029 { get; set; }

        /// <summary>
        /// Юр. признаки. Применяет соглашение о разделе продукции — СРП по состоянию на 31 декабря
        /// года, предшествующего году размещения таких сведений. Размещение сведений ежегодно 1
        /// августа (ФНС)
        /// </summary>
        [JsonProperty("m7030", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7030 { get; set; }

        /// <summary>
        /// Юр. признаки. Организация является участником консолидированной группы налогоплательщиков
        /// по состоянию на 31 декабря года, предшествующего году размещения таких сведений.
        /// Размещение сведений ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("m7031", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7031 { get; set; }

        /// <summary>
        /// Юр. признаки. Организация является ответственным участником консолидированной группы
        /// налогоплательщиков по состоянию на 31 декабря года, предшествующего году размещения таких
        /// сведений. Размещение сведений ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("m7032", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7032 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие организации в перечне стратегических предприятий и стратегических
        /// акционерных обществ, который утвержден Указом Президента Российской Федерации от
        /// 04.08.2004 № 1009
        /// </summary>
        [JsonProperty("m7033", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7033 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие организации в перечне АО по Распоряжению Правительства № 91-Р —
        /// 'Золотая акция' государства
        /// </summary>
        [JsonProperty("m7034", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7034 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие организации в едином реестре членов СРО НОСТРОЙ
        /// </summary>
        [JsonProperty("m7035", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7035 { get; set; }

        /// <summary>
        /// Юр. признаки. Наличие организации в едином реестре членов СРО НОПРИЗ
        /// </summary>
        [JsonProperty("m7036", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7036 { get; set; }

        /// <summary>
        /// Юр. признаки. За последний месяц организация подавала заявления в ЕГРЮЛ, связанные с планируемой ликвидацией организации
        /// </summary>
        [JsonProperty("m7037", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7037 { get; set; }

        /// <summary>
        /// Юр. признаки. За последний месяц организация подавала заявления в ЕГРЮЛ, связанные с изменением руководителя или управляющей компании. По заявлениям еще не принято решение о государственной регистрации, либо принято решение об отказе в регистрации
        /// </summary>
        [JsonProperty("m7038", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7038 { get; set; }

        /// <summary>
        /// Юр. признаки. За последний месяц организация подавала заявления в ЕГРЮЛ, связанные с изменением состава участников (владельцев). По заявлениям еще не принято решение о государственной регистрации, либо принято решение об отказе в регистрации
        /// </summary>
        [JsonProperty("m7039", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7039 { get; set; }

        /// <summary>
        ///Юр. признаки. За последний месяц организация подавала заявления в ЕГРЮЛ, связанные с изменением юридического адреса. По заявлениям еще не принято решение о государственной регистрации, либо принято решение об отказе в регистрации
        /// </summary>
        [JsonProperty("m7040", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7040 { get; set; }

        /// <summary>
        /// Юр. признаки. За последний месяц организация подавала заявления в ЕГРЮЛ, связанные с изменением уставного капитала. По заявлениям еще не принято решение о государственной регистрации, либо принято решение об отказе в регистрации
        /// </summary>
        [JsonProperty("m7041", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7041 { get; set; }

        /// <summary>
        /// Юр. признаки. За последний месяц были поданы заявления в ЕГРИП, связанные с прекращением деятельности лица в качестве ИП
        /// </summary>
        [JsonProperty("m7042", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M7042 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в санкционном списке США
        /// </summary>
        [JsonProperty("m8001", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8001 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в секторальном санкционном списке США
        /// </summary>
        [JsonProperty("m8002", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8002 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в санкционном списке Евросоюза
        /// </summary>
        [JsonProperty("m8003", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8003 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в санкционном списке Великобритании
        /// </summary>
        [JsonProperty("m8004", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8004 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в санкционном списке Украины
        /// </summary>
        [JsonProperty("m8005", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8005 { get; set; }

        /// <summary>
        /// Санкции. Наличие организации в санкционном списке Швейцарии
        /// </summary>
        [JsonProperty("m8006", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8006 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под санкции США, и их суммарная доля владения не
        /// меньше 50%
        /// </summary>
        [JsonProperty("m8007", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8007 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под секторальные санкции США, и их суммарная доля
        /// владения не меньше 50%
        /// </summary>
        [JsonProperty("m8008", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8008 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под санкции Евросоюза, и их суммарная доля
        /// владения не меньше 50%
        /// </summary>
        [JsonProperty("m8009", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8009 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под санкции Великобритании, и их суммарная доля
        /// владения не меньше 50%
        /// </summary>
        [JsonProperty("m8010", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8010 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под санкции Украины, и их суммарная доля владения
        /// не меньше 50%
        /// </summary>
        [JsonProperty("m8011", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8011 { get; set; }

        /// <summary>
        /// Санкции. Совладельцы компании попадают под санкции Швейцарии, и их суммарная доля
        /// владения не меньше 50%
        /// </summary>
        [JsonProperty("m8012", NullValueHandling = NullValueHandling.Ignore)]
        public bool? M8012 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Количество найденных исполнительных производств в отношении организаций со
        /// схожими реквизитами (за 12 последних месяцев)
        /// </summary>
        [JsonProperty("q1001", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q1001 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Количество найденных исполнительных производств в отношении организаций со
        /// схожими реквизитами (всего)
        /// </summary>
        [JsonProperty("q1002", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q1002 { get; set; }

        /// <summary>
        /// Залоги. Число уведомлений о залогах движимого имущества (залогодатель)
        /// </summary>
        [JsonProperty("q1101", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q1101 { get; set; }

        /// <summary>
        /// Залоги. Число уведомлений о залогах движимого имущества (залогодержатель)
        /// </summary>
        [JsonProperty("q1102", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q1102 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел за 12 последних месяцев
        /// </summary>
        [JsonProperty("q2001", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2001 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел за 3 года
        /// </summary>
        [JsonProperty("q2002", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2002 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества дел за 12 последних месяцев
        /// </summary>
        [JsonProperty("q2003", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2003 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества дел за 3 года
        /// </summary>
        [JsonProperty("q2004", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2004 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества проигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("q2011", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2011 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества частично проигранных дел (за 3
        /// года)
        /// </summary>
        [JsonProperty("q2012", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2012 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества не проигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("q2013", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2013 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел в процессе рассмотрения (за 3
        /// года)
        /// </summary>
        [JsonProperty("q2014", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2014 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, исход которых определить не
        /// удалось (за 3 года)
        /// </summary>
        [JsonProperty("q2015", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2015 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества проигранных дел (за 12 месяцев)
        /// </summary>
        [JsonProperty("q2016", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2016 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества частично проигранных дел (за 12
        /// месяцев)
        /// </summary>
        [JsonProperty("q2017", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2017 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества не проигранных дел (за 12 месяцев)
        /// </summary>
        [JsonProperty("q2018", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2018 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел в процессе рассмотрения (за 12
        /// месяцев)
        /// </summary>
        [JsonProperty("q2019", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2019 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества не выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("q2021", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2021 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества частично выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("q2022", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2022 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("q2023", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2023 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества дел в процессе рассмотрения (за 3 года)
        /// </summary>
        [JsonProperty("q2024", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2024 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка количества дел, исход которых определить не
        /// удалось (за 3 года)
        /// </summary>
        [JsonProperty("q2025", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2025 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, которые связаны с проведением
        /// процедуры банкротства
        /// </summary>
        [JsonProperty("q2031", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2031 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, которые связаны с
        /// обязательствами по договорам займа, кредита, лизинга
        /// </summary>
        [JsonProperty("q2032", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2032 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, которые связаны с налогами:
        /// иски налоговых органов, взыскание налогов, оспаривание решений налоговых органов
        /// </summary>
        [JsonProperty("q2033", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2033 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, которые связаны с
        /// обязательствами по договорам на оказание услуг
        /// </summary>
        [JsonProperty("q2034", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2034 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка количества дел, которые связаны с
        /// обязательствами по договорам поставки
        /// </summary>
        [JsonProperty("q2035", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q2035 { get; set; }

        /// <summary>
        /// Госконтракты. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве поставщика (за 12 последних месяцев)
        /// </summary>
        [JsonProperty("q4002", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q4002 { get; set; }

        /// <summary>
        /// Госконтракты. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве поставщика
        /// </summary>
        [JsonProperty("q4003", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q4003 { get; set; }

        /// <summary>
        /// Госконтракты. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве заказчика (за 12 последних месяцев)
        /// </summary>
        [JsonProperty("q4004", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q4004 { get; set; }

        /// <summary>
        /// Госконтракты. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве заказчика
        /// </summary>
        [JsonProperty("q4005", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q4005 { get; set; }

        /// <summary>
        /// Финансы. Год, за который доступна последняя бухгалтерская отчетность по организации
        /// </summary>
        [JsonProperty("q6001", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q6001 { get; set; }

        /// <summary>
        /// Юр. признаки. Оценка числа связанных компаний, которые были ликвидированы в результате
        /// банкротства
        /// </summary>
        [JsonProperty("q7005", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7005 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество не ликвидированных юридических лиц, зарегистированных по тому же
        /// адресу (с учетом номера офиса) на текущий момент времени
        /// </summary>
        [JsonProperty("q7006", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7006 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество не ликвидированных юридических лиц, зарегистированных по тому же
        /// адресу (без учета номера офиса) на текущий момент времени
        /// </summary>
        [JsonProperty("q7007", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7007 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество юридических лиц, когда-либо зарегистированных по тому же адресу
        /// (с учетом номера офиса)
        /// </summary>
        [JsonProperty("q7008", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7008 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество юридических лиц, когда-либо зарегистированных по тому же адресу
        /// (без учета номера офиса)
        /// </summary>
        [JsonProperty("q7009", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7009 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество юрлиц, в уставном капитале которых есть доля текущего юрлица
        /// (учрежденные юрлица)
        /// </summary>
        [JsonProperty("q7017", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7017 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество не ликвидированных юридических лиц, в которых в качестве
        /// действующего руководителя упомянут действующий руководитель текущей организации (с учетом
        /// ИННФЛ, если известен)
        /// </summary>
        [JsonProperty("q7018", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7018 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество не ликвидированных юридических лиц, в которых в качестве
        /// действующего руководителя упомянут действующий руководитель текущей организации (с учетом
        /// только ФИО)
        /// </summary>
        [JsonProperty("q7019", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7019 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество юридических лиц, в которых в качестве действующего или бывшего
        /// руководителя упомянут действующий руководитель текущей организации (с учетом ИННФЛ, если
        /// известен)
        /// </summary>
        [JsonProperty("q7020", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7020 { get; set; }

        /// <summary>
        /// Юр. признаки. Количество юридических лиц, в которых в качестве действующего или бывшего
        /// руководителя упомянут действующий руководитель текущей организации (с учетом только ФИО)
        /// </summary>
        [JsonProperty("q7021", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7021 { get; set; }

        /// <summary>
        /// Юр. признаки. Среднесписочная численность работников за календарный год, предшествующий
        /// году размещения таких сведений. Размещение сведений ежегодно 1 августа (ФНС)
        /// </summary>
        [JsonProperty("q7022", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7022 { get; set; }

        /// <summary>
        /// Юр. признаки. Число арбитражных дел о банкротстве в качестве ответчика (наличие дела о
        /// банкротстве может не свидетельствовать о начале процедуры банкротства)
        /// </summary>
        [JsonProperty("q7026", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q7026 { get; set; }

        /// <summary>
        /// Число потенциальных сайтов компании
        /// </summary>
        [JsonProperty("q8001", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q8001 { get; set; }

        /// <summary>
        /// Количество товарных знаков, действующих или недействующих, в которых упоминается текущая
        /// компания
        /// </summary>
        [JsonProperty("q9001", NullValueHandling = NullValueHandling.Ignore)]
        public long? Q9001 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Сумма (в рублях) найденных исполнительных производств в отношении организаций
        /// со схожими реквизитами (за 12 последних месяцев)
        /// </summary>
        [JsonProperty("s1001", NullValueHandling = NullValueHandling.Ignore)]
        public double? S1001 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Сумма (в рублях) найденных исполнительных производств в отношении организаций
        /// со схожими реквизитами (всего)
        /// </summary>
        [JsonProperty("s1002", NullValueHandling = NullValueHandling.Ignore)]
        public double? S1002 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Сумма (в рублях) найденных исполнительных производств, предметом которых
        /// являются налоги и сборы, в отношении организаций со схожими реквизитами (всего)
        /// </summary>
        [JsonProperty("s1007", NullValueHandling = NullValueHandling.Ignore)]
        public double? S1007 { get; set; }

        /// <summary>
        /// Исп. пр-ва. Сумма (в рублях) найденных исполнительных производств, предметом которых
        /// являются страховые взносы, в отношении организаций со схожими реквизитами (всего)
        /// </summary>
        [JsonProperty("s1008", NullValueHandling = NullValueHandling.Ignore)]
        public double? S1008 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы за 12 последних месяцев (в
        /// рублях)
        /// </summary>
        [JsonProperty("s2001", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2001 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы за 3 последних года (в рублях)
        /// </summary>
        [JsonProperty("s2002", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2002 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы за 12 последних месяцев (в рублях)
        /// </summary>
        [JsonProperty("s2003", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2003 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы за 3 последних года (в рублях)
        /// </summary>
        [JsonProperty("s2004", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2004 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы проигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("s2011", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2011 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы частично проигранных дел (за 3
        /// года)
        /// </summary>
        [JsonProperty("s2012", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2012 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы не проигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("s2013", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2013 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы по делам в процессе
        /// рассмотрения (за 3 года)
        /// </summary>
        [JsonProperty("s2014", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2014 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы по делам, исход которых
        /// определить не удалось (за 3 года)
        /// </summary>
        [JsonProperty("s2015", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2015 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы проигранных дел (за 12 месяцев)
        /// </summary>
        [JsonProperty("s2016", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2016 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы частично проигранных дел (за 12
        /// месяцев)
        /// </summary>
        [JsonProperty("s2017", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2017 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы не проигранных дел (за 12
        /// месяцев)
        /// </summary>
        [JsonProperty("s2018", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2018 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы дел в процессе рассмотрения (за
        /// 12 месяцев)
        /// </summary>
        [JsonProperty("s2019", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2019 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы не выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("s2021", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2021 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы частично выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("s2022", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2022 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы выигранных дел (за 3 года)
        /// </summary>
        [JsonProperty("s2023", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2023 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы по делам в процессе рассмотрения
        /// (за 3 года)
        /// </summary>
        [JsonProperty("s2024", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2024 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве истца. Оценка исковой суммы по делам, исход которых определить
        /// не удалось (за 3 года)
        /// </summary>
        [JsonProperty("s2025", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2025 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы у дел, которые связаны с
        /// проведением процедуры банкротства
        /// </summary>
        [JsonProperty("s2031", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2031 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы у дел, которые связаны с
        /// обязательствами по договорам займа, кредита, лизинга
        /// </summary>
        [JsonProperty("s2032", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2032 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы у дел, которые связаны с
        /// налогами: иски налоговых органов, взыскание налогов, оспаривание решений налоговых органов
        /// </summary>
        [JsonProperty("s2033", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2033 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы у дел, которые связаны с
        /// обязательствами по договорам на оказание услуг
        /// </summary>
        [JsonProperty("s2034", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2034 { get; set; }

        /// <summary>
        /// Арбитраж. Дела в качестве ответчика. Оценка исковой суммы у дел, которые связаны с
        /// обязательствами по договорам поставки
        /// </summary>
        [JsonProperty("s2035", NullValueHandling = NullValueHandling.Ignore)]
        public double? S2035 { get; set; }

        /// <summary>
        /// Госконтракты. Сумма по госконтрактам (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве поставщика (за 12 последних месяцев, с оценкой в рублях)
        /// </summary>
        [JsonProperty("s4002", NullValueHandling = NullValueHandling.Ignore)]
        public double? S4002 { get; set; }

        /// <summary>
        /// Госконтракты. Сумма по госконтрактам (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве поставщика (с оценкой в рублях)
        /// </summary>
        [JsonProperty("s4003", NullValueHandling = NullValueHandling.Ignore)]
        public double? S4003 { get; set; }

        /// <summary>
        /// Госконтракты. Сумма по госконтрактам (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве заказчика (за 12 последних месяцев, с оценкой в рублях)
        /// </summary>
        [JsonProperty("s4004", NullValueHandling = NullValueHandling.Ignore)]
        public double? S4004 { get; set; }

        /// <summary>
        /// Госконтракты. Сумма по госконтрактам (44ФЗ и 223ФЗ), в которых организация участвует в
        /// качестве заказчика (с оценкой в рублях)
        /// </summary>
        [JsonProperty("s4005", NullValueHandling = NullValueHandling.Ignore)]
        public double? S4005 { get; set; }

        /// <summary>
        /// Финансы. Выручка на начало отчетного периода (за последний отчетный год, оценка в рублях)
        /// </summary>
        [JsonProperty("s6003", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6003 { get; set; }

        /// <summary>
        /// Финансы. Выручка на конец отчетного периода (за последний отчетный год, оценка в рублях)
        /// </summary>
        [JsonProperty("s6004", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6004 { get; set; }

        /// <summary>
        /// Финансы. Баланс на начало отчетного периода (за последний отчетный год, оценка в рублях)
        /// </summary>
        [JsonProperty("s6005", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6005 { get; set; }

        /// <summary>
        /// Финансы. Баланс на конец отчетного периода (за последний отчетный год, оценка в рублях)
        /// </summary>
        [JsonProperty("s6006", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6006 { get; set; }

        /// <summary>
        /// Финансы. Чистая прибыль на начало отчетного периода (за последний отчетный год, оценка в
        /// рублях)
        /// </summary>
        [JsonProperty("s6007", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6007 { get; set; }

        /// <summary>
        /// Финансы. Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в
        /// рублях)
        /// </summary>
        [JsonProperty("s6008", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6008 { get; set; }

        /// <summary>
        /// Финансы. Общая сумма уплаченных налогов и сборов в году, предшествующем году размещения
        /// таких сведений. Размещение сведений ежегодно 1 октября (ФНС)
        /// </summary>
        [JsonProperty("s6009", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6009 { get; set; }

        /// <summary>
        /// Финансы. Сумма доходов по данным бухгалтерской отчетности за год, предшествующий году
        /// размещения таких сведений. Размещение сведений ежегодно 1 октября (ФНС)
        /// </summary>
        [JsonProperty("s6010", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6010 { get; set; }

        /// <summary>
        /// Финансы. Сумма расходов по данным бухгалтерской отчетности за год, предшествующий году
        /// размещения таких сведений. Размещение сведений ежегодно 1 октября (ФНС)
        /// </summary>
        [JsonProperty("s6011", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6011 { get; set; }

        /// <summary>
        /// Финансы. Общая сумма задолженности по налогам на 31 декабря года, предшествующего году
        /// размещения таких сведений. При условии, что она не уплачена до 1 октября года размещения
        /// таких сведений. Размещение сведений ежегодно 1 декабря (ФНС)
        /// </summary>
        [JsonProperty("s6012", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6012 { get; set; }

        /// <summary>
        /// Финансы. Общая сумма штрафов за налоговые правонарушения, назначенные в году,
        /// предшествующем году размещения таких сведений. При условии, что они не уплачены до 1
        /// октября года размещения таких сведений. Размещение сведений ежегодно 1 декабря (ФНС)
        /// </summary>
        [JsonProperty("s6013", NullValueHandling = NullValueHandling.Ignore)]
        public double? S6013 { get; set; }
    }

    /// <summary>
    /// Юр. признаки. Текущая стадия банкротства (вычисляется на основе сообщений о банкротстве)
    /// </summary>
    public enum E7014 { ВнешнееУправление, КонкурсноеПроизводство, КонкурсноеПроизводствоЗавершено, Наблюдение, НеУдалосьОпределитьСтадию, ОтказаноВПризнанииДолжникаБанкротом, ПроизводствоПоДелуПрекращено, РеализацияИмущества, РеализацияИмуществаЗавершена, РеструктуризацияДолгов, РеструктуризацияДолговЗавершена, ФинансовоеОздоровление };

    public static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                OrganizationTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                E7014Converter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                IpStatusConverter.Singleton,
                UlStatusConverter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                StageConverter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                TypeEnumConverter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                ContractEnforcementConverter.Singleton,
                ContractStageConverter.Singleton,
                PurchaseTypeConverter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                ContractEnforcementConverter.Singleton,
                ContractStageConverter.Singleton,
                PurchaseTypeConverter.Singleton,
                //new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
                LimitTypeConverter.Singleton,
                EventConverter.Singleton,
                AssuranceTypeConverter.Singleton,
                RoleConverter.Singleton,
                SuggestEnumConverter.Singleton,
                TypeEnumConverter.Singleton
            }
        };
    }

    internal class E7014Converter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(E7014) || t == typeof(E7014?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            return value switch
            {
                "Внешнее управление" => E7014.ВнешнееУправление,
                "Конкурсное производство" => E7014.КонкурсноеПроизводство,
                "Конкурсное производство завершено" => E7014.КонкурсноеПроизводствоЗавершено,
                "Наблюдение" => E7014.Наблюдение,
                "Не удалось определить стадию" => E7014.НеУдалосьОпределитьСтадию,
                "Отказано в признании должника банкротом" => E7014.ОтказаноВПризнанииДолжникаБанкротом,
                "Производство по делу прекращено" => E7014.ПроизводствоПоДелуПрекращено,
                "Реализация имущества" => E7014.РеализацияИмущества,
                "Реализация имущества завершена" => E7014.РеализацияИмуществаЗавершена,
                "Реструктуризация долгов" => E7014.РеструктуризацияДолгов,
                "Реструктуризация долгов завершена" => E7014.РеструктуризацияДолговЗавершена,
                "Финансовое оздоровление" => E7014.ФинансовоеОздоровление,
                _ => throw new Exception("Cannot unmarshal type E7014")
            };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (E7014)untypedValue;
            switch (value)
            {
                case E7014.ВнешнееУправление:
                    serializer.Serialize(writer, "Внешнее управление");
                    return;
                case E7014.КонкурсноеПроизводство:
                    serializer.Serialize(writer, "Конкурсное производство");
                    return;
                case E7014.КонкурсноеПроизводствоЗавершено:
                    serializer.Serialize(writer, "Конкурсное производство завершено");
                    return;
                case E7014.Наблюдение:
                    serializer.Serialize(writer, "Наблюдение");
                    return;
                case E7014.НеУдалосьОпределитьСтадию:
                    serializer.Serialize(writer, "Не удалось определить стадию");
                    return;
                case E7014.ОтказаноВПризнанииДолжникаБанкротом:
                    serializer.Serialize(writer, "Отказано в признании должника банкротом");
                    return;
                case E7014.ПроизводствоПоДелуПрекращено:
                    serializer.Serialize(writer, "Производство по делу прекращено");
                    return;
                case E7014.РеализацияИмущества:
                    serializer.Serialize(writer, "Реализация имущества");
                    return;
                case E7014.РеализацияИмуществаЗавершена:
                    serializer.Serialize(writer, "Реализация имущества завершена");
                    return;
                case E7014.РеструктуризацияДолгов:
                    serializer.Serialize(writer, "Реструктуризация долгов");
                    return;
                case E7014.РеструктуризацияДолговЗавершена:
                    serializer.Serialize(writer, "Реструктуризация долгов завершена");
                    return;
                case E7014.ФинансовоеОздоровление:
                    serializer.Serialize(writer, "Финансовое оздоровление");
                    return;
            }
            throw new Exception("Cannot marshal type E7014");
        }

        public static readonly E7014Converter Singleton = new E7014Converter();
    }
}
