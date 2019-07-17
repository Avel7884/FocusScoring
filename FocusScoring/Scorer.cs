using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace FocusScoring
{
    public class Scorer
    {
        public Scorer()
        {
            InitMarkers();
        }

        public bool GetMarker(Company company, string markerName)
        {
            return markersDict[markerName].Check(company);
        }

        private int ConutScore(IEnumerable<Marker> markers)
        {
            var redSum = 0;
            var yellowSum = 0;
            var greenSum = 0;
            foreach (var marker in markers)
            {
                if (marker.Colour == MarkerColour.Red || marker.Colour == MarkerColour.RedAffiliates)
                    redSum += marker.Score;
                if (marker.Colour == MarkerColour.Yellow || marker.Colour == MarkerColour.YellowAffiliates)
                    yellowSum += marker.Score;
                if (marker.Colour == MarkerColour.Green || marker.Colour == MarkerColour.GreenAffiliates)
                    greenSum += marker.Score;
            }

            var maxScore = redSum > 0 ? 39 : yellowSum > 0 ? 69 : 100;

            redSum *= 39;
            yellowSum *= 40;
            greenSum *= 21;

            redSum /= markersList
                .Where(marker => marker.Colour == MarkerColour.Red || marker.Colour == MarkerColour.RedAffiliates)
                .Select(x => x.Score).Sum();
            yellowSum /= markersList
                .Where(marker => marker.Colour == MarkerColour.Yellow || marker.Colour == MarkerColour.YellowAffiliates)
                .Select(x => x.Score).Sum();
            greenSum /= markersList
                .Where(marker => marker.Colour == MarkerColour.Green || marker.Colour == MarkerColour.GreenAffiliates)
                .Select(x => x.Score).Sum();

            var score = 79 - redSum - yellowSum + greenSum;
            return Math.Min(maxScore, score);
        }

        private Dictionary<string, Marker> markersDict;

        private static List<Marker> markersList;

        public Marker[] GetAllMarkers => markersList.ToArray();

        public MarkerResult[] CheckMarkers(Company company)
        {
            if (company.Markers != null)
                return company.Markers;
            var results = markersList.Select(marker => marker.Check(company)).Where(x => x).ToArray();
            company.Markers = results;
            return results;
        }

        public int GetScore(Company company)
        {
            var score = ConutScore((company.Markers ?? CheckMarkers(company)).Select(x => x.Marker));
            company.Score = score;
            return score;
        }

        private bool DoubleTryParse(string param, out double result)
        {
            return double.TryParse(param.Replace('.', ','), out result);
        }

        private void InitMarkers()
        {
            //TODO compare from fucn4 to func21 w/ 1C
            //TODO rename markers
            markersList = new List<Marker>
            {
                new Marker("Статус компании связан с произошедшей или планируемой ликвидацией", MarkerColour.Red,
                    "Статус организации принимает значение: недействующее, в стадии ликвидации", 5,
                    company => company.GetParam("Dissolving") == "true" || company.GetParam("Dissolved") == "true"),

                new Marker("Вероятное банкротство организации", MarkerColour.Red,
                    "Обнаружены арбитражные дела о банкротстве за последние 3 месяца \n " +
                    "Обнаружены сообщение о банкротстве за последние 12 месяцев \n " +
                    "Обнаружены признаки завершенной процедуры банкротства", 5,
                    company => company.GetParam("m7013") == "true" || company.GetParam("m7014") == "true" || company.GetParam("m7016") == "true"),

                new Marker("Критическая сумма исполнительных производств", MarkerColour.Red,
                    "Критическая сумма исполнительных производств " +
                    "(сумма исполнительных производств составляет более 20% от выручки организации за последний отчетный период) " +
                    "и более суммы уставного капитала, и более 100 тыс. руб.", 4,
                    company =>
                    {  
                        if (DoubleTryParse(company.GetParam("Sum"), out double sum) && DoubleTryParse(company.GetParam("s1001"), out double a) &&
                            DoubleTryParse(company.GetParam("s6004"), out double b))
                            return a > (0.2 * b) && a > sum & a > 500000;
                        return false;
                    }),

                new Marker("Критический рост суммы арбитражных дел в качестве истца", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве истца за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    company =>
                    {
                        if (DoubleTryParse(company.GetParam("s2003"), out double sumDel) &&
                            DoubleTryParse(company.GetParam("s2004"), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),

                new Marker("Критический рост суммы арбитражных дел в качестве ответчика", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве ответчика за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    company =>
                    {
                        if (DoubleTryParse(company.GetParam("s2001"), out double sumDel) &&
                            DoubleTryParse(company.GetParam("s2002"), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),

                new Marker("Критический сумма арбитражных дел в качестве истца", MarkerColour.Red,
                    "Критическая сумма арбитражных дел в качестве ответчика." +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период и более суммы уставного капитала, " +
                    "и более 500 тыс. руб.", 1,
                    company =>
                    {
                        if (DoubleTryParse(company.GetParam("s2001"), out double sumDel) &&
                            DoubleTryParse(company.GetParam("s6004"), out double revenue) &&
                            DoubleTryParse(company.GetParam("Sum"), out double statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                    }),

                new Marker("Критический сумма арбитражных дел в качестве ответчика", MarkerColour.Red,
                    "Критическая сумма арбитражных дел в качестве истца." +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период" +
                    "и более суммы уставного капитала, и более 500 тыс. руб.", 1,
                    company =>
                    {
                        if (DoubleTryParse(company.GetParam("s2003"), out double sumDel) && DoubleTryParse(company.GetParam("s6004"),
                                                                                     out double revenue)
                                                                                 && DoubleTryParse(company.GetParam("Sum"),
                                                                                     out double statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                    }),

                new Marker("В реестре недобросовестных поставщиков", MarkerColour.Red,
                    "Организация была найдена в реестре недобросовестных поставщиков" +
                    "(ФАС, Федеральное Казначейство)", 3,
                    company => { return company.GetParam("m4001") == "true"; }),

                new Marker("Руководство в реестре дисквалифицированных лиц", MarkerColour.Red,
                    "ФИО руководителей или учредителей были найдены в реестре дисквалифицированных лиц (ФНС)", 4,
                    company => { return company.GetParam("m5008") == "true"; }),

                new Marker("Существенное снижение выручки", MarkerColour.Red, "Выручка снизилась более, чем на 50%", 1,
                    company =>
                    {
                        if (long.TryParse(company.GetParam("s6004").Replace('.', ','), out long s6004) &
                            long.TryParse(company.GetParam("s6003").Replace('.', ','), out long s6003))
                            return s6004 < (0.5 * s6003);
                        return false;
                    }),

                new Marker("Руководитель либо учредитель компании банкрот", MarkerColour.Red,
                    "Наличие сообщений о банкротстве физлица, " +
                    "являющегося руководителем (лицом с правом подписи без доверенности), " +
                    "учредителем, либо индивидуальным предпринимателем. Необходимо изучить сообщения",
                    3,
                    company => { return company.GetParam("m7022") == "true"; }),

                new Marker("Более половины связных организаций в стадии ликвидации", MarkerColour.RedAffiliates,
                    "У более чем 50% связанных организаций статус связан с произошедшей или планируемой ликвидацией", 4,
                    company =>
                    {
                        var Dissolved = company.GetParams("DissolvedAffiliates").Length;
                        var Dissolving = company.GetParams("DissolvingAffiliates").Length;
                        int affiliatesCount = company.GetParams("InnAffilalates").Length;
                        return (Dissolved + Dissolving) / affiliatesCount * 100 > 50;
                    }),

                new Marker("Более половины связных организаций имеют признаки банкротства", MarkerColour.RedAffiliates,
                    "У более чем 50% связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний",
                    5,
                    company =>
                    {
                        //TODO Check for interceptions
                        int affiliatesCount = company.GetParams("InnAffilalates").Length;
                        var m7013 = company.GetParams("m7013Affiliates").Length;
                        var m7014 = company.GetParams("m7014Affiliates").Length;
                        var m7016 = company.GetParams("m7016Affiliates").Length;
                        return (m7013 + m7014 + m7016) / affiliatesCount * 100 > 50;
                    }),

                new Marker("Выручка по группе компаний снизилась более, чем на 50%", MarkerColour.RedAffiliates,
                    "Выручка по группе компаний снизилась более, чем на 50%", 3,
                    company =>
                    {
                        //TODO Check correctness 
                        double s6004;
                        s6004 = company.GetParams("s6004Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => double.Parse(x));
                        double s6003;
                        s6003 = company.GetParams("s6003Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => double.Parse(x));
                        return s6004 < 0.5 * s6003;
                    }),

                new Marker("Критическая сумма исполнительных производств по группе компаний",
                    MarkerColour.RedAffiliates, "Критическая сумма исполнительных производств по группе компаний", 4,
                    company =>
                    {
                        var revs = company.GetMultiParam("s6004Affiliates");
                        var cases = company.GetMultiParam("s1001Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");
                        var count = .0;
                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(sums[i], out double sum) &&
                                DoubleTryParse(cases[i], out double a) &&
                                DoubleTryParse(revs[i], out double b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;

                    }),
                new Marker("Критическая сумма арбитражных дел по группе компаний", MarkerColour.RedAffiliates,
                    "У болле чем 30% связанных организаций сработал маркер критическая сумма арбитражных дел", 1,
                    company =>
                    {
                        var casesIst = company.GetMultiParam("s2003Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");
                        var revs = company.GetMultiParam("s6004Affiliates");
                        var casesOtv = company.GetMultiParam("s2001Affiliates");

                        var count = .0;
                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(revs[i], out double rev) &&
                                DoubleTryParse(sums[i], out double sum) &&
                                ((DoubleTryParse(casesIst[i], out double caseIst) &&
                                  (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                                 (DoubleTryParse(casesOtv[i], out double caseOtv) &&
                                  (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                                count += 1;

                        return count / sums.Length > 0.3;
                    }),

                new Marker("Индивидуальный предприниматель сменил ФИО", MarkerColour.Yellow,
                    "Индивидуальный предприниматель сменил ФИО", 3,
                    company => FIOCache.HasChanged(company.Inn, company.GetParam("FIO"))),

                new Marker("Организация в процессе реорганизации", MarkerColour.Yellow,
                    "Находится в процессе реорганизации в форме присоединения к другому юридическому лицу (слияние, присоединение и т.д.)",
                    3,
                    company => { return company.GetParam("Reorganizing") == "true"; }),
                new Marker("Директор и учредитель одно физическое лицо", MarkerColour.Yellow,
                    "Директор и учредитель одно физическое лицо", 1,
                    company =>
                    {
                        var a = company.GetParams("head");
                        var b = company.GetParams("FounderFL");
                        return a.Any(x => b.Any(y => x == y));
                    }),

                new Marker("Среди учредителей найдены иностранные лица", MarkerColour.Yellow,
                    "Среди учредителей найдены иностранные лица", 1,
                    company => { return company.GetParam("FoundersForeign") == ""; }),
                new Marker("Значительная сумма исполнительных производств", MarkerColour.Yellow,
                    "Значительная сумма исполнительных производств. " +
                    "Т.е. сумма исполнительных производств составляет более 10% от выручки организации за последний отчетный период " +
                    "и более суммы уставного капитала, и более 100 тыс. руб.", 4,
                    company =>
                    {
                        if (DoubleTryParse(company.GetParam("Sum"), out double sum)
                            && DoubleTryParse(company.GetParam("s1001"), out double a)
                            && DoubleTryParse(company.GetParam("s6004"), out double b))
                            return a > 0.1 * b & a > sum & a > 100000;
                        return false;
                    }),

                new Marker("Исполнительные производства (заработная плата)", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых является заработная плата",
                    5,
                    company => { return company.GetParam("m1003") == "true"; }),

                new Marker("Исполнительные производства (наложение ареста)", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых является наложение ареста",
                    5,
                    company => { return company.GetParam("m1004") == "true"; }),

                new Marker("Исполнительные производства (кредитные платежи)", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых является кредитные платежи",
                    5,
                    company => { return company.GetParam("m1005") == "true"; }),

                new Marker("Исполнительные производства (взыскание заложенного имущества", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых является обращение взыскания на заложенное имущество",
                    3,
                    company => { return company.GetParam("m1006") == "true"; }),

                new Marker("Исполнительные производства (налоги и сборы)", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых являются налоги и сборы",
                    4,
                    company => { return company.GetParam("s1007") == "true"; }),

                new Marker("Исполнительные производства (страховые взносы)", MarkerColour.Yellow,
                    "У организации были найдены исполнительные производства, предметом которых являются страховые взносы",
                    4,
                    company => { return company.GetParam("s1008") == "true"; }),

                //new Marker("Значительный рост суммы арбитражных дел в качестве истеца",MarkerColour.Yellow,"Значительный рост суммы арбитражных дел в качестве истца за последние 12 месяцев. " +
                //"Т.е. сумма дел за последние 12 месяцев составляет более 30% по сравнению с суммой дел за предыдущие 2 года. " +
                //"При этом сумма арбитражных дел за последние 12 месяцев более 1 млн. руб.," +
                //"а рост составил 500 тыс. руб.",1,
                //()=>{
                //    //var sumDel = GetParam("s2003");
                //    //var sumDelPast = GetParam("s2004");
                //    //var revenue = GetParam("s6004");
                //    //var sum = GetParam("Sum");
                //    if(DoubleTryParse(GetParam("s2003"),out double sumDel) && DoubleTryParse(GetParam("s2004"),out double sumDelPast) 
                //    && DoubleTryParse(GetParam("s6004"),out double revenue) && DoubleTryParse(GetParam("Sum"), out double sum))
                //    {
                //        if(sumDel >5000000 && )                   ?????
                //    }
                //    return false;
                //}),
                //new Marker("Значительный рост суммы арбитражных дел в качестве ответчика",MarkerColour.Yellow,"Значительный рост суммы арбитражных дел в качестве ответчика за последние 12 месяцев." +
                //"Т.е сумма дел за последние 12 месяцев более чем на 20% больше, чем среднее значение за два предыдущих года",1,
                //()=>{ }),
                //new Marker("Значительная сумма арбитражных дел в качестве истца",MarkerColour.Yellow,"Значительная сумма арбитражных дел в качестве истеца. " +
                //"Т.е сумма дел за последние 12 месяцев составляет более 10% от выручки организации за последний отчетный период " +
                //"и более суммы уставного капитала, и более 500 тыс. руб.",1,
                //()=>{ }),
                //new Marker("Значительная сумма арбитражных дел в качестве ответчика",MarkerColour.Yellow,"Значительная сумма арбитражных дел в качестве ответчика. " +
                //"Т.е сумма дел за последние 12 месяцев составляет более 10% от выручки организации за последний отчетный период " +
                //"и более суммы уставного капитала, и более 500 тыс. руб.",1,
                //()=>{ }),

                new Marker("Отсутствует связь по юр. адресу", MarkerColour.Yellow,
                    "Организация была найдена в списке организаций, связь с которыми по указанному или юридическому адресу отсутствует(ФНС)",
                    3,
                    company => { return company.GetParam("m5002") == "true"; }),

                new Marker("Недостоверные сведение об адресе", MarkerColour.Yellow,
                    "В ЕГРЮЛ указан  признак недостоверности сведений в отношении адреса", 4,
                    company => { return company.GetParam("m5006") == "true"; }),

                new Marker("Недостоверные сведения о руководителе или учредителе", MarkerColour.Yellow,
                    "В ЕГРЮЛ указан признак недостоверности сведений в отношении руководителя или учредителей", 5,
                    company => { return company.GetParam("m5007") == "true"; }),

                new Marker("Задолженность по уплате налогов", MarkerColour.Yellow,
                    "Организация была найдена в списке юридических лиц, " +
                    "имеющих задолженность по уплате налогов более 1000руб, которая направлялась на взыскание судебному приставу-исполнителю (ФНС)",
                    4,
                    company => { return company.GetParam("m5004") == "true"; }),

                new Marker("Не предоставляет отчетность более года", MarkerColour.Yellow,
                    "Организация была найдена в списке юридических лиц, не представляющих налоговую отчетность более года",
                    5,
                    company => { return company.GetParam("m5005") == "true"; }),

                new Marker("Рекомендована дополнительная проверка", MarkerColour.Yellow,
                    "Рекомендована дополнительная проверка руководства и владельцев компании на номинальности", 5,
                    company => { return company.GetParam("m7001") == "true"; }),

                new Marker("Организация зарегистрирована менее 3 мес назад", MarkerColour.Yellow,
                    "Организация зарегистрирована менее 3 месяцев тому назад", 4,
                    company =>
                    {
                        return company.GetParam("m7004") != "true" && company.GetParam("m7003") != "true" &&
                               company.GetParam("m7002") != "true";
                    }),

                new Marker("Организация зарегистрирована менее 6 мес назад", MarkerColour.Yellow,
                    "Организация зарегистрирована менее 6 месяцев тому назад", 3,
                    company => { return company.GetParam("m7004") != "true" && company.GetParam("m7003") == "true"; }),

                new Marker("Организация зарегистрирована менее 12 мес назад", MarkerColour.Yellow,
                    "Организация зарегистрирована менее 12 месяцев тому назад", 2,
                    company => { return company.GetParam("m7004") == "true"; }),
                new Marker("Значительное кол-во учрежденных юр.лиц", MarkerColour.Yellow,
                    "Значительное количество юрлиц, в уставном капитале которых есть доля текущего юрлица (учрежденные юрлица)",
                    1,
                    company => { return int.TryParse(company.GetParam("q7017"), out int count) && count > 10; }),
                new Marker("Значительное кол-во юр. лиц. руководителя (с учетом ИННФЛ)", MarkerColour.Yellow,
                    "Значительное количество не ликвидированных юридических лиц, в которых в качестве действующего руководителя упомянут действующий руководитель текущей организации (с учетом ИННФЛ, если известен)",
                    3,
                    company => { return int.TryParse(company.GetParam("q7018"), out int count) && count > 10; }),
                new Marker("Значительное кол-во юр. лиц. руководителя (с учетом ФИО)", MarkerColour.Yellow,
                    "Значительное количество не ликвидированных юридических лиц, в которых в качестве действующего руководителя упомянут действующий руководитель текущей организации (с учетом только ФИО)",
                    1,
                    company => { return int.TryParse(company.GetParam("q7019"), out int count) && count > 50; }),
                new Marker("Значительное кол-во бывших юр. лиц. руководителя (с учетом ИННФЛ)", MarkerColour.Yellow,
                    "Значительное количество юридических лиц, в которых в качестве бывшего руководителя упомянут действующий руководитель текущей организации (с учетом ИННФЛ, если известен)",
                    1,
                    company =>
                    {
                        return int.TryParse(company.GetParam("q7018"), out int count) &&
                               int.TryParse(company.GetParam("q7020"), out int totalCount) && (totalCount - count) > 20;
                    }),
                new Marker("Значительное кол-во бывших юр. лиц. руководителя (с учетом ФИО)", MarkerColour.Yellow,
                    "Значительное количество юридических лиц, в которых в качестве бывшего руководителя упомянут действующий руководитель текущей организации (с учетом только ФИО)",
                    1,
                    company =>
                    {
                        return int.TryParse(company.GetParam("q7019"), out int count) &&
                               int.TryParse(company.GetParam("q7021"), out int totalCount) && (totalCount - count) > 80;
                    }),
                new Marker("Компания терпит убытки", MarkerColour.Yellow,
                    "Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях)", 3,
                    company => { return int.TryParse(company.GetParam("s6008"), out int value) && value < 0; }),
                new Marker("Наличие арбитражной практики", MarkerColour.Green,
                    "Кол-во арбитражных дел за последние 12 месяцев в качестве ответчика или истца больше нуля", 1,
                    company => { return company.GetParam("q2001") != "" || company.GetParam("q2003") != ""; }),
                new Marker("Наличие бух. форм за предыдущий отчетный период", MarkerColour.Green,
                    "Наличие бухгалтерских форм за предыдущий отчетный период", 3,
                    company => { return company.GetParam("m6002") != ""; }),
                new Marker("Наличие гос. контрактов", MarkerColour.Green,
                    "Наличие государственных контрактов. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в качестве поставщика или заказчика (за 12 последних месяцев) больше нуля",
                    5,
                    company => { return company.GetParam("q4002") != "" || company.GetParam("q4004") != ""; }),
                new Marker("Наличие товарных знаков", MarkerColour.Green,
                    "Наличие товарных знаков, действующих или недействующих, в которых упоминается текущая компания", 5,
                    company => { return company.GetParam("q9001") != ""; }),
                new Marker("Наличие филиалов или представительств", MarkerColour.Green,
                    "Организация имеет филиалы или представительства", 1,
                    //TODO check
                    company => { return company.GetParams("Branches").Count() > 0; }),
                new Marker("Уставный капитал более 100 000 руб.", MarkerColour.Green,
                    "Уставный капитал более 100 000 руб.", 3,
                    company => { return int.TryParse(company.GetParam("Sum"), out int sum) && sum > 100000; }),
                new Marker("Организация зарегистрирована более 5 лет тому назад", MarkerColour.Green,
                    "Организация зарегистрирована более 5 лет тому назад", 4,
                    company => DateTime.TryParse(company.GetParam("regDate"), out var date) &&
                          (DateTime.Today - date).Days > 365 * 5 + 1),
                new Marker("Компания сменила юр. адрес за последние 6 месяцев", MarkerColour.Yellow,
                    "Компания сменила юр. адрес за последние 6 месяцев", 1,
                    //TODO check correctness 
                    company =>
                    {
                        return DateTime.TryParse(company.GetParam("LegalAddress"), out var date) &&
                               (DateTime.Today - date).Days < 365 / 2;
                    }),
                new Marker("Компания сменила юр. адрес дважды за последние 12 месяцев", MarkerColour.Yellow,
                    "Компания сменила юр. адрес дважды за последние 12 месяцев", 2,
                    //TODO check correctness 
                    company =>
                    {
                        int count = 0;
                        var address = company.GetParams("LegalAddress");
                        foreach (var e in address)
                        {
                            if (DateTime.TryParse(e, out var date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count == 2;
                    }),
                new Marker("Компания сменила юр. адрес трижды и более за последние 12 месяцев", MarkerColour.Yellow,
                    "Компания сменила юр. адрес трижды и более за последние 12 месяцев", 5,
                    company =>
                    {
                        //TODO check correctness 
                        int count = 0;
                        var address = company.GetParams("LegalAddress");
                        foreach (var e in address)
                        {
                            if (DateTime.TryParse(e, out var date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                    }),
                new Marker("Компания сменила название за последние 6 месяцев", MarkerColour.Yellow,
                    "Компания сменила название за последние 6 месяцев", 1,
                    company=>
                    {
                        return DateTime.TryParse(company.GetParam("LegalName"), out var date) &&
                               (DateTime.Today - date).Days < 365 / 2;
                    }),
                new Marker("Компания сменила название дважды и более за последние 12 месяцев", MarkerColour.Yellow,
                    "Компания сменила название дважды и более за последние 12 месяцев", 3,
                    company =>
                    {
                        //TODO check correctness 
                        int count = 0;
                        var names = company.GetParams("LegalName");
                        foreach (var e in names)
                        {
                            if (DateTime.TryParse(e, out var date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                    }),
                new Marker("Изменился КПП дважды или более за последние 12 месяцев", MarkerColour.Yellow,
                    "Изменился КПП дважды или более за последние 12 месяцев", 3,
                    company =>
                    {
                        //TODO check correctness 
                        int count = 0;
                        var kpps = company.GetParams("kpp");
                        foreach (var e in kpps)
                        {
                            if (DateTime.TryParse(e, out var date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                    }),
                new Marker("Компания сменила руководителя или управляющую компанию за последние 6 месяцев",
                    MarkerColour.Yellow,
                    "Компания сменила руководителя или управляющую компанию за последние 6 месяцев", 1,
                    company =>
                    {
                        int companiesCount = 0;
                        int headsCount = 0;
                        var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount > 0 || headsCount > 0;
                    }),
                new Marker("Компания сменила руководителя или управляющую компанию дважды за последние 12 месяцев",
                    MarkerColour.Yellow,
                    "Компания сменила руководителя или управляющую компанию дважды за последние 12 месяцев", 3,
                    company =>
                    {
                        int companiesCount = 0;
                        int headsCount = 0;
                        var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount + headsCount == 2;
                    }),
                new Marker(
                    "Компания сменила руководителя или управляющую компанию трижды и более за последние 12 месяцев",
                    MarkerColour.Yellow,
                    "Компания сменила руководителя или управляющую компанию трижды и более за последние 12 месяцев", 5,
                    company =>
                    {
                        int companiesCount = 0;
                        int headsCount = 0;
                        var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out var date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount + headsCount > 2;
                    }),
                new Marker("Компания сменила учредителя за последние 6 месяцев", MarkerColour.Yellow,
                    "Компания сменила учредителя за последние 6 месяцев", 3,
                    company =>
                    {
                        //TODO
                        return false;
                    }),
                new Marker("Значительное количество компаний, найденных в особых реестрах ФНС",
                    MarkerColour.YellowAffiliates, "Значительное количество компаний, найденных в особых реестрах ФНС",
                    4,
                    company =>
                    {
                        //TODO Rename
                        var zp = company.GetMultiParam("m5002Affiliates");
                        var na = company.GetMultiParam("m5004Affiliates");
                        var kp = company.GetMultiParam("m5006Affiliates");
                        var zi = company.GetMultiParam("m5007Affiliates");
                        var count = .0;
                        for (int i = 0; i < zp.Length; i++)
                            if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                                count++;

                        return count / zp.Length > 0.3;
                    }),

                new Marker("Значительное количество компаний, по которым требуется дополнительная проверка",
                    MarkerColour.YellowAffiliates,
                    "Значительное количество компаний, по которым требуется дополнительная проверка", 5,
                    company =>
                    {
                        var zi = company.GetMultiParam("m7001Affiliates");
                        //TODO Rename
                        var count = .0;
                        for (int i = 0; i < zi.Length; i++)
                            if (zi[i] == "true")
                                count++;
                        return count / zi.Length > 0.3;
                    }),

                new Marker("Значительное количество компаний, зарегистрированных менее 12 месяцев назад",
                    MarkerColour.YellowAffiliates,
                    "Значительное количество компаний, зарегистрированных менее 12 месяцев назад", 5,
                    company =>
                    {
                        var zi = company.GetMultiParam("m7001Affiliates");
                        //TODO Rename
                        var count = .0;
                        for (int i = 0; i < zi.Length; i++)
                            if (zi[i] == "true")
                                count++;

                        return count / zi.Length > 0.3;
                    }),

                new Marker(
                    "Значительное количество компаний, у которых за постлю 12 мес. хоья бы раз менялся деректор или учередитель",
                    MarkerColour.YellowAffiliates,
                    "Значительное количество компаний, у которых за постлю 12 мес. хоья бы раз менялся деректор или учередитель",
                    2,
                    company =>
                    {
                        return false; //TODO finish    
                        //TODO Rename
                        var zp = company.GetMultiParam("m5002Affiliates");
                        var na = company.GetMultiParam("m5004Affiliates");
                        var kp = company.GetMultiParam("m5006Affiliates");
                        var zi = company.GetMultiParam("m5007Affiliates");
                        var kv = company.GetMultiParam("m5006Affiliates");
                        var zt = company.GetMultiParam("m5007Affiliates");

                        var count = .0;
                        for (int i = 0; i < zp.Length; i++)
                            if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                                count++;

                        return count / zp.Length > 0.3;
                    }),

                new Marker("У группы компаний замечена активность в арбитражных делах", MarkerColour.GreenAffiliates,
                    "Более 30% связаных компаний имеют арбитражную практику", 1,
                    company =>
                    {
                        var q1 = company.GetMultiParam("q2001Affiliates");
                        var q3 = company.GetMultiParam("q2003Affiliates");

                        var count = .0;
                        for (int i = 0; i < q1.Length; i++)
                            if (q1[i] != "" || q3[i] != "")
                                count++;

                        return count / q1.Length > 0.3;
                    }),

                new Marker("У более чем половины связанных компаний найдена бухгалтерская отчетность",
                    MarkerColour.GreenAffiliates,
                    "Более чем 50% связанных компаний обнаружено наличие бухгалтерской отчетности за предыдущий период",
                    4,
                    company =>
                    {
                        var m2 = company.GetMultiParam("m6002Affiliates");
                        return m2.Count(x => x != "") / (double) m2.Length > 0.5;
                    }),

                new Marker("У группы компаний замечена активность в государственных торгах",
                    MarkerColour.GreenAffiliates,
                    "Более 20% связанных компаний имеют государственные контракты как заказчик или поставщик", 5,
                    company =>
                    {
                        var q1 = company.GetMultiParam("q2001Affiliates");
                        var q3 = company.GetMultiParam("q2003Affiliates");

                        var count = .0;
                        for (int i = 0; i < q1.Length; i++)
                            if (q1[i] != "" || q3[i] != "")
                                count++;

                        return count / q1.Length > 0.2;
                    }),

                new Marker("Значительная часть организаций из группы компаний существуют более 5 лет",
                    MarkerColour.GreenAffiliates,
                    "Более 30% связанных организаций зарегистрированы более 5 лет тому назад", 4,
                    company =>
                    {
                        var dates = company.GetMultiParam("regDateAffiliates");
                        return dates.Count(x =>
                                   DateTime.TryParse(x, out var date) && (DateTime.Today - date).Days > 365 * 5 + 1) /
                               dates.Length > 0.3;
                    })
            };


            markersList.Add(new Marker("Значительно снизилась выручка", MarkerColour.Yellow,
                "Выручка снизилась более чем на 30%", 3,
                company =>
                {
                    if (markersList.First(x => x.Name == "Существенное снижение выручки").Check(company))
                    {
                        if (DoubleTryParse(company.GetParam("s6004"), out double revenu) &&
                            DoubleTryParse(company.GetParam("s6003"), out double revenuPast))
                            return revenu < revenuPast * 0.7;
                    }

                    return false;
                }));

            markersList.Add(new Marker("Значительное число ликвидированных связанных компаний",
                MarkerColour.YellowAffiliates,
                "Значительное число связных компаний, которые были ликвидированы в результате банкротства", 3,
                company =>
                {
                    if (markersList
                        .First(x => x.Name == "Статус компании связан с произошедшей или планируемой ликвидацией").Check(company))
                    {
                        int Affiliatescount = company.GetParams("InnAffilalates").Count();
                        int q7005Count = company.GetParams("q7005Affiliates").Count();
                        return q7005Count > 5 && q7005Count > Affiliatescount * 0.2;
                    }

                    return false;
                }));

            markersList.Add(new Marker("Значительное число юр.лиц по этому адресу", MarkerColour.Yellow,
                "Значительное количество юридических лиц на текущий момент времени", 2,
                company =>
                {
                    if (markersList
                            .Where(x => x.Name == "Статус компании связан с произошедшей или планируемой ликвидацией")
                            .First().Check(company))
                    {
                        if (int.TryParse(company.GetParam("q7006"), out int count1) && count1 > 10
                        ) //КоличествоНеЛиквидированныхСУчетомНомераОфиса
                            return true;
                        if (int.TryParse(company.GetParam("q7007"), out int count2) && count2 > 50
                        ) //КоличествоНеЛиквидированныхБезУчетаНомераОфиса
                            return true;
                    }

                    return false;
                }));

            markersList.Add(
                new Marker("Выручка по группе компаний снизилась более, чем на 30%", MarkerColour.YellowAffiliates,
                    "Выручка по группе компаний снизилась более, чем на 30%", 3,
                    company =>
                    {
                        if (markersList.First(x => x.Name == "Выручка по группе компаний снизилась более, чем на 50%")
                            .Check(company))
                            return false;
                        double s6004;
                        s6004 = company.GetParams("s6004Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => double.Parse(x));
                        double s6003;
                        s6003 = company.GetParams("s6003Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => double.Parse(x));
                        return s6004 < 0.3 * s6003;
                    }));

            markersList.Add(
                new Marker("Значительная сумма исполнительных производств по группе компаний",
                    MarkerColour.YellowAffiliates, "Значительная сумма исполнительных производств по группе компаний",
                    3,
                    company =>
                    {
                        if (markersList.Find(x =>
                            x.Name == "Критическая сумма исполнительных производств по группе компаний").Check(company))
                            return false;


                        var revs = company.GetMultiParam("s6004Affiliates");
                        var cases = company.GetMultiParam("s1001Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");

                        var count = .0;
                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(sums[i], out double sum) &&
                                DoubleTryParse(cases[i], out double a) &&
                                DoubleTryParse(revs[i], out double b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;

                    }));

            markersList.Add(new Marker("Значительная сумма арбитражных дел по группе компаний",
                MarkerColour.YellowAffiliates,
                "У болле чем 30% связанных организаций сработал маркер критическая сумма арбитражных дел", 5,
                company =>
                {
                    if (markersList.Find(x => x.Name == "Критическая сумма арбитражных дел по группе компаний").Check(company))
                        return false;

                    var casesIst = company.GetMultiParam("s2003Affiliates");
                    var sums = company.GetMultiParam("SumAffiliates");
                    var revs = company.GetMultiParam("s6004Affiliates");
                    var casesOtv = company.GetMultiParam("s2001Affiliates");

                    var count = .0;
                    for (int i = 0; i < sums.Length; i++)
                        if (DoubleTryParse(revs[i], out double rev) &&
                            DoubleTryParse(sums[i], out double sum) &&
                            ((DoubleTryParse(casesIst[i], out double caseIst) &&
                              (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                             (DoubleTryParse(casesOtv[i], out double caseOtv) &&
                              (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                            count += 1;

                    return count / sums.Length > 0.3;
                }));

            markersList.Add(new Marker("Значительнрое число компаний с особыми исполниельными производствами",
                MarkerColour.YellowAffiliates,
                "Более чем 30% связаных компаний имеют исполнительные производства предметом которых являются зарплата,наложение ареста, кредитные платежи, обращение взыскания на заложенное иммущество.",
                3,
                company =>
                {
                    var zp = company.GetMultiParam("s1003Affiliates");
                    var na = company.GetMultiParam("s1004Affiliates");
                    var kp = company.GetMultiParam("s1005Affiliates");
                    var zi = company.GetMultiParam("s1006Affiliates");
                    var count = .0;
                    for (int i = 0; i < zp.Length; i++)
                        if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                            count++;
                    return count / zp.Length > 0.3;
                }));



            //markersList.Add(new Marker("Значительное число юр.лиц по этому адресу", MarkerColour.Yellow, "Значительное количество юридических лиц на текущий момент времени", 2,
            //        () => {
            //            if (!markersList.Where(x => x.Name == "Статус компании связан с произошедшей или планируемой ликвидацией").First().Check())
            //                {
            //                double count = GetParam("q7006").
            //                }
            //            return false;
            //        }));
            markersDict = markersList.ToDictionary(x => x.Name);

        }
    }
}