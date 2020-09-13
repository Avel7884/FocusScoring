using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using FocusAccess;
using FocusAccess.ResponseClasses;
using FocusScoring;

namespace FocusMarkers
{
    public abstract class CodeChecksProvider : IChecksProvider<InnUrlArg>
    {
        public string MarkerArgName => "LibraryCheckMethodName";
        /*

        public Func<object[], CheckResult> Provide1(Marker<INN> Marker)
        {
            MethodInfo methodInfo; 
            if (Marker.CheckArguments.TryGetValue(MarkerArgName, out var checkArg) &&
                (methodInfo = GetType().GetMethod(checkArg)) != null) 
                return Expression.Lambda<Func<object[], CheckResult>>(
                    Expression.IfThenElse(Expression.Call(IsParameterMatch),  Expression.Call(methodInfo))).Compile();
            Expression.Call(z=>z,);
            Expression.Lambda<Action<int>>()
            throw new KeyNotFoundException("Things went wrong."); //TODO make exception
        }*/

        private static bool IsParameterMatch(IParameterValue[] objects, MethodInfo info) => 
            objects.Length == info.GetParameters().Length &&
            objects.All(x => x != null) &&
            info.GetParameters().Zip(objects,(t,o)=>o.GetType() == t.ParameterType).All(x=>x);

        public Func<IParameterValue[], CheckResult> Provide(Marker<InnUrlArg> Marker)
        {
            MethodInfo methodInfo;
            if (!Marker.CheckArguments.TryGetValue(MarkerArgName, out var checkArg) ||
                (methodInfo = GetType().GetMethod(checkArg)) == null)
                throw new KeyNotFoundException("Things went wrong.");
            var paramArray = methodInfo.GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray();
            var expr = Expression.Lambda(Expression.Call(methodInfo,paramArray),paramArray).Compile();
            return p =>
            {
                if (!IsParameterMatch(p, methodInfo))
                    throw new Exception(); //TODO make exception
                return (CheckResult)expr.DynamicInvoke(p);
            };
        }

        /*public Func<object[], CheckResult> Provide(Marker<INN> Marker)
        {
            if (Marker.CheckArguments.TryGetValue(MarkerArgName, out var checkArg) 
                && dict.TryGetValue(checkArg,out var value))
                return value;
            throw new KeyNotFoundException("Things went wrong."); 
        }*/
    }

    class FocusChecksProvider : CodeChecksProvider
    {
        public static CheckResult Marker0(ReqValue req, AnalyticsValue analytics)=>new CheckResult {Result = true};
        
        public static CheckResult Marker1(ReqValue req,AnalyticsValue analytics)
        {
            var pureProfit = analytics.Analytics.S6008;
            return new CheckResult
            {
                Result = pureProfit.HasValue && pureProfit<0 && DateTime.Today - req.RegistrationDate > TimeSpan.FromDays(365),
                Verbose = "Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях): " 
                          + (pureProfit.HasValue ? pureProfit.Value.ToString(CultureInfo.CurrentCulture) : "")
            };
        }

        public static CheckResult Marker2(ReqValue req) =>
            CheckResult.Failed();

        public static CheckResult Marker3(AnalyticsValue analytics)
        {
            if(analytics.Analytics.M7014.HasValue && analytics.Analytics.M7014.Value)
                return new CheckResult
                {
                    Result = true,
                    Verbose = "Текущая стадия банкротства: " + analytics.Analytics.E7014
                };
            return CheckResult.Failed();
        }

        public static CheckResult Marker4(ReqValue req)
        {
            if ((req.Ip?.Status?.PurpleStatus?.Dissolved ?? false) ||
                (req.Ul?.Status?.FluffyStatus?.Dissolved ?? false))
                return CheckResult.Checked("Недействующее");
                    
            if (req.Ul?.Status?.FluffyStatus?.Dissolving ?? false)
                return CheckResult.Checked("В процессе ликвидации (либо планируется исключение из ЕГРЮЛ)");
                    
            return CheckResult.Failed();
        }

        public static CheckResult Marker5(AnalyticsValue analytics) =>
            new CheckResult
            {
                Result = analytics.Analytics?.M7015 ?? false,
                Verbose = "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца"
            };
        
        public static CheckResult Marker6(AnalyticsValue analytics)
        {
            var analyt = analytics.Analytics;
            if ((analyt?.M7014 ?? false) && analyt.E7014 != null && analyt.E7014 != E7014.ПроизводствоПоДелуПрекращено && analyt.E7014 != E7014.ОтказаноВПризнанииДолжникаБанкротом)
                return CheckResult.Checked("Число арбитражных дел о банкротстве в качестве ответчика: " + analyt.Q7026);
            return CheckResult.Failed();
        }

        public static CheckResult Marker7(AnalyticsValue analytics) =>
            new CheckResult {Result = analytics.Analytics?.M1003 ?? false};

        public static CheckResult Marker8(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M4001 ?? false,
                Verbose = "Организация была найдена в реестре недобросовестных поставщиков (ФАС, Федеральное Казначейство)"
            };
        
        public static CheckResult Marker9(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M5008 ?? false,
                Verbose = "ФИО руководителей были найдены в реестре дисквалифицированных лиц (ФНС)"
            };

        public static CheckResult Marker10(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M7022 ?? false };
        
        public static CheckResult Marker11(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M5008 ?? false,
                Verbose = "ФИО руководителей были найдены в реестре дисквалифицированных лиц (ФНС)"
            };
        
        public static CheckResult Marker12(AnalyticsValue analytics)=>
        new CheckResult
        {
            Result = analytics.Analytics?.M7010 ?? false,
            Verbose = "По состоянию на указанную дату действовало ограничение на операции по банковским счетам организации, установленное ИФНС - %1."
        };
        
        public static CheckResult Marker13(ReqValue req, AnalyticsValue analytics)
        {
            var status = req.Status();
            if ((analytics.Analytics.M7016 ?? false) && status != null)
                return CheckResult.Checked("Статус организации: " + status);
            return CheckResult.Failed();
        }
        
        public static CheckResult Marker14(ReqValue req, AnalyticsValue analytics)=>
            new CheckResult {Result = (analytics.Analytics.M7042 ?? false) || (analytics.Analytics.M7037 ?? false)};
        
        public static CheckResult Marker15(AnalyticsValue analytics)
        {
            var count = analytics.Analytics?.Q7018 ?? 0;
            return count > 20 ? 
                CheckResult.Checked("Количество не ликвидированных организаций: " + count) : 
                CheckResult.Failed();
        }
        
        public static CheckResult Marker16(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M5006 ?? false};
        
        public static CheckResult Marker17(ReqValue req, AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M5007 ?? false};
        
        public static CheckResult Marker18(AnalyticsValue analytics)
        {
            var massManager = analytics.Analytics?.M5009 ?? false;
            var massFounder = analytics.Analytics?.M5010 ?? false;
            return massFounder || massManager
                ? CheckResult.Checked(
                    (massManager ? "ФИО руководителя найдено в списке \"массовых\" руководителей." : "") +
                    (massManager && massFounder ? Environment.NewLine : "") +
                    (massFounder ? "ФИО учредителя найдено в списке \"массовых\" учредителей." : "")) :
                CheckResult.Failed();
        }
        
        public static CheckResult Marker19(AnalyticsValue[] affAnalytics)
        {
            if (affAnalytics.Length <= 1)
                return CheckResult.Failed();
            var totalBankruptcies = affAnalytics.Count(x => x.Analytics.M7014 ?? false);
            return totalBankruptcies*2 >= affAnalytics.Length ?
                CheckResult.Checked($"У {totalBankruptcies} из {affAnalytics} связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний") :
                CheckResult.Failed();
        }
        
        public static CheckResult Marker20(ReqValue[] affReq)
        {
            /*
            if (affReq.Length <= 0)
                return CheckResult.Failed();
            var totalBankruptcies = affReq.Count(x => x.Analytics.M7014 ?? false);
            return totalBankruptcies*2 >= affReq.Length ?
                CheckResult.Checked($"У {totalBankruptcies} из {affReq} связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний") :*/
                return CheckResult.Failed();
        }
        
        public static CheckResult Marker21(ReqValue req, ReqValue[] affReq)
        {
            if (req.Inn.Length != 12 || affReq.Length <= 0)
                return CheckResult.Failed();
            var sideManager = affReq
                .Where(r => !r?.Ul?.Status?.FluffyStatus?.Dissolved ?? false)
                .Where(r => r.Ul.Heads.Any(h => h.Innfl == req.Inn))
                .Select(r=>r.Ul.LegalName.Short)
                .ToArray();
            return sideManager.Any() ?
                CheckResult.Checked("Является руководителем следующих компаний: " + string.Join("    ", sideManager)) :
                CheckResult.Failed();
        }

        
        public static CheckResult Marker22(ReqValue req, ReqValue[] affReq, EgrDetailsValue[] affEgr)
        {
            if (req.Inn.Length != 12 || affReq.Length <= 0)
                return CheckResult.Failed();
            var dissolvedAff = affReq.Where(r => !r?.Ul?.Status?.FluffyStatus?.Dissolved ?? false).ToArray();
            var prevFounder = affEgr.Where(e => e.Ul != null && e.Ul.FoundersFl.Any(f => f.Innfl == req.Inn)).Select(r=>r.Inn).ToArray();
            var sideFounder = dissolvedAff.Where(r => prevFounder.Contains(r.Inn)).Select(r=>r.Inn).ToArray();
            return sideFounder.Any() ?
                CheckResult.Checked("Является учредителем следующих компаний: " + string.Join("    ", sideFounder)) :
                CheckResult.Failed();
        }

        public static CheckResult Marker23(ReqValue[] affReq)
        {
            if (affReq.Length <= 1)
                return CheckResult.Failed();
            var totalLiquidated = affReq.Count(r => (r.Ip.Status?.PurpleStatus?.Dissolved ?? false) ||
                                                    (r.Ul.Status?.FluffyStatus?.Dissolved ?? false) ||
                                                    (r.Ul.Status?.FluffyStatus?.Dissolving ?? false));
            return totalLiquidated*2 >= affReq.Length ?
                CheckResult.Checked($"У {totalLiquidated} из {affReq.Length} связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний") :
                CheckResult.Failed();
        }
        
        public static CheckResult Marker24(ReqValue req, ReqValue[] affReq)
        {
            if (req.Inn.Length != 12  || affReq.Length <= 1)
                return CheckResult.Failed();
            var sideIps = affReq
                .Where(r => r.Inn == req.Inn && (!r.Ip?.Status?.PurpleStatus?.Dissolved ?? false))
                .Select(r=>r.Ip.Fio)
                .ToArray();
            return sideIps.Any() ?
                CheckResult.Checked("По данному ИННФЛ зарегестрированны следующие ИП: " + string.Join("    ", sideIps)):
                CheckResult.Failed();
        }
        
        public static CheckResult Marker25(AnalyticsValue analytics, AnalyticsValue[] affAnalytics)
        {
            if (affAnalytics.Length <= 1)
                return CheckResult.Failed();
            var liquidated = analytics.Analytics.Q7005 ?? 0;
            return liquidated * 5 > affAnalytics.Length
                ? CheckResult.Checked("Количество ликвидированных/ликвидируемых организаций: " + liquidated)
                : CheckResult.Failed();
        }

        public static CheckResult Marker26(AnalyticsValue[] affAnalytics)
        {
            var falseInfo = affAnalytics.Count(a => a.Analytics.M5007 ?? false);
            return falseInfo * 10 > affAnalytics.Length
                ? CheckResult.Checked(
                    $"У {falseInfo} из {affAnalytics.Length} связанных организаций в ЕГРЮЛ указан признак недостоверности сведений в отношении руководителей или учредителей")
                : CheckResult.Failed();
        }

        public static CheckResult Marker27(AnalyticsValue[] affAnalytics)
        {
            var bankruptcyCases = affAnalytics.Count(a => (a.Analytics.Q7026 ?? 0) > 0);
            return bankruptcyCases * 10 > affAnalytics.Length
                ? CheckResult.Checked(
                    $"У {bankruptcyCases} из {affAnalytics.Length} связанных организаций имеются арбитражные дела о банктротстве в качестве ответчика")
                : CheckResult.Failed();
        }

        public static CheckResult Marker28(AnalyticsValue[] affAnalytics)
        {
            return CheckResult.Failed();
            var bankruptcyCase = affAnalytics.Count(a => (a.Analytics.Q7026 ?? 0) > 0);
            return bankruptcyCase * 10 > affAnalytics.Length
                ? CheckResult.Checked(
                    $"У {bankruptcyCase} из {affAnalytics.Length} связанных организаций имеются арбитражные дела о банктротстве в качестве ответчика")
                : CheckResult.Failed();
        }

        public static CheckResult Marker29(AnalyticsValue[] affAnalytics)
        {
            var salaryCases = affAnalytics.Count(a => a.Analytics.M1003 ?? false);
            return salaryCases * 10 > affAnalytics.Length
                ? CheckResult.Checked(
                    $"У {salaryCases} из {affAnalytics.Length} связанных организаций обнаружены исполнительные производства, предметом которых является заработная плата")
                : CheckResult.Failed();
        }

        public static CheckResult Marker30(AnalyticsValue[] affAnalytics)
        {
            var falseAddress = affAnalytics.Count(a => a.Analytics.M5006 ?? false);
            return falseAddress * 10 > affAnalytics.Length
                ? CheckResult.Checked(
                    $"У {falseAddress} из {affAnalytics.Length} связанных организаций в ЕГРЮЛ указан признак недостоверности в отношении адреса")
                : CheckResult.Failed();
        }

        public static CheckResult Marker31(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var casesSum = analytics.Analytics.S2001 ?? 0;
            var revenue = analytics.Analytics.S6004 ?? 0;
            var cap = egrDetails.Ul.StatedCapital.Sum;
            
            return casesSum > 5000000 && 
                   (cap == null || casesSum > cap) &&
                   casesSum * 10 > revenue
                ? CheckResult.Checked(
                    $"Сумма дел: {casesSum} . Выручка: {revenue} ")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker32(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M1005 ?? false};

        public static CheckResult Marker33(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M1004 ?? false};

        public static CheckResult Marker34(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M5005 ?? false};

        public static CheckResult Marker35(AnalyticsValue analytics)=>
            CheckResult.Failed();
        
        public static CheckResult Marker36(AnalyticsValue analytics)=>
            CheckResult.Failed();
        
        public static CheckResult Marker37(AnalyticsValue analytics)=>
            CheckResult.Failed();
        
        public static CheckResult Marker38(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M7001 ?? false,
                Verbose = "Рекомендована дополнительная проверка руководства и владельцев компании на номинальность."
            };
        
        public static CheckResult Marker39(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M5004 ?? false,
                Verbose = "Организация была найдена в списке юридических лиц, имеющих задолженность по уплате налогов более 1000 руб., которая направлялась на взыскание судебному приставу-исполнителю (ФНС)."
            };
        
        public static CheckResult Marker40(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var casesSum = analytics.Analytics.S2001 ?? 0;
            var revenue = analytics.Analytics.S6004 ?? 0;
            var cap = egrDetails.Ul.StatedCapital.Sum;
            
            return casesSum > 500000 && 
                   (cap == null || casesSum > cap) &&
                   casesSum * 10 > revenue
                ? CheckResult.Checked(
                    $"Сумма дел: {casesSum} . Выручка: {revenue} ")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker41(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var penaltySum = analytics.Analytics?.S1007;
            
            return penaltySum > 0
                ? CheckResult.Checked( $"У организации были найдены исполнительные производства, предметом которых являются налоги и сборы на сумму " + penaltySum)
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker42(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var penaltySum = analytics.Analytics?.S1008;
            
            return penaltySum > 0
                ? CheckResult.Checked( $"У организации были найдены исполнительные производства, предметом которых является страховые взносы на сумму " + penaltySum)
                : CheckResult.Failed();
        }

        public static CheckResult Marker43(AnalyticsValue analytics)
        {
            var reports = new []{"","",""};
            
            var casesCount = analytics.Analytics?.Q2031 ?? 0;
            if (casesCount > 0)
                reports[0] = $"\n- Банкротством. Всего дел {casesCount} на сумму {analytics.Analytics?.Q2031}";

            casesCount = analytics.Analytics?.Q2032 ?? 0;
            if (casesCount > 0)
                reports[1] = $"\n- Займами и кредитами. Всего дел {casesCount} на сумму {analytics.Analytics?.Q2032}";

            casesCount = analytics.Analytics?.Q2033 ?? 0;
            if (casesCount > 0)
                reports[0] = $"\n- Договорами поставки. Всего дел {casesCount} на сумму {analytics.Analytics?.Q2033}";

            return reports.Any(x => x != "") ? 
                CheckResult.Checked(string.Join("", reports)):
                CheckResult.Failed();
        }

        public static CheckResult Marker44(AnalyticsValue analytics) =>
            (!analytics.Analytics?.M7004 ?? false) ||
            (!analytics.Analytics?.M7003 ?? false) ||
            (!analytics.Analytics?.M7002 ?? false) ? 
                CheckResult.Checked() :
                CheckResult.Failed();

        public static CheckResult Marker45(AnalyticsValue analytics)=>
            CheckResult.Failed();

        public static CheckResult Marker46(AnalyticsValue analytics)=>
            CheckResult.Failed();

        public static CheckResult Marker47(ReqValue req, EgrDetailsValue egr)
        {
            var managers = req.Ul?.Heads;
            var founders = egr.Ul?.FoundersFl;
            if(managers == null || founders == null)
                return CheckResult.Failed();
            var match = managers.FirstOrDefault(m => founders.Select(f => f.Innfl).Contains(m.Innfl));
            return match != null ? 
                CheckResult.Checked($"Директор и учредитель одно физ. лицо {match.Fio}({match.Innfl})") : 
                CheckResult.Failed();
        }

        public static CheckResult Marker48(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var casesSum = analytics.Analytics.S2003 ?? 0;
            var revenue = analytics.Analytics.S6004 ?? 0;
            var cap = egrDetails.Ul.StatedCapital.Sum;
            
            return casesSum > 5000000 && 
                   (cap == null || casesSum > cap) &&
                   casesSum * 10 > revenue
                ? CheckResult.Checked($"Сумма дел: {casesSum} . Выручка: {revenue} ")
                : CheckResult.Failed();
        }

        public static CheckResult Marker49(AnalyticsValue analytics)
        {
            var nonliquidatedOffise = analytics.Analytics?.Q7006 ?? 0;
            var nonliquidatedNonoffise = analytics.Analytics?.Q7007 ?? 0;
            
            return nonliquidatedOffise > 10 || nonliquidatedNonoffise > 50
                ? CheckResult.Checked($"Кол-во не ликвидированных (с учетом № оф.): {nonliquidatedOffise}\n\t|Кол-во не ликвидированных (без учета № оф.): {nonliquidatedNonoffise}")
                : CheckResult.Failed();
        }

        public static CheckResult Marker50(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var casesSum = analytics.Analytics.S2003 ?? 0;
            var casesSumPast = analytics.Analytics.S2004 ?? 0;
            var revenue = analytics.Analytics.S6004 ?? 0;
            var cap = egrDetails.Ul.StatedCapital.Sum;

            return casesSum > 5000000 &&
                   casesSum * 3 > casesSumPast &&
                   (cap == null || casesSum > cap) &&
                   casesSum * 10 > revenue
                ? CheckResult.Checked(
                    $"Сумма дел: {casesSum} . Среднее за предыдущие два года: {(casesSumPast - casesSum) / 2} ")
                : CheckResult.Failed();
        }
        
        
        public static CheckResult Marker51(EgrDetailsValue egrDetails, AnalyticsValue analytics)
        {
            var casesSum = analytics.Analytics.S2001 ?? 0;
            var casesSumPast = analytics.Analytics.S2002 ?? 0;
            var revenue = analytics.Analytics.S6004 ?? 0;
            var cap = egrDetails.Ul.StatedCapital.Sum;

            return casesSum > 5000000 &&
                   casesSum * 3 > casesSumPast &&
                   (cap == null || casesSum > cap) &&
                   casesSum * 10 > revenue
                ? CheckResult.Checked(
                    $"Сумма дел: {casesSum} . Среднее за предыдущие два года: {(casesSumPast - casesSum) / 2} ")
                : CheckResult.Failed();
        }

        public static CheckResult Marker52(EgrDetailsValue egrDetails, AnalyticsValue analytics)=>
            CheckResult.Failed();
        
        public static CheckResult Marker53(AnalyticsValue analytics)=>
            new CheckResult{Result = analytics.Analytics?.M1006 ?? false};
        
        public static CheckResult Marker54(ReqValue req)=>
            new CheckResult{Result = req.Ul?.Status?.FluffyStatus?.Reorganizing ?? false};
        
        public static CheckResult Marker55(AnalyticsValue analytics)=>
            new CheckResult{Result = (!analytics.Analytics?.M7003 ?? false) && (analytics.Analytics.M7004 ?? false)};
        
        public static CheckResult Marker56(AnalyticsValue analytics)=>
            CheckResult.Failed();
        public static CheckResult Marker57(AnalyticsValue analytics)=>
            CheckResult.Failed();
        public static CheckResult Marker58(AnalyticsValue analytics)=>
            CheckResult.Failed();
        public static CheckResult Marker59(AnalyticsValue analytics)=>
            CheckResult.Failed();
        public static CheckResult Marker60(AnalyticsValue analytics)=>
            CheckResult.Failed();

        public static CheckResult Marker61(AnalyticsValue analytics)
        {
            var reports = new []{"","",""};
            
            var casesCount = analytics.Analytics?.Q2031 ?? 0;
            if (casesCount > 0)
                reports[0] = $"\n- изменение руководителя или управляющей компании";

            casesCount = analytics.Analytics?.Q2032 ?? 0;
            if (casesCount > 0)
                reports[1] = $"\n- изменение состава участников (владельцев)";

            casesCount = analytics.Analytics?.Q2033 ?? 0;
            if (casesCount > 0)
                reports[0] = $"\n- изменение юридического адреса";
            
            casesCount = analytics.Analytics?.Q2033 ?? 0;
            if (casesCount > 0)
                reports[0] = $"\n- изменение уставного капитала";

            return reports.Any(x => x != "") ? 
                CheckResult.Checked(string.Join("", reports)):
                CheckResult.Failed();
        }
        
        public static CheckResult Marker62(AnalyticsValue analytics)
        {
            var thisYear = analytics.Analytics?.S6004 ?? 0;
            var lastYear = analytics.Analytics?.S6003 ?? 0;
            var year = analytics.Analytics?.Q6001 ?? 0;

            return thisYear * 2 < lastYear
                ? CheckResult.Checked(
                    $"Выручка на конец {year} г.: {thisYear}. На начало {year} г.: {lastYear}\n|{Math.Abs(lastYear - thisYear)}")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker63(EgrDetailsValue egr)
        {
            var statedCapital = egr.Ul?.StatedCapital?.Sum ?? 0;

            return statedCapital > 0 && statedCapital < 100000
                ? CheckResult.Checked(
                    $"Уставный капитал организации: {statedCapital}")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker64(AnalyticsValue analytics)=>
            new CheckResult{Result = analytics.Analytics?.M5003 ?? false};
        
        public static CheckResult Marker65(AnalyticsValue analytics)
        {
            var registratedOffise = analytics.Analytics?.Q7008 ?? 0;
            var registratedNonoffise = analytics.Analytics?.Q7009 ?? 0;
            
            return registratedOffise > 10 || registratedNonoffise > 50
                ? CheckResult.Checked($"Кол-во когда-либо зарегистрированных (с учетом № оф.): {registratedOffise}\n\t|Кол-во когда-либо зарегистрированных (без учета № оф.): {registratedNonoffise}")
                : CheckResult.Failed();
        }

        public static CheckResult Marker66(AnalyticsValue analytics) =>
            CheckResult.Failed();
        
        public static CheckResult Marker67(AnalyticsValue analytics)=>
            new CheckResult{Result = analytics.Analytics?.M7004 ?? false};

        public static CheckResult Marker68(AnalyticsValue analytics) =>
            CheckResult.Failed();
        
        public static CheckResult Marker69(AnalyticsValue analytics) =>
            CheckResult.Failed();
        
        public static CheckResult Marker70(AnalyticsValue analytics) =>
            CheckResult.Failed();



/*

        private IReadOnlyDictionary<string,Func<object[],CheckResult>> dict = new Dictionary<string, Func<object[], CheckResult>>
        {
            {
                "Marker1", x =>
                {
                    var regDate = (x[0] as ReqValue)?.RegistrationDate;
                    var pureProfit = (x[1] as AnalyticsValue)?.Analytics.S6008;
                    if(pureProfit.HasValue && pureProfit<0 && DateTime.Today - regDate > TimeSpan.FromDays(365))
                        return new CheckResult
                        {
                            Result = true,
                            Verbose = "Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях): " 
                                    + pureProfit.Value
                        };
                    return CheckResult.Failed();
                }
            },
            {
                "Marker2", x => new CheckResult{Result = true}
            },
            {
                "Marker3", x =>
                {
                    if((x[0] as ReqValue)?.Ip == null)
                        return CheckResult.Failed();
                    var analyt = (x[1] as AnalyticsValue)?.Analytics;
                    if(analyt != null && analyt.M7014.HasValue && analyt.M7014.Value)
                        return new CheckResult
                        {
                            Result = true,
                            Verbose = "Текущая стадия банкротства: " + analyt.E7014
                        };
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker4", x =>
                {
                    /*var regDate = (x[0] as ReqValue)?.RegistrationDate;
                    var pureProfit = (x[1] as AnalyticsValue)?.Analytics.S6008;
                    if(pureProfit.HasValue && pureProfit<0 && DateTime.Today - regDate > TimeSpan.FromDays(365))
                        return new CheckResult
                        {
                            Result = true,
                            Verbose = "Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях): " 
                                      + pureProfit.Value
                        };#1#
                    return CheckResult.Failed();
                }
            },
            {
                "Marker5", x =>
                {
                    var req = x[0] as ReqValue;
                    
                    // ReSharper disable once PossibleInvalidOperationException
                    if (req?.Ip?.Status != null && !req.Ip.Status.Value.IsNull && req.Ip.Status.Value.PurpleStatus.Dissolved.Value ||
                        
                        req.Ul?.Status.Value != null && !req.Ul.Status.Value.IsNull && req.Ul.Status.Value.FluffyStatus.Dissolved.Value)
                        return CheckResult.Checked("Недействующее");
                    
                    if (req.Ul != null && req.Ul.Status.HasValue && !req.Ul.Status.Value.IsNull &&
                         // ReSharper disable once PossibleInvalidOperationException
                         req.Ul.Status.Value.FluffyStatus.Dissolving.Value)
                        return CheckResult.Checked("В процессе ликвидации (либо планируется исключение из ЕГРЮЛ)");
                    
                    return CheckResult.Failed();
                }
            },
            {
                "Marker6", x =>
                {
                    var analyt = (x[0] as AnalyticsValue)?.Analytics;
                    if ( analyt?.M7014 != null && analyt.M7014.Value && analyt.E7014 != null && analyt.E7014 != E7014.ПроизводствоПоДелуПрекращено && analyt.E7014 != E7014.ОтказаноВПризнанииДолжникаБанкротом)
                        return CheckResult.Checked("Число арбитражных дел о банкротстве в качестве ответчика: " + analyt.Q7026);
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker7", x =>
                {
                    var analyt = (x[0] as AnalyticsValue)?.Analytics;
                    if (analyt.M1003 != null && analyt.M1003.Value)
                        return CheckResult.Checked();
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker8", x =>
                {
                    var analyt = (x[0] as AnalyticsValue)?.Analytics;
                    if (analyt.M7015 != null && analyt.M7015.Value)
                        return CheckResult.Checked(
                            "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца");
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker9", x =>
                {
                    var analyt = (x[0] as AnalyticsValue)?.Analytics;
                    if (analyt.M5008 != null && analyt.M5008.Value)
                        return CheckResult.Checked("ФИО руководителей были найдены в реестре дисквалифицированных лиц (ФНС)");
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker10", x =>
                {
                    var analyt = (x[0] as AnalyticsValue)?.Analytics;
                    if (analyt.M7010 != null && analyt.M7010.Value)
                        return CheckResult.Checked(
                            "По состоянию на указанную дату действовало ограничение на операции по банковским счетам организации, установленное ИФНС - %1.");
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker11", x =>
                {
                    var req = x[0] as ReqValue;
                    var analyt = (x[1] as AnalyticsValue)?.Analytics;
                    var status = req.Status();
                    if (analyt.M7016 != null && analyt.M7016.Value && status != null)
                        return CheckResult.Checked(
                            "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца");
                    return CheckResult.Failed();
                }    
            },
            {
                "Marker8", x =>
                {
                    var analyt = (x[1] as AnalyticsValue)?.Analytics;
                    if (analyt != null && analyt.M7015.HasValue && analyt.M7015.Value)
                        return CheckResult.Checked(
                            "Обнаружены сообщения о намерении обратиться в суд с заявлением о банкротстве за последние 3 месяца");
                    return CheckResult.Failed();
                }    
            },
        };*/
    }
}