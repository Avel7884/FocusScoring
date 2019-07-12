using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using FocusScoring;

namespace FocusScoring
{
    public class Company
    {
        private string inn;
        private static Dictionary<string, (ApiMethod, string)> paramDict = new Dictionary<string, (ApiMethod, string)>()
        {
            {"Short",(ApiMethod.req,"/ArrayOfreq/req/UL/legalName/short") },
            {"Full",(ApiMethod.req,"/ArrayOfreq/req/UL/legalName/full") },
            {"FIO",(ApiMethod.req,"/ArrayOfreq/req/IP/fio") },
            {"Dissolving", (ApiMethod.req,"/ArrayOfreq/req/UL/status/dissolving")},
            {"Dissolved", (ApiMethod.req,"/ArrayOfreq/req/UL/status/dissolved")},
            {"legalAddress",(ApiMethod.req,"/ArrayOfreq/req/UL/history/legalAddresses/legalAddress") },
            {"legalName",(ApiMethod.req,"/ArrayOfreq/req/UL/history/legalNames/legalName") },
            {"kppWithDate",(ApiMethod.req,"/ArrayOfreq/req/UL/history/kpps/kppWithDate") },
            {"Reorganizing",(ApiMethod.req,"/ArrayOfreq/req/UL/status/reorganizing") },
            {"head",(ApiMethod.req,"/ArrayOfreq/req/UL/heads/head/innfl") },
            {"regDate",(ApiMethod.req,"/ArrayOfreq/req/UL/registrationDate") },
            {"managementCompany",(ApiMethod.req,"/ArrayOfreq/req/UL/managementCompanies/managementCompany") },
            {"RegistrationDate",(ApiMethod.req,"/ArrayOfreq/req/UL/registrationDate") },
            {"Branches",(ApiMethod.req,"ArrayOfreq/req/UL/branches/name") },
            {"m7013",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7013")},
            {"m7014",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7014")},
            {"m7016",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7016") },
            {"s1001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s1001") },
            {"s6003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6003") },
            {"s6004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6004") },
            {"s6008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6008") },
            {"Sum",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/statedCapital/sum") },
            {"s2003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2003") },
            {"s2004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2004") },
            {"s2001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2001") },
            {"s2002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2002") },
            {"m4001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m4001") },
            {"m5008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5008") },
            {"m7022",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7022") },
            {"FounderFL",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersFL/founderFL/innfl") },
            {"FoundersForeign",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersForeign/founderForeign") },
            {"m1003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1003") },
            {"m1004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1004") },
            {"m1005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1005") },
            {"m1006",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1006") },
            {"s1007",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s1007") },
            {"s1008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s1008") },
            {"q2003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2003") },
            {"q2004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2004") },
            {"q2001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2001") },
            {"q2002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2002") },
            {"m5002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5002") },
            {"m5003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5003") },
            {"m5004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5004") },
            {"m5005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5005") },
            {"m5006",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5006") },
            {"m5007",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5007") },
            {"m6002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m6002") },
            {"m7001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7001") },
            {"m7002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7002") },
            {"m7003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7003") },
            {"m7004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7004") },
            {"q4002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q4002") },
            {"q4004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q4004") },
            {"q7005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7005") },
            {"q7006",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7006") },
            {"q7007",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7007") },
            {"q7008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7008") },
            {"q7009",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7009") },
            {"q7017",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7017") },
            {"q7018",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7018") },
            {"q7019",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7019") },
            {"q7020",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7020") },
            {"q7021",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7021") },  
            {"q9001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q9001") },  
            {"DissolvingAffiliates", (ApiMethod.companyAffiliatesreq, "/ArrayOfreq/req/UL/status/dissolving")},
            {"DissolvedAffiliates", (ApiMethod.companyAffiliatesreq, "/ArrayOfreq/req/UL/status/dissolved")},
            {"InnAffilalates",(ApiMethod.companyAffiliatesreq,"/ArrayOfreq/req/inn")},
            {"m7013Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7013")},
            {"m7014Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7014")},
            {"m7016Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7016")},
            {"s6003Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/s6003")},
            {"s6004Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/s6004")},
            {"q7005Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q7005")},
            {"q4002Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q4002")},
            {"q4004Affiliates", (ApiMethod.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q4004")},
            {"SumAffiliates", (ApiMethod.companyAffiliatesegrDetails, "/ArrayOfegrDetails/egrDetails/UL/statedCapital/sum")},
            {"s1001Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s1001")},
            {"s2003Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s2003")},
            {"s2001Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s2001")},
            {"s1003Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1003")},
            {"s1004Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1004")},
            {"s1005Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1005")},
            {"s1006Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1006")},
            {"m5002Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5002")},
            {"m5004Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5004")},
            {"m5006Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5006")},
            {"m5007Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5007")},
            {"m7001Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m7001")},
            {"m7004Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m7004")},
            {"q2003Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/q2003")},
            {"q2001Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/q2001")},
            {"m6002Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m6002")},
            {"regDateAffiliates", (ApiMethod.companyAffiliatesreq, "/ArrayOfreq/req/UL/registrationDate")},
        };

        private ParamAccess access;

        private Company(ParamAccess paramAccess = null)
        {
            access = ParamAccess.Start();
            InitMarkers();
            score = -1;
        }

        public static Company CreateINN(string inn)
        {
            var c = new Company();
            c.inn = inn;
            return c;
        }

        public string GetParam(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParam(method, inn, node);
        }

        public string[] GetMultiParam(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParams(method, inn, node).ToArray();
        }
        //TODO Rename
        public string[] GetMultiParam2(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetMultiParam(method, inn, node).ToArray();
        }

        public bool GetMarker(string markerName)
        {
            return markersDict[markerName].Check();
        }


        public string CompanyName()
        {
            string _short = GetParam("Short");
            string full = GetParam("Full");
            string fio = GetParam("FIO");
            if (_short != "")
                return _short;
            if (full != "")
                return full;
            if (fio != "")
                return "ИП" + " " + fio;
            return "";
        }


        private Dictionary<string, Marker> markersDict;

        private static List<Marker> markersList;

        //TODO move in another class, also refactor
        public static Company DummyCompany = new Company();
        public Marker[] GetAllMarkers => markersList.ToArray();

        private int score;
        public int Score
        {
            get
            {
                if (score < 0)
                    score = ConutScore(Markers);
                return score;
            }
        }

        private Marker[] markers = null;
        public Marker[] Markers
        {
            get
            {
                if (markers == null)
                    markers = GetMarkers();
                return markers;
            }
        }

        //TODO refactor all
        private int ConutScore(Marker[] markers)
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
                if (marker.Colour == MarkerColour.Green|| marker.Colour == MarkerColour.GreenAffiliates)
                    greenSum+= marker.Score;
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
                .Where(marker => marker.Colour == MarkerColour.Green|| marker.Colour == MarkerColour.GreenAffiliates)
                .Select(x => x.Score).Sum();

            var score = 79 - redSum - yellowSum + greenSum;
            return Math.Min(maxScore, score);
        }
        //TODO Rename Refactor AAAAAA 
        private Marker[] GetMarkers()
        {
            return markersList.Where(marker=>marker.Check()).ToArray();
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
                new Marker("Статус компании связан с произошедшей или планируемой ликвидацией", MarkerColour.Red,"Статус организации принимает значение: недействующее, в стадии ликвидации", 5,
                    () => GetParam("Dissolving") == "true" || GetParam("Dissolved") == "true"),

                new Marker("Вероятное банкротство организации", MarkerColour.Red,"Обнаружены арбитражные дела о банкротстве за последние 3 месяца \n " +
                "Обнаружены сообщение о банкротстве за последние 12 месяцев \n " +
                "Обнаружены признаки завершенной процедуры банкротства", 5,
                    () => GetParam("m7013") == "true" || GetParam("m7014") == "true" || GetParam("m7016") == "true"),

                new Marker("Критическая сумма исполнительных производств", MarkerColour.Red, "Критическая сумма исполнительных производств " +
                                                      "(сумма исполнительных производств составляет более 20% от выручки организации за последний отчетный период) " +
                                                      "и более суммы уставного капитала, и более 100 тыс. руб.", 4,
                    () =>
                    { //TODO possible error need consideration...  or not  
                        if(DoubleTryParse("Sum",out double sum) && DoubleTryParse("s1001",out double a) && DoubleTryParse("s6004",out double b))
                            return a > (0.2 * b) && a > sum & a > 500000;
                        return false;
                    }),

                new Marker("Критический рост суммы арбитражных дел в качестве истца", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве истца за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    () =>
                    {
                        if (DoubleTryParse(GetParam("s2003"), out double sumDel) &&
                            DoubleTryParse(GetParam("s2004"), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),

                new Marker("Критический рост суммы арбитражных дел в качестве ответчика", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве ответчика за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    () =>
                    {
                        if (DoubleTryParse(GetParam("s2001").Replace('.',','), out double sumDel) &&
                            DoubleTryParse(GetParam("s2002").Replace('.',','), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),

                  new Marker("Критический сумма арбитражных дел в качестве истца", MarkerColour.Red, "Критическая сумма арбитражных дел в качестве ответчика." +
                  "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период и более суммы уставного капитала, " +
                  "и более 500 тыс. руб.",1,
                  () => {
                        if(DoubleTryParse(GetParam("s2001").Replace('.',','),out double sumDel) && DoubleTryParse(GetParam("s6004").Replace('.',','),out double revenue) && DoubleTryParse(GetParam("Sum").Replace('.',','),out double statedCapitalFocus))
                           return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                  }),

                new Marker("Критический сумма арбитражных дел в качестве ответчика", MarkerColour.Red, "Критическая сумма арбитражных дел в качестве истца." +
                                                      "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период" +
                                                      "и более суммы уставного капитала, и более 500 тыс. руб.", 1,
                    () =>
                    {
                        if (DoubleTryParse(GetParam("s2003"), out double sumDel) && DoubleTryParse(GetParam("s6004"),out double revenue)
                                                    && DoubleTryParse(GetParam("Sum"), out double statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                    }),

                new Marker("В реестре недобросовестных поставщиков", MarkerColour.Red,
                    "Организация была найдена в реестре недобросовестных поставщиков" +
                    "(ФАС, Федеральное Казначейство)", 3,
                    () => { return GetParam("m4001") == "true"; }),

                new Marker("Руководство в реестре дисквалифицированных лиц", MarkerColour.Red,
                    "ФИО руководителей или учредителей были найдены в реестре дисквалифицированных лиц (ФНС)", 4,
                    () => { return GetParam("m5008") == "true"; }),

                new Marker("Существенное снижение выручки", MarkerColour.Red, "Выручка снизилась более, чем на 50%", 1,
                    () =>
                    {
                        if (long.TryParse(GetParam("s6004").Replace('.',','), out long s6004) &
                            long.TryParse(GetParam("s6003").Replace('.',','), out long s6003))
                            return s6004 < (0.5 * s6003);
                        return false;
                    }),

                new Marker("Руководитель либо учредитель компании банкрот", MarkerColour.Red, "Наличие сообщений о банкротстве физлица, " +
                                                       "являющегося руководителем (лицом с правом подписи без доверенности), " +
                                                       "учредителем, либо индивидуальным предпринимателем. Необходимо изучить сообщения",
                    3,
                    () => { return GetParam("m7022") == "true"; }),

                new Marker("Более половины связных организаций в стадии ликвидации",MarkerColour.RedAffiliates,"У более чем 50% связанных организаций статус связан с произошедшей или планируемой ликвидацией",4,
                    ()=>{
                    var Dissolved = GetMultiParam("DissolvedAffiliates").Length;
                    var Dissolving = GetMultiParam("DissolvingAffiliates").Length;
                    int affiliatesCount = GetMultiParam("InnAffilalates").Length;
                        return (Dissolved+Dissolving)/affiliatesCount * 100 > 50;
                    }),

                new Marker("Более половины связных организаций имеют признаки банкротства",MarkerColour.RedAffiliates,"У более чем 50% связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний",5,
                    ()=>{
                        //TODO Check for interceptions
                       int affiliatesCount = GetMultiParam("InnAffilalates").Length;
                       var m7013 = GetMultiParam("m7013Affiliates").Length;
                       var m7014 = GetMultiParam("m7014Affiliates").Length;
                       var m7016 = GetMultiParam("m7016Affiliates").Length;
                        return (m7013 + m7014 + m7016 )/affiliatesCount * 100 > 50;
                    }),

                new Marker("Выручка по группе компаний снизилась более, чем на 50%",MarkerColour.RedAffiliates,"Выручка по группе компаний снизилась более, чем на 50%",3,
                    ()=>{
                        //TODO Check for correct
                        double s6004;
                        s6004 = GetMultiParam("s6004Affiliates").Select(x=>x.Replace('.',',')).Sum(x=>double.Parse(x));
                        double s6003;
                        s6003 = GetMultiParam("s6003Affiliates").Select(x=>x.Replace('.',',')).Sum(x=>double.Parse(x));
                        return s6004 < 0.5 * s6003;
                    }),

                new Marker("Критическая сумма исполнительных производств по группе компаний",MarkerColour.RedAffiliates,"Критическая сумма исполнительных производств по группе компаний",4,
                    () =>
                    {
                        var revs = GetMultiParam2("s6004Affiliates");
                        var cases = GetMultiParam2("s1001Affiliates");
                        var sums = GetMultiParam2("SumAffiliates");
                        var count = .0;
                        for(int i=0;i<sums.Length;i++)
                            if(DoubleTryParse(sums[i],out double sum) &&
                               DoubleTryParse(cases[i],out double a) &&
                               DoubleTryParse(revs[i],out double b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;

                    }),
                new Marker("Критическая сумма арбитражных дел по группе компаний",MarkerColour.RedAffiliates,"У болле чем 30% связанных организаций сработал маркер критическая сумма арбитражных дел",1,
                    () => 
                    {
                        var casesIst = GetMultiParam2("s2003Affiliates");
                        var sums = GetMultiParam2("SumAffiliates");
                        var revs = GetMultiParam2("s6004Affiliates");
                        var casesOtv = GetMultiParam2("s2001Affiliates");

                        var count = .0;
                        for (int i = 0; i < sums.Length;i++)
                            if(DoubleTryParse(revs[i],out double rev) &&
                               DoubleTryParse(sums[i],out double sum) &&
                               ((DoubleTryParse(casesIst[i], out double caseIst) &&
                                 (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                                (DoubleTryParse(casesOtv[i], out double caseOtv) &&
                                 (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                                count += 1;

                        return count / sums.Length > 0.3;
                    }),

                new Marker("Индивидуальный предприниматель сменил ФИО",MarkerColour.Yellow,"Индивидуальный предприниматель сменил ФИО",3,
                    ()=> FIOCache.HasChanged(inn, GetParam("FIO"))),
                
                new Marker("Организация в процессе реорганизации",MarkerColour.Yellow,"Находится в процессе реорганизации в форме присоединения к другому юридическому лицу (слияние, присоединение и т.д.)",3,
                    ()=>{return GetParam("Reorganizing")=="true";}),
                new Marker("Директор и учредитель одно физическое лицо",MarkerColour.Yellow,"Директор и учредитель одно физическое лицо",1,
                    ()=>{
                       var a = GetMultiParam("head");
                        var b = GetMultiParam("FounderFL");
                        return a.Any(x=>b.Any(y=>x==y));
                    }),

                new Marker("Среди учредителей найдены иностранные лица",MarkerColour.Yellow,"Среди учредителей найдены иностранные лица",1,
                    ()=>{return GetParam("FoundersForeign")=="";}),
                    new Marker("Значительная сумма исполнительных производств",MarkerColour.Yellow,"Значительная сумма исполнительных производств. " +
                    "Т.е. сумма исполнительных производств составляет более 10% от выручки организации за последний отчетный период " +
                    "и более суммы уставного капитала, и более 100 тыс. руб.",4,
                    ()=>{
                         if(DoubleTryParse(GetParam("Sum").Replace('.',','),out double sum)
                        && DoubleTryParse(GetParam("s1001").Replace('.',','),out double a)
                        && DoubleTryParse(GetParam("s6004").Replace('.',','),out double b))
                            return  a > 0.1 * b & a > sum & a > 100000;
                        return false;
                    }),

                new Marker("Исполнительные производства (заработная плата)",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых является заработная плата",5,
                    ()=>{return GetParam("m1003")=="true";}),

                new Marker("Исполнительные производства (наложение ареста)",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых является наложение ареста",5,
                    ()=>{return GetParam("m1004")=="true";}),

                new Marker("Исполнительные производства (кредитные платежи)",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых является кредитные платежи",5,
                    ()=>{return GetParam("m1005")=="true";}),

                new Marker("Исполнительные производства (взыскание заложенного имущества",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых является обращение взыскания на заложенное имущество",3,
                    ()=>{return GetParam("m1006")=="true";}),

                new Marker("Исполнительные производства (налоги и сборы)",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых являются налоги и сборы",4,
                    ()=>{return GetParam("s1007")=="true";}),

                new Marker("Исполнительные производства (страховые взносы)",MarkerColour.Yellow,"У организации были найдены исполнительные производства, предметом которых являются страховые взносы",4,
                    ()=>{return GetParam("s1008")=="true";}),
                
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
                
                new Marker("Отсутствует связь по юр. адресу",MarkerColour.Yellow,"Организация была найдена в списке организаций, связь с которыми по указанному или юридическому адресу отсутствует(ФНС)",3,
                    ()=>{return GetParam("m5002")=="true";}),

                new Marker("Недостоверные сведение об адресе",MarkerColour.Yellow,"В ЕГРЮЛ указан  признак недостоверности сведений в отношении адреса",4,
                    ()=>{return GetParam("m5006")=="true";}),

                new Marker("Недостоверные сведения о руководителе или учредителе",MarkerColour.Yellow,"В ЕГРЮЛ указан признак недостоверности сведений в отношении руководителя или учредителей",5,
                    ()=>{return GetParam("m5007")=="true";}),

                new Marker("Задолженность по уплате налогов",MarkerColour.Yellow,"Организация была найдена в списке юридических лиц, " +
                    "имеющих задолженность по уплате налогов более 1000руб, которая направлялась на взыскание судебному приставу-исполнителю (ФНС)",4,
                    ()=>{return GetParam("m5004")=="true";}),

                new Marker("Не предоставляет отчетность более года",MarkerColour.Yellow,"Организация была найдена в списке юридических лиц, не представляющих налоговую отчетность более года",5,
                    ()=>{return GetParam("m5005")=="true";}),

                new Marker("Рекомендована дополнительная проверка",MarkerColour.Yellow,"Рекомендована дополнительная проверка руководства и владельцев компании на номинальности",5,
                    ()=>{return GetParam("m7001")=="true";}),

                new Marker("Организация зарегистрирована менее 3 мес назад",MarkerColour.Yellow,"Организация зарегистрирована менее 3 месяцев тому назад",4,
                    ()=>{return GetParam("m7004")!="true" && GetParam("m7003")!="true" && GetParam("m7002")!="true"; }),

                new Marker("Организация зарегистрирована менее 6 мес назад",MarkerColour.Yellow,"Организация зарегистрирована менее 6 месяцев тому назад",3,
                    ()=>{return GetParam("m7004")!="true" && GetParam("m7003")=="true"; }),

                new Marker("Организация зарегистрирована менее 12 мес назад",MarkerColour.Yellow,"Организация зарегистрирована менее 12 месяцев тому назад",2,
                    ()=>{return GetParam("m7004")=="true"; }),
                new Marker("Значительное кол-во учрежденных юр.лиц",MarkerColour.Yellow,"Значительное количество юрлиц, в уставном капитале которых есть доля текущего юрлица (учрежденные юрлица)",1,
                ()=>{return int.TryParse(GetParam("q7017"),out int count)&&count>10;}),
                new Marker("Значительное кол-во юр. лиц. руководителя (с учетом ИННФЛ)",MarkerColour.Yellow,"Значительное количество не ликвидированных юридических лиц, в которых в качестве действующего руководителя упомянут действующий руководитель текущей организации (с учетом ИННФЛ, если известен)",3,
                ()=>{return int.TryParse(GetParam("q7018"),out int count)&&count>10; }),
                new Marker("Значительное кол-во юр. лиц. руководителя (с учетом ФИО)",MarkerColour.Yellow,"Значительное количество не ликвидированных юридических лиц, в которых в качестве действующего руководителя упомянут действующий руководитель текущей организации (с учетом только ФИО)",1,
                ()=>{return int.TryParse(GetParam("q7019"),out int count)&&count>50; }),
                new Marker("Значительное кол-во бывших юр. лиц. руководителя (с учетом ИННФЛ)",MarkerColour.Yellow,"Значительное количество юридических лиц, в которых в качестве бывшего руководителя упомянут действующий руководитель текущей организации (с учетом ИННФЛ, если известен)",1,
                ()=>{return int.TryParse(GetParam("q7018"),out int count)&&int.TryParse(GetParam("q7020"),out int totalCount)&&(totalCount-count)>20; }),
                new Marker("Значительное кол-во бывших юр. лиц. руководителя (с учетом ФИО)",MarkerColour.Yellow,"Значительное количество юридических лиц, в которых в качестве бывшего руководителя упомянут действующий руководитель текущей организации (с учетом только ФИО)",1,
                ()=>{return int.TryParse(GetParam("q7019"),out int count)&&int.TryParse(GetParam("q7021"),out int totalCount)&&(totalCount-count)>80; }),
                new Marker("Компания терпит убытки",MarkerColour.Yellow,"Чистая прибыль на конец отчетного периода (за последний отчетный год, оценка в рублях)",3,
                ()=>{return int.TryParse(GetParam("s6008"),out int value) && value<0; }),
                new Marker("Наличие арбитражной практики",MarkerColour.Green,"Кол-во арбитражных дел за последние 12 месяцев в качестве ответчика или истца больше нуля",1,
                ()=>{return GetParam("q2001")!=""||GetParam("q2003")!=""; }),
                new Marker("Наличие бух. форм за предыдущий отчетный период",MarkerColour.Green,"Наличие бухгалтерских форм за предыдущий отчетный период",3,
                ()=>{return GetParam("m6002")!=""; }),
                new Marker("Наличие гос. контрактов",MarkerColour.Green,"Наличие государственных контрактов. Количество госконтрактов (44ФЗ и 223ФЗ), в которых организация участвует в качестве поставщика или заказчика (за 12 последних месяцев) больше нуля",5,
                ()=>{return GetParam("q4002")!=""||GetParam("q4004")!=""; }),
                new Marker("Наличие товарных знаков",MarkerColour.Green,"Наличие товарных знаков, действующих или недействующих, в которых упоминается текущая компания",5,
                ()=>{return GetParam("q9001")!=""; }),
                new Marker("Наличие филиалов или представительств",MarkerColour.Green,"Организация имеет филиалы или представительства",1,
                //TODO check
                ()=>{return GetMultiParam("Branches").Count()>0; }),
                new Marker("Уставный капитал более 100 000 руб.",MarkerColour.Green,"Уставный капитал более 100 000 руб.",3,
                ()=>{return int.TryParse(GetParam("Sum"),out int sum)&&sum>100000; }),         
                new Marker("Организация зарегистрирована более 5 лет тому назад",MarkerColour.Green,"Организация зарегистрирована более 5 лет тому назад",4,
                () => DateTime.TryParse(GetParam("regDate"),out var date) && (DateTime.Today - date).Days > 365*5+1),
                
                new Marker("Значительное количество компаний, найденных в особых реестрах ФНС",MarkerColour.YellowAffiliates,"Значительное количество компаний, найденных в особых реестрах ФНС",4,
                    () =>
                    {
                        //TODO Rename
                        var zp = GetMultiParam2("m5002Affiliates");
                        var na = GetMultiParam2("m5004Affiliates");
                        var kp = GetMultiParam2("m5006Affiliates");
                        var zi = GetMultiParam2("m5007Affiliates");
                        var count = .0;
                        for(int i=0;i<zp.Length;i++)
                            if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                                count++;

                        return count / zp.Length > 0.3;
                    }),
                
                new Marker("Значительное количество компаний, по которым требуется дополнительная проверка",MarkerColour.YellowAffiliates,"Значительное количество компаний, по которым требуется дополнительная проверка",5,
                    () =>
                    {
                        var zi = GetMultiParam2("m7001Affiliates");
                            //TODO Rename
                        var count = .0;
                        for(int i=0;i<zi.Length;i++)
                            if (zi[i] == "true")
                                count++;
                        return count / zi.Length > 0.3;
                    }),
                
                new Marker("Значительное количество компаний, зарегистрированных менее 12 месяцев назад",MarkerColour.YellowAffiliates,"Значительное количество компаний, зарегистрированных менее 12 месяцев назад",5,
                    () =>
                    {
                        var zi = GetMultiParam2("m7001Affiliates");
                        //TODO Rename
                        var count = .0;
                        for(int i=0;i<zi.Length;i++)
                            if (zi[i] == "true")
                                count++;

                        return count / zi.Length > 0.3;
                    }),
                
                new Marker("Значительное количество компаний, у которых за постлю 12 мес. хоья бы раз менялся деректор или учередитель",MarkerColour.YellowAffiliates,"Значительное количество компаний, у которых за постлю 12 мес. хоья бы раз менялся деректор или учередитель",2,
                    () =>
                    {
                        return false;//TODO finish    
                        //TODO Rename
                        var zp = GetMultiParam2("m5002Affiliates");
                        var na = GetMultiParam2("m5004Affiliates");
                        var kp = GetMultiParam2("m5006Affiliates");
                        var zi = GetMultiParam2("m5007Affiliates");
                        var kv = GetMultiParam2("m5006Affiliates");
                        var zt = GetMultiParam2("m5007Affiliates");

                        var count = .0;
                        for(int i=0;i<zp.Length;i++)
                            if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true" )
                                count++;

                        return count / zp.Length > 0.3;
                    }),
                
                new Marker("У группы компаний замечена активность в арбитражных делах",MarkerColour.GreenAffiliates,"Более 30% связаных компаний имеют арбитражную практику",1,
                    () =>
                    {
                        var q1 = GetMultiParam2("q2001Affiliates");
                        var q3 = GetMultiParam2("q2003Affiliates");
                        
                        var count = .0;
                        for(int i=0;i<q1.Length;i++)
                            if (q1[i]!=""||q3[i]!="")
                                count++;

                        return count / q1.Length > 0.3;
                    }),
                
                new Marker("У более чем половины связанных компаний найдена бухгалтерская отчетность",MarkerColour.GreenAffiliates,"Более чем 50% связанных компаний обнаружено наличие бухгалтерской отчетности за предыдущий период",4,
                    () =>
                    {
                        var m2 = GetMultiParam2("m6002Affiliates");
                        return m2.Count(x => x != "") / (double)m2.Length > 0.5;
                    }),
                
                new Marker("У группы компаний замечена активность в государственных торгах",MarkerColour.GreenAffiliates,"Более 20% связанных компаний имеют государственные контракты как заказчик или поставщик",5,
                    () =>
                    {
                        var q1 = GetMultiParam2("q2001Affiliates");
                        var q3 = GetMultiParam2("q2003Affiliates");
                        
                        var count = .0;
                        for(int i=0;i<q1.Length;i++)
                            if (q1[i]!=""||q3[i]!="")
                                count++;

                        return count / q1.Length > 0.2;
                    }),
                
                new Marker("Значительная часть организаций из группы компаний существуют более 5 лет", MarkerColour.GreenAffiliates,"Более 30% связанных организаций зарегистрированы более 5 лет тому назад",4,
                    () =>
                    {
                        var dates = GetMultiParam2("regDateAffiliates");
                        return dates.Count(x =>
                                DateTime.TryParse(x, out var date) && (DateTime.Today - date).Days > 365 * 5 + 1) /
                            dates.Length > 0.3;
                    })
        };
            
            
            markersList.Add(new Marker("Значительно снизилась выручка", MarkerColour.Yellow, "Выручка снизилась более чем на 30%", 3,
                    () => {
                        if (!(markersList.Where(x => x.Name == "Существенное снижение выручки").First().Check()))
                        {
                            if (DoubleTryParse(GetParam("s6004"), out double revenu) && DoubleTryParse(GetParam("s6003"), out double revenuPast))
                                return revenu < revenuPast * 0.7;
                        }
                        return false;
                    }));
            
            markersList.Add(new Marker("Значительное число ликвидированных связанных компаний", MarkerColour.YellowAffiliates, "Значительное число связных компаний, которые были ликвидированы в результате банкротства",3,  
                    () => {
                        if (!markersList.Where(x => x.Name == "Статус компании связан с произошедшей или планируемой ликвидацией").First().Check())
                        {
                            int Affiliatescount = GetMultiParam("InnAffilalates").Count();
                            int q7005Count = GetMultiParam("q7005Affiliates").Count();
                            return q7005Count > 5 && q7005Count > Affiliatescount * 0.2; 
                        }
                        return false;
                    }));
            
            markersList.Add(new Marker("Значительное число юр.лиц по этому адресу", MarkerColour.Yellow, "Значительное количество юридических лиц на текущий момент времени", 2,
                    () =>
                    {
                        if (!markersList.Where(x => x.Name == "Статус компании связан с произошедшей или планируемой ликвидацией").First().Check())
                        {
                            if (int.TryParse(GetParam("q7006"), out int count1) && count1 > 10)//КоличествоНеЛиквидированныхСУчетомНомераОфиса
                                return true;
                            if (int.TryParse(GetParam("q7007"), out int count2) && count2 > 50)//КоличествоНеЛиквидированныхБезУчетаНомераОфиса
                                return true;
                        }
                        return false;
                    }));
            
            markersList.Add(
                new Marker("Выручка по группе компаний снизилась более, чем на 30%",MarkerColour.YellowAffiliates,"Выручка по группе компаний снизилась более, чем на 30%",3,
                    ()=>
                    {
                        if (markersList.First(x => x.Name == "Выручка по группе компаний снизилась более, чем на 50%").Check())
                            return false;
                        double s6004;
                        s6004 = GetMultiParam("s6004Affiliates").Select(x=>x.Replace('.',',')).Sum(x=>double.Parse(x));
                        double s6003;
                        s6003 = GetMultiParam("s6003Affiliates").Select(x=>x.Replace('.',',')).Sum(x=>double.Parse(x));
                        return s6004 < 0.3 * s6003;
                    }));
            
            markersList.Add(
                new Marker("Значительная сумма исполнительных производств по группе компаний",MarkerColour.YellowAffiliates,"Значительная сумма исполнительных производств по группе компаний",3,
                    () =>
                    {
                        if (markersList.Find(x => x.Name == "Критическая сумма исполнительных производств по группе компаний").Check())
                            return false;
                        
                        
                        var revs = GetMultiParam2("s6004Affiliates");
                        var cases = GetMultiParam2("s1001Affiliates");
                        var sums = GetMultiParam2("SumAffiliates");
                        
                        var count = .0;
                        for(int i=0;i<sums.Length;i++)
                            if(DoubleTryParse(sums[i],out double sum) && 
                               DoubleTryParse(cases[i],out double a) && 
                               DoubleTryParse(revs[i],out double b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;

                    }));
            
            markersList.Add(new Marker("Значительная сумма арбитражных дел по группе компаний",MarkerColour.YellowAffiliates,"У болле чем 30% связанных организаций сработал маркер критическая сумма арбитражных дел",5,
                    () =>
                    {
                        if (markersList.Find(x => x.Name == "Критическая сумма арбитражных дел по группе компаний").Check())
                            return false;
                        
                        var casesIst = GetMultiParam2("s2003Affiliates");
                        var sums = GetMultiParam2("SumAffiliates");
                        var revs = GetMultiParam2("s6004Affiliates");
                        var casesOtv = GetMultiParam2("s2001Affiliates");

                        var count = .0; 
                        for (int i = 0; i < sums.Length;i++)
                            if(DoubleTryParse(revs[i],out double rev) && 
                               DoubleTryParse(sums[i],out double sum) &&
                               ((DoubleTryParse(casesIst[i], out double caseIst) &&
                                 (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                                (DoubleTryParse(casesOtv[i], out double caseOtv) &&
                                 (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                                count += 1;

                        return count / sums.Length > 0.3;
                    }));
            
            markersList.Add(new Marker("Значительнрое число компаний с особыми исполниельными производствами",MarkerColour.YellowAffiliates,"Более чем 30% связаных компаний имеют исполнительные производства предметом которых являются зарплата,наложение ареста, кредитные платежи, обращение взыскания на заложенное иммущество.",3,
                () =>
                {
                    var zp = GetMultiParam2("s1003Affiliates");
                    var na = GetMultiParam2("s1004Affiliates");
                    var kp = GetMultiParam2("s1005Affiliates");
                    var zi = GetMultiParam2("s1006Affiliates");
                    var count = .0;
                    for(int i=0;i<zp.Length;i++)
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
