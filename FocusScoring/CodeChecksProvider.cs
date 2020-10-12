using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FocusAccess;
using FocusAccess.Response;
using FocusAccess.ResponseClasses;
using FocusMonitoring;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace FocusScoring
{
    public abstract class CodeChecksProvider : IParameterizedChecksProvider<INN>
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

        
        private static bool IsParametersMatch(IReadOnlyCollection<object> objects, MethodInfo info) => 
            objects.Count == info.GetParameters().Length &&
            objects.All(x => x != null) &&
            info.GetParameters().Zip(objects,IsMatch).All(x=>x);

        private static bool IsMatch(ParameterInfo t, object o) => !o.GetType().IsArray
            ? o.GetType() == t.ParameterType
            : o is Array array && t.ParameterType.IsArray &&
              array.Cast<IParameterValue>().All(v => v.GetType() == t.ParameterType.GetElementType());
        
        /*private static bool IsShallowMatch(ParameterInfo t, object o) => !o.GetType().IsArray
            ? o.GetType() == t.ParameterType
            : o is Array array && t.ParameterType.IsArray &&
              array.Cast<IParameterValue>().First(v => v.GetType() == t.ParameterType.GetElementType());*/
        
        public Func<object[], CheckResult> ProvideCheck(string markerKey)
        {
            MethodInfo methodInfo; //!Marker.CheckArguments.TryGetValue(MarkerArgName, out var checkArg) ||
            if ((methodInfo = GetType().GetMethod(markerKey)) == null)
                throw new KeyNotFoundException("Things went wrong.");
            var paramArray = methodInfo.GetParameters().Select(p => Expression.Parameter(p.ParameterType)).ToArray();
            var expr = Expression.Lambda(Expression.Call(methodInfo,paramArray),paramArray).Compile();
            return p =>
            {
                if (!IsParametersMatch(p, methodInfo))//TODO error with affil arrays 
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
        
        public IMarkerParameters ProvideParameters(string markerKey)
        {
            var parameters = GetType().GetMethod(markerKey)?.GetParameters() 
                ?? throw new ArgumentException("Invalid marker key.");
            return new MarkerParameters
            {
                MethodsUsed = parameters.Where(p=>typeof(IParameterValue).IsAssignableFrom(p.ParameterType)
                                                ||typeof(IParameterValue[]).IsAssignableFrom(p.ParameterType))
                        .Select(p => p.ParameterType.OriginalMethodOf()).ToArray(),
                History = parameters.Where(p=>typeof(MonitoringChanges<>).IsAssignableFrom(p.ParameterType))
                    .Select(p => p.ParameterType.OriginalMethodOf()).ToArray(),
            };
        }
    }

    class FocusChecksProvider : CodeChecksProvider
    {
        public static CheckResult Marker1(AnalyticsValue[] analytics)=>
            new CheckResult {Result = true};
        
        public static CheckResult Marker123(ReqValue req,AnalyticsValue analytics)
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
        
        public static CheckResult Marker9(AnalyticsValue analytics)
        {
            var sanctions = new (bool Check, string Descripion)[]
            {
                (analytics.Analytics.M8001 ?? false, "Организация в санкционном списке США"),
                (analytics.Analytics.M8002 ?? false, "Организация в секторальном санкционном списке США"),
                (analytics.Analytics.M8003 ?? false, "Организация в санкционном списке Евросоюза"),
                (analytics.Analytics.M8004 ?? false, "Организация в санкционном списке Великобритании"),
                (analytics.Analytics.M8005 ?? false, "Организация в санкционном списке Украины"),
                (analytics.Analytics.M8006 ?? false, "Организация в санкционном списке Швейцарии")
            };
            return new CheckResult
            {
                Result = sanctions.Any(s=>s.Check),
                Verbose = string.Join("\r\n",sanctions.Where(s => s.Check).Select(s => s.Descripion))
            };
        }

        public static CheckResult Marker10(AnalyticsValue analytics)=>
            new CheckResult {Result = analytics.Analytics?.M7022 ?? false };
        
        public static CheckResult Marker11(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics?.M5008 ?? false,
                Verbose = "ФИО руководителей были найдены в реестре дисквалифицированных лиц (ФНС)"
            };
        
        public static CheckResult Marker12(AnalyticsValue analytics)
        {
            var lastCheck = DateTime.Parse(analytics.Analytics?.D7010);
            return new CheckResult
            {
                Result = (analytics.Analytics?.M7010 ?? false) && DateTime.Today - lastCheck < TimeSpan.FromDays(30),
                Verbose = $"По состоянию на указанную дату действовало ограничение на операции по банковским счетам организации, установленное ИФНС - {lastCheck}. В настоящий момент это решение может быть уже отменено. Рекомендуем проверить текущее состояние банковских счетов."
            };
        }

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

        public static CheckResult Marker35(MonitoringChanges<ReqValue> reqs)
        {
            var managerChanges = reqs.ParseAsParameterValue(x => x.Ul.Heads, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var ownerChanges = reqs.ParseAsParameterValue(x => x.Ul.ManagementCompanies, DateTime.Now - TimeSpan.FromDays(365)).Length;
            
            var sanctions = new (bool Check, string Descripion)[]
            {
                (managerChanges >= 3, $"руководителей: {managerChanges}"),
                (ownerChanges > 0, $"упр. компаний: {ownerChanges}")
            };
            
            return new CheckResult
            {
                Result = sanctions.Any(s=>s.Check),
                Verbose = string.Join("\r\n",sanctions.Where(s => s.Check).Select(s => s.Descripion))
            };
        }
        
        public static CheckResult Marker36(MonitoringChanges<EgrDetailsValue> egrs)  
        {
            var changes = egrs.ParseAsParameterValue(x => x.Ul.FoundersFl, DateTime.Now - TimeSpan.FromDays(365)).Length
            + egrs.ParseAsParameterValue(x => x.Ul.FoundersUl, DateTime.Now - TimeSpan.FromDays(365)).Length
            + egrs.ParseAsParameterValue(x => x.Ul.FoundersForeign, DateTime.Now - TimeSpan.FromDays(365)).Length;
            
            
            return new CheckResult
            {
                Result = changes >= 3,
                Verbose = $"Количество изменений за 12 месяцев: {changes}"
            };
        }
        
        public static CheckResult Marker37(MonitoringChanges<ReqValue> reqs)
        {
            var changes = reqs.ParseAsParameterValue(x => x.Ul.LegalAddress, DateTime.Now - TimeSpan.FromDays(365)).Length;
            
            return new CheckResult
            {
                Result = changes >= 3,
                Verbose = $"Организация сменила юр. адрес за последние 12 месяцев: {changes}"
            };
        }

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
                reports[2] = $"\n- Договорами поставки. Всего дел {casesCount} на сумму {analytics.Analytics?.Q2033}";

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

        public static CheckResult Marker52(ReqValue req, MonitoringChanges<ReqValue> reqChanges)
        {
            var fios = reqChanges.ParseAsParameterValue(r => r.Ip.Fio);
            return fios.Length > 0
                ? CheckResult.Checked($"Новое ФИО: {req.Inn} Старое ФИО:{fios[fios.Length - 1]}")
                : CheckResult.Failed();
        }

        public static CheckResult Marker53(AnalyticsValue analytics)=>
            new CheckResult{Result = analytics.Analytics?.M1006 ?? false};
        
        public static CheckResult Marker54(ReqValue req)=>
            new CheckResult{Result = req.Ul?.Status?.FluffyStatus?.Reorganizing ?? false};
        
        public static CheckResult Marker55(AnalyticsValue analytics)=>
            new CheckResult{Result = (!analytics.Analytics?.M7003 ?? false) && (analytics.Analytics.M7004 ?? false)};
        
        public static CheckResult Marker56(MonitoringChanges<EgrDetailsValue> egrs)
        {
            var changes = egrs.ParseAsParameterValue(x => x.Ul.StatedCapital, DateTime.Now - TimeSpan.FromDays(365)).Length;
            return new CheckResult
            {
                Result = changes > 1,
                Verbose = $"Количество изменений уставного капитала за полгода: {changes}"
            };
        }
        
        public static CheckResult Marker57(MonitoringChanges<ReqValue> req)
        {
            var changes = req.ParseAsParameterValue(x => x.Ul.Kpp, DateTime.Now - TimeSpan.FromDays(365)).Length;
            return new CheckResult
            {
                Result = changes > 1,
                Verbose = $"Количество изменений КПП за полгода: {changes}"
            };
        }
        
        public static CheckResult Marker58(MonitoringChanges<ReqValue> req)
        {
            var changes = req.ParseAsParameterValue(x => x.Ul.Kpp, DateTime.Now - TimeSpan.FromDays(365)).Length;
            return new CheckResult
            {
                Result = changes > 1,
                Verbose = $"Количество изменений уставного капитала за полгода: {changes}"
            };
        }
        
        public static CheckResult Marker59(MonitoringChanges<ReqValue> req)
        {
            var changes = req.ParseAsParameterValue(x => x.Ul.LegalName, DateTime.Now - TimeSpan.FromDays(365)).Length;
            return new CheckResult
            {
                Result = changes > 1,
                Verbose = $"Организация сменила название за последние 12 месяцев: {changes}"
            };
        }
        
        public static CheckResult Marker60(MonitoringChanges<EgrDetailsValue> egrs)
        {
            var changes12 = egrs.ParseAsParameterValue(x => x.Ul.FoundersFl, DateTime.Now - TimeSpan.FromDays(365)).Length
                            +egrs.ParseAsParameterValue(x => x.Ul.FoundersUl, DateTime.Now - TimeSpan.FromDays(365)).Length
                            +egrs.ParseAsParameterValue(x => x.Ul.FoundersForeign, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = egrs.ParseAsParameterValue(x => x.Ul.FoundersFl, DateTime.Now - TimeSpan.FromDays(182)).Length
                            +egrs.ParseAsParameterValue(x => x.Ul.FoundersUl, DateTime.Now - TimeSpan.FromDays(182)).Length
                            +egrs.ParseAsParameterValue(x => x.Ul.FoundersForeign, DateTime.Now - TimeSpan.FromDays(182)).Length;
            
            return new CheckResult
            {
                Result = changes12 > 1 && changes6 > 0,
                Verbose = $"Количество изменений за 6 месяцев:  {changes6}"
            };
        }

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

        public static CheckResult Marker66(EgrDetailsValue egr, MonitoringChanges<EgrDetailsValue> egrs)
        {
            var changes = egrs
                .ParseAsParameterValue(
                    x => x.Ip.Activities.PrincipalActivity, DateTime.Now - TimeSpan.FromDays(365))
                .Concat(egrs
                    .ParseAsParameterValue(
                    x => x.Ul.Activities.PrincipalActivity,
                        DateTime.Now - TimeSpan.FromDays(365)))
                .ToArray();

            var activity = egr.Ip.Activities.PrincipalActivity ?? egr.Ul.Activities.PrincipalActivity;
            
            return new CheckResult
            {
                Result = changes.Length > 1,
                Verbose = $"Новое значение: {activity}, \r\n Старое значение: {changes[changes.Length-1]}"
            };
        }
        public static CheckResult Marker67(AnalyticsValue analytics)=>
            new CheckResult{Result = analytics.Analytics?.M7004 ?? false};

        public static CheckResult Marker68(MonitoringChanges<ReqValue> req)
        {
            var changes12 = req.ParseAsParameterValue(x => x.Ul.Kpp, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = req.ParseAsParameterValue(x => x.Ul.Kpp, DateTime.Now - TimeSpan.FromDays(182)).Length;
            //TODO ask Why check 12?
            return new CheckResult
            {
                Result = changes12 > 1 && changes6 > 0,
                Verbose = $"Количество изменений наименования за полгода: {changes6}"
            };
        }

        public static CheckResult Marker69(MonitoringChanges<ReqValue> req)
        {
            var changes12 = req.ParseAsParameterValue(x => x.Ul.LegalAddress, DateTime.Now - TimeSpan.FromDays(365)).Length;
            return new CheckResult
            {
                Result = changes12 > 2,
                Verbose = $"Организация сменила юр. адрес дважды за последние 12 месяцев"
            };
        }

        
        public static CheckResult Marker70(AnalyticsValue analytics) =>
            CheckResult.Failed();

        public static CheckResult Marker71(AnalyticsValue analytics)
        {
            var diff = (analytics.Analytics?.Q7018 ?? 0) - (analytics.Analytics?.Q7020 ?? 0);
            return diff < 10
                ? CheckResult.Failed()
                : CheckResult.Checked($"Количество не ликвидированных организаций: {diff}.");
        }
        
        public static CheckResult Marker72 (AnalyticsValue analytics)
        {
            var diff = analytics.Analytics?.Q7021 ?? 0;
            return diff < 10
                ? CheckResult.Failed()
                : CheckResult.Checked($"Количество ликвидированных юридических лиц: {diff}.");
        }
        
        public static CheckResult Marker73 (AnalyticsValue analytics)
        {
            var diff = analytics.Analytics?.Q7017 ?? 0;
            return diff < 10
                ? CheckResult.Failed()
                : CheckResult.Checked($"Количество юрлиц, в уставном капитале которых есть доля текущего юрлица: {diff}.");
        }
        
        public static CheckResult Marker74 (AnalyticsValue analytics)
        {
            var diff = analytics.Analytics?.Q7019 ?? 0;
            return diff < 10
                ? CheckResult.Failed()
                : CheckResult.Checked($"Количество не ликвидированных юридических лиц: {diff}.");
        }
        
        public static CheckResult Marker75(EgrDetailsValue egr,MonitoringChanges<EgrDetailsValue> egrs)
        {
            var changes = egrs
                .ParseAsParameterValue(
                    x => x.Ip.NalogRegBody.NalogCode, DateTime.Now - TimeSpan.FromDays(365))
                .Concat(egrs
                    .ParseAsParameterValue(
                        x => x.Ul.NalogRegBody.NalogCode,
                        DateTime.Now - TimeSpan.FromDays(365)))
                .ToArray();

            var code = egr.Ip.NalogRegBody.NalogCode ?? egr.Ul.NalogRegBody.NalogCode;

            return changes.Length > 0
                ? CheckResult.Checked($"Новое значение: {code}, \r\n Старое значение: {changes[changes.Length - 1]}")
                : CheckResult.Failed();
        }

        public static CheckResult Marker76 ( EgrDetailsValue egr, MonitoringChanges<EgrDetailsValue> egrs)
        {
            var changes = egrs
                .ParseAsParameterValue(
                    x => x.Ip.Activities.ComplementaryActivities, DateTime.Now - TimeSpan.FromDays(365))
                .Concat(egrs
                    .ParseAsParameterValue(
                        x => x.Ul.Activities.ComplementaryActivities,
                        DateTime.Now - TimeSpan.FromDays(365)))
                .ToArray();

            var activities = egr.Ip.Activities.ComplementaryActivities ?? egr.Ul.Activities.ComplementaryActivities;

            return changes.Length > 0
                ? CheckResult.Checked($"Новое значение: {activities}, \r\n Старое значение: {changes[changes.Length - 1]}")
                : CheckResult.Failed();
        }

        public static CheckResult Marker77(MonitoringChanges<EgrDetailsValue> egr)
        {
            var changes12 = egr.ParseAsParameterValue(x => x.Ul.StatedCapital, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = egr.ParseAsParameterValue(x => x.Ul.StatedCapital, DateTime.Now - TimeSpan.FromDays(182)).Length;
            //TODO ask Why check 12?
            return new CheckResult
            {
                Result = changes12<=1 && changes6 > 0,
                Verbose = $"Количество изменений уставного капитала за полгода: {changes6}"
            };
        }

        public static CheckResult Marker78(MonitoringChanges<ReqValue> req)
        {
            var changes12 = req.ParseAsParameterValue(x => x.Ul.LegalName, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = req.ParseAsParameterValue(x => x.Ul.LegalName, DateTime.Now - TimeSpan.FromDays(182)).Length;
            //TODO ask Why check 12?
            return new CheckResult
            {
                Result = changes12 > 1 && changes6 > 0,
                Verbose = $"Количество изменений наименования за полгода: {changes6}"
            };
        }

        public static CheckResult Marker79(MonitoringChanges<ReqValue> req)
        {
            var changes12 = req.ParseAsParameterValue(x => x.Ul.Heads, DateTime.Now - TimeSpan.FromDays(365)).Length
                +req.ParseAsParameterValue(x => x.Ul.ManagementCompanies, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = req.ParseAsParameterValue(x => x.Ul.Heads, DateTime.Now - TimeSpan.FromDays(182)).Length
                +req.ParseAsParameterValue(x => x.Ul.ManagementCompanies, DateTime.Now - TimeSpan.FromDays(182)).Length;
            //TODO ask Why check 12?
            return new CheckResult
            {
                Result = changes12 > 1 && changes6 > 0,
                Verbose = $"Количество изменений руководителей и упр. компаний за полгода: {changes6}"
            };
        }
        public static CheckResult Marker80(MonitoringChanges<ReqValue> req)
        {
            var changes12 = req.ParseAsParameterValue(x => x.Ul.LegalAddress, DateTime.Now - TimeSpan.FromDays(365)).Length;
            var changes6 = req.ParseAsParameterValue(x => x.Ul.LegalAddress, DateTime.Now - TimeSpan.FromDays(182)).Length;
            //TODO ask Why check 12?
            return new CheckResult
            {
                Result = changes12 > 1 && changes6 > 0,
                Verbose = $"Количество изменений наименования за полгода: {changes6}"
            };
        }
        public static CheckResult Marker81(EgrDetailsValue egr)
        {
            var forCount = egr.Ul.FoundersForeign.Length;
            return forCount < 1
                ? CheckResult.Failed()
                : CheckResult.Checked($"Количество иностранных учредителей: {forCount}.");
        }

        public static CheckResult Marker82(ReqValue flreq, ReqValue[] reqs, EgrDetailsValue[] egrs)
        {
            var list = new List<ReqValue>();
            foreach (var (egr, req) in egrs.Zip(reqs,ValueTuple.Create))
            {
                var innCheck = false;
                FounderFl[] founders;
                Head[] managers;
                if (req.Ul != null)
                {
                    if (req.Ul.Status?.FluffyStatus?.Dissolved ?? false)
                    {
                        founders = egr.Ul.FoundersFl ?? new FounderFl[0];
                        managers = req.Ul.Heads;
                    }
                    else
                    {
                        founders = egr.Ul.History.FoundersFl ?? new FounderFl[0];
                        managers = req.Ul.History.Heads;
                    }

                    foreach (var founder in founders)
                        if(founder.Innfl == flreq.Inn)
                        {
                            list.Add(req);
                            innCheck = true;
                        }

                    if (innCheck) continue;
                    
                    foreach (var manager in managers)
                        if(manager.Innfl == flreq.Inn)
                        {
                            list.Add(req);
                            innCheck = true;
                        }
                }
                else if(req.Ip.Status?.PurpleStatus?.Dissolved ?? false) 
                    list.Add(req);
            }
            return list.Count < 1
                ? CheckResult.Failed()
                : CheckResult.Checked(string.Join("\r\n",
                    list.Select(r=>$"{r.Inn}: {r.Ul?.LegalName.Short ?? r.Ip.Fio}")));
        }
        
        public static CheckResult Marker83(EgrDetailsValue[] egrs, AnalyticsValue[] analyticses)
        {
            var count = analyticses.Zip(egrs, ValueTuple.Create).Count(t => Marker40(t.Item2, t.Item1).Result);
            return count * 10 < egrs.Length * 3
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {egrs.Length} связанных организаций значительная сумма исполнительных производств");
        }
        
        public static CheckResult Marker84(AnalyticsValue[] analyticses)
        {
            var count = analyticses.Count(a => Marker38(a).Result);
            return count * 10 < analyticses.Length
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {analyticses.Length} связанных организаций рекомендована дополнительная проверка");
        }
        
        public static CheckResult Marker85(EgrDetailsValue[] egrs, AnalyticsValue[] analyticses)
        {/*
            var counts = new int[] { };
            var countDisqual = analyticses.Count(a => Marker11(a).Result);
            var countAddr = analyticses.Count(a => Marker16(a).Result);
            var countTax = analyticses.Count(a => Marker39(a).Result);
            var countReport = analyticses.Count(a => Marker34(a).Result);*/

            var results = analyticses.Select(a =>
                    ValueTuple.Create(
                        Marker11(a).Result, 
                        Marker16(a).Result, 
                        Marker39(a).Result, 
                        Marker34(a).Result))
                .ToArray();

            var count = results.Count(t => t.Item1 || t.Item2 || t.Item3 || t.Item4);

            return  count * 10 < analyticses.Length * 3
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {egrs.Length} связанных организаций рекомендована дополнительная проверка");
        }
        public static CheckResult Marker86(EgrDetailsValue[] egrs, AnalyticsValue[] analyticses)
        {/*
            var counts = new int[] { };
            var countDisqual = analyticses.Count(a => Marker11(a).Result);
            var countAddr = analyticses.Count(a => Marker16(a).Result);
            var countTax = analyticses.Count(a => Marker39(a).Result);
            var countReport = analyticses.Count(a => Marker34(a).Result);*/

            var results = analyticses.Select(a =>
                    ValueTuple.Create(
                        Marker7(a).Result, 
                        Marker33(a).Result, 
                        Marker32(a).Result, 
                        Marker53(a).Result))
                .ToArray();

            var count = results.Count(t => t.Item1 || t.Item2 || t.Item3 || t.Item4);

            return  count * 10 < analyticses.Length * 3
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {egrs.Length} связанных организаций рекомендована дополнительная проверка");
        }
        
        public static CheckResult Marker87(AnalyticsValue[] analyticses)
        {
            var revPast = analyticses.Sum(a => a.Analytics.S6003 ?? .0);
            var rev = analyticses.Sum(a => a.Analytics.S6004 ?? .0);
            return rev != .0 && rev * 10 > revPast * 7
                ? CheckResult.Failed()
                : CheckResult.Checked($"По группе компаний выручка снизилась на {100*(revPast-rev)/revPast}%.");
        }
        
        public static CheckResult Marker88(ReqValue[] reqs)
        {
            return CheckResult.Failed();
            /*var revPast = reqs.Sum(a => Marker80());
            var rev = analyticses.Sum(a => a.Analytics.S6004 ?? .0);
            return rev != .0 && rev * 10 > revPast * 7
                ? CheckResult.Failed()
                : CheckResult.Checked($"По группе компаний выручка снизилась на {100*(revPast-rev)/revPast}%.");*/
        }
        
        public static CheckResult Marker89(EgrDetailsValue[] egrs, AnalyticsValue[] analyticses)
        {
            var count = analyticses
                .Zip(egrs, ValueTuple.Create)
                .Count(t => Marker48(t.Item2, t.Item1).Result || 
                            Marker32(t.Item1).Result);
            return count * 10 < egrs.Length * 3
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {egrs.Length} связанных организаций значительная сумма арбитражных дел");
        }
        
        public static CheckResult Marker90(MonitoringChanges<ReqValue>[] reqs)
        {
            var count = reqs
                .Count(t => Marker68(t).Result);
            return count * 10 > reqs.Length * 3
                ? CheckResult.Failed()
                : CheckResult.Checked($"У {count} из {reqs.Length} зарегистрированы менее 12 мес назад");
        }
        
        public static CheckResult Marker91(AnalyticsValue analyticses)
        {
            var prop1 = analyticses.Analytics.Q4002;
            var sum1 = analyticses.Analytics.S4002;
            var prop2 = analyticses.Analytics.Q4004;
            var sum2 = analyticses.Analytics.S4004;
            return prop1 + prop2 > 0
                ? CheckResult.Checked(
                    $"Участвовал в качестве поставщика: {prop1} (на сумму: {sum1}).\r\n|Участвовал в качестве заказчика: {prop2} (на сумму {sum2}).")
                : CheckResult.Failed();
        }

        public static CheckResult Marker92(AnalyticsValue analytics)=>
            new CheckResult
            {
                Result = analytics.Analytics.M5005.HasValue,
                Verbose =  "Количество товарных знаков, действующих или недействующих, в которых упоминается текущая компания:"+ analytics.Analytics.M5005 ?? ""
            };
        
        public static CheckResult Marker93(ReqValue req)=>
            new CheckResult
            {
                Result = DateTime.Today - req.RegistrationDate < TimeSpan.FromDays(5 * 365),
                Verbose =  "Дата образования: "+ (req.RegistrationDate.ToString() ?? "")
            };

        public static CheckResult Marker94(AnalyticsValue analytics) =>
            new CheckResult {Result = analytics.Analytics.M6002 ?? false};
        public static CheckResult Marker95(EgrDetailsValue egr)=>
            new CheckResult
            {
                Result = (egr.Ul.StatedCapital.Sum ?? 0) > 100000.0,
                Verbose =  "Уставный капитал: " + (egr.Ul.StatedCapital.Sum ?? 0)
            };

        public static CheckResult Marker96(AnalyticsValue analytics)
        {
            var defend = analytics.Analytics.Q2001;
            var plaintiff = analytics.Analytics.Q2003;
            return defend.HasValue || plaintiff.HasValue
                ? CheckResult.Checked($"Кол-во дел в качестве ответчика: {defend}.\r\nКол-во дел в качестве истца: {plaintiff}.")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker97(ReqValue req)=>
            new CheckResult
            {
                Result = req.Ul != null && req.Ul.Branches!= null && req.Ul.Branches.Length>0,
                Verbose =  "Количество филиалов\\представительств: " + (req.Ul?.Branches?.Length ?? 0)
            };
        
        public static CheckResult Marker98(AnalyticsValue[] analyticses)
        {
            var count = analyticses
                .Count(t => Marker92(t).Result);
            return count * 10 > analyticses.Length * 2
                ? CheckResult.Checked(
                    $"У {count} из {analyticses.Length} связанных организаций имеют государственные контракты.")
                : CheckResult.Failed();
        }
        public static CheckResult Marker99(ReqValue[] reqs) =>
            new CheckResult {Result = reqs?.Any() ?? false};
        
        public static CheckResult Marker100(ReqValue[] reqs)
        {
            var count = reqs
                .Count(t => Marker93(t).Result);
            return count * 10 > reqs.Length * 3
                ? CheckResult.Checked(
                    $"У {count} из {reqs.Length} связанных организаций зарегистрированы более 5 лет тому назад")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker101(AnalyticsValue[] analyticses)
        {
            var count = analyticses
                .Count(t => Marker94(t).Result);
            return count * 10 > analyticses.Length * 5
                ? CheckResult.Checked(
                    $"У {count} из {analyticses.Length} связанных организаций предоставили бухгалтерскую отчетность за предыдущий период.")
                : CheckResult.Failed();
        }
        
        public static CheckResult Marker102(EgrDetailsValue[] egrs)
        {
            var count = egrs
                .Count(t => Marker95(t).Result);
            return count * 10 > egrs.Length * 3
                ? CheckResult.Checked(
                    $"У {count} из {egrs.Length} связанных организаций есть практика арбитражных дел за последние 12 мес.")
                : CheckResult.Failed();
        }

        public static CheckResult Marker103(EgrDetailsValue[] egrs) =>
            CheckResult.Failed();
        
        public static CheckResult Marker104(LicencesValue licenses)
        {
            var count = licenses.Licenses
                .Count(l => l.DateEnd == null || DateTime.Parse(l.DateEnd) > DateTime.Today);
            return count * 10 > 0
                ? CheckResult.Checked(
                    $"Количество действующих лицензий: {count}")
                : CheckResult.Failed();
        }



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