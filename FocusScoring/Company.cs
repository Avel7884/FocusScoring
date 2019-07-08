using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using FocusScoring;

namespace FocusScoring
{
    public class Company
    {
        private string inn;
        private Dictionary<string, (ApiMethod, string)> paramDict = new Dictionary<string, (ApiMethod, string)>()
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
            {"head",(ApiMethod.req,"/ArrayOfreq/req/UL/heads/head") },
            {"managementCompany",(ApiMethod.req,"/ArrayOfreq/req/UL/managementCompanies/managementCompany") },
            {"RegistrationDate",(ApiMethod.req,"/ArrayOfreq/req/UL/registrationDate") },
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
            {"FounderFL",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersFL/founderFL") },
            {"FoundersForeign",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersForeign/founderForeign") },
            {"m1003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1003") },
            {"m1004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1004") },
            {"m1005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1005") },
            {"m1006",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m1006") },
            {"q2003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2003") },
            {"q2004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2004") },
            {"q2001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2001") },
            {"q2002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q2002") },
            {"m5002",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5002") },
            {"m5003",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5003") },
            {"m5004",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5004") },
            {"m5005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5005") },
            {"m7001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7001") },
            {"q7005",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7005") },
            {"q7006",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7006") },
            {"q7007",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7007") },
            {"q7008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7008") },
            {"q7009",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7009") },
            {"q7017",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7017") },
            {"q7018",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7018") },
            {"q7020",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7020") },
            {"q7021",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7021") },  
        };
        
        private Dictionary<string, (ApiMethod, string,string)> multiParamDict = new Dictionary<string, (ApiMethod, string,string)>()
        {
            {"DissolvingAffiliates", (ApiMethod.companyAffiliatesreq, "/ArrayOfreq/req", "/ArrayOfreq/req/UL/status/dissolving")},
            {"DissolvedAffiliates", (ApiMethod.companyAffiliatesreq, "/ArrayOfreq/req", "/ArrayOfreq/req/UL/status/dissolved")},
            { "m7013Affiliates", (ApiMethod.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics", "/ArrayOfanalytics/analytics/analytics/m5003")},
        };

        private ParamAccess access;

        private Company(ParamAccess paramAccess = null)
        {
            access = ParamAccess.Start();
            InitMarkers();
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
            (ApiMethod method, string multiNode, string node) = multiParamDict[paramName];
            return access.GetParams(method, inn, multiNode, node).ToArray();
        }

        public bool GetMarker(string markerName)
        {
            return markers[markerName].Check();
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


        private Dictionary<string, Marker> markers;

        private Marker[] markersList;

        public Marker[] GetMarkers()
        {
            return markersList.Where(marker=>marker.Check()).ToArray();
        }
        
        private void InitMarkers()
        {
            markersList = new[]
            {
                new Marker("CompanyStatus", MarkerColour.Red,
                    "Статус компании связан с произошедшей или планируемой ликвидацией", 5,
                    () => GetParam("Dissolving") == "True" || GetParam("Dissolved") == "True"),
                new Marker("BankruptcyStatus", MarkerColour.Red,
                    "Статус компании связан с произошедшей или планируемой ликвидацией", 5,
                    () => GetParam("m7013") == "True" || GetParam("m7014") == "True" || GetParam("m7016") == "True"),
                new Marker("func4", MarkerColour.Red, "Критическая сумма исполнительных производств " +
                                                      "(сумма исполнительных производств составляет более 20% от выручки организации за последний отчетный период) " +
                                                      "и более суммы уставного капитала, и более 100 тыс. руб.", 4,
                    () =>
                    {
                        var sum = GetParam("Sum");
                        var a = GetParam("s1001");
                        var b = GetParam("s6004");
                        if (sum != "" & a != "" & b != "")
                            return double.Parse(a) > (0.2 * double.Parse(b)) & (double.Parse(a) > double.Parse(sum)) &
                                   double.Parse(a) > 100000;
                        return false;
                    }),

                new Marker("func5", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве истца за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    () =>
                    {
                        if (double.TryParse(GetParam("s2003"), out double sumDel) &&
                            double.TryParse(GetParam("s2004"), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),

                new Marker("func6", MarkerColour.Red,
                    "Критический рост суммы арбитражных дел в качестве ответчика за последние 12 месяцев. " +
                    "Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года. " +
                    "При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.", 1,
                    () =>
                    {
                        if (double.TryParse(GetParam("s2001"), out double sumDel) &&
                            double.TryParse(GetParam("s2002"), out double sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                    }),
                  new Marker("func8", MarkerColour.Red, "Критическая сумма арбитражных дел в качестве ответчика." +
                  "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период и более суммы уставного капитала, " +
                  "и более 500 тыс. руб.",1,
                  () => {
                        if(double.TryParse(GetParam("s2001"),out double sumDel) && double.TryParse(GetParam("s6004"),out double revenue) && double.TryParse(GetParam("Sum"),out double statedCapitalFocus))
                           return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                  }),
                new Marker("func7", MarkerColour.Red, "Критическая сумма арбитражных дел в качестве истца." +
                                                      "Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период" +
                                                      "и более суммы уставного капитала, и более 500 тыс. руб.", 1,
                    () =>
                    {
                        if (double.TryParse(GetParam("s2003"), out double sumDel) && double.TryParse(GetParam("s6004"),
                                                                                      out double revenue)
                                                                                  && double.TryParse(GetParam("Sum"),
                                                                                      out double statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                    }),

                new Marker("func9", MarkerColour.Red,
                    "Организация была найдена в реестре недобросовестных поставщиков" +
                    "(ФАС, Федеральное Казначейство)", 3,
                    () => { return GetParam("m4001") == "True"; }),

                new Marker("func10", MarkerColour.Red,
                    "ФИО руководителей или учредителей были найдены в реестре дисквалифицированных лиц (ФНС)", 4,
                    () => { return GetParam("m5008") == "True"; }),

                new Marker("func11", MarkerColour.Red, "Выручка снизилась более, чем на 50%", 1,
                    () =>
                    {
                        if (long.TryParse(GetParam("s6004"), out long s6004) &
                            long.TryParse(GetParam("s6003"), out long s6003))
                            return s6004 < (0.5 * s6003);
                        return false;
                    }),

                new Marker("func12", MarkerColour.Red, "Наличие сообщений о банкротстве физлица, " +
                                                       "являющегося руководителем (лицом с правом подписи без доверенности), " +
                                                       "учредителем, либо индивидуальным предпринимателем. Необходимо изучить сообщения",
                    3,
                    () => { return GetParam("m7022") == "True"; }),

                    new Marker("func13",MarkerColour.Red,"У более чем 50% связанных организаций статус связан с произошедшей или планируемой ликвидацией",4,
                    ()=>{
                    var Dissolved = GetMultiParam("DissolvedAffiliates");
                    var Dissolving = GetMultiParam("DissolvingAffiliates");
                    int affiliatesCount = Dissolving.Length;
                    var badAffiliatesCount = Dissolved.Zip(Dissolving, (x,y)=> x!="" || y!="").Aggregate(0,(i,x)=>i+(x?1:0));
                        return badAffiliatesCount/affiliatesCount * 100 > 50;
                    }),
                    new Marker("func14",MarkerColour.Red,"У более чем 50% связанных организаций присутствуют маркеры, свидетельствующие о вероятном банкротстве компаний",5,
                    ()=>{
                        var a = GetMultiParam("m7013Affiliates");
                        return false;
                        }),
         //         new Marker("",MarkerColour.Red,"",,
         //         ()=>{ }),
        };  
            markers = markersList.ToDictionary(x => x.Name);

        }
    }
}
