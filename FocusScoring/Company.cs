using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

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
            {"ArrayOfreq/req" ,(ApiMethod.companyAffiliatesreq,"/ArrayOfreq/req")}

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
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParams(method, inn, node).ToArray();
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

        private void InitMarkers()
        {
            var markersList = new[]
            {
                new Marker("CompanyStatus", "Статус компании связан с",
                    () => GetParam("Dissolving") == "True" || GetParam("Dissolved") == "True"),
            };
            
            
            markers = new Dictionary<string, Marker>()
        {
            #region redflag
                        {"CompanyStatus" , ()=> {
                        return (GetParam("Dissolving") == "True" || GetParam("Dissolved") == "True");
                        } },
                        {"BankruptcyStatus", ()=>{
                        return (GetParam("m7013") == "True" || GetParam("m7014") == "True" || GetParam("m7016") == "True");
                        } },
                        {"func4" , ()=> {
                        double sum = double.Parse(GetParam("Sum"));
                        double a = double.Parse(GetParam("s1001"));
                        double b =  double.Parse(GetParam("s6004"));
                        return a > (0.2 * b) & (a > sum) & a > 100000;
                            //Критическая сумма исполнительных производств 
                            //(сумма исполнительных производств составляет более 20% от выручки организации за последний отчетный период) 
                            //и более суммы уставного капитала, и более 100 тыс. руб.
                        } },
                        {"func5", ()=> {
                        double sumDel = double.Parse(GetParam("s2003"));
                        double sumDelPast = double.Parse(GetParam("s2004"));
                        return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                            //Критический рост суммы арбитражных дел в качестве истца за последние 12 месяцев.
                            //Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года.
                            //При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.
                        } },
                        {"func6", ()=>{
                        double sumDel = double.Parse(GetParam("s2001"));
                        double sumDelPast = double.Parse(GetParam("s2002"));
                        return (sumDelPast > sumDel) & (sumDel > (sumDelPast - sumDel) / 2) & (sumDel > 5000000);
                            //Критический рост суммы арбитражных дел в качестве ответчика за последние 12 месяцев. 
                            //Т.е. сумма дел за последние 12 месяцев составляет более 50% по сравнению с суммой дел за предыдущие 2 года.
                            //При этом сумма арбитражных дел за последние 12 месяцев более 5 млн. руб.
                        } },
                        {"func7" , ()=> {
                        double sumDel = double.Parse(GetParam("s2003"));
                        double revenue = double.Parse(GetParam("s6004"));
                        double statedCapitalFocus = double.Parse(GetParam("Sum"));
                        return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                            //Критическая сумма арбитражных дел в качестве истца.
                            //Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период 
                            //и более суммы уставного капитала, и более 500 тыс. руб.
                        } },
                        {"func8" , ()=> {
                        double sumDel = double.Parse(GetParam("s2001"));
                        double revenue = double.Parse(GetParam("s6004"));
                        double statedCapitalFocus = double.Parse(GetParam("Sum"));
                        return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                            //Критическая сумма арбитражных дел в качестве истца.
                            //Т.е. сумма дел за последние 12 месяцев составляет более 20% от выручки организации за последний отчетный период 
                            //и более суммы уставного капитала, и более 500 тыс. руб.
                        } },
                        {"func9" , ()=> {
                        return (GetParam("m4001") == "True");
                        } },
                        {"func10" , ()=> {
                        return (GetParam("m5008") == "True");
                        } },
                        {"func11" , ()=> {
                        return long.Parse(GetParam("s6004"))<(0.5 * long.Parse(GetParam("6003")));
                        } },
                        {"func12" , ()=> {
                        return (GetParam("m7022") == "True");
                        } },
            //            {"func13" , ()=> {
            //            short affiliatesCount = 0;
            //            short badAffiliatesCount = 0;
            //            bool isBad= false;


            //            } },
            //            {"func14" , ()=> {

            //            } },
            //            {"funcn15" , ()=> {

            //            } },
            //            {"funcn16" , ()=> {

            //            } },
            //            {"func17" , ()=> {

            //            } },
#endregion
            //            {"func18" , ()=> {
            //            return (GetParam("Reorganizing") == "True");
            //            } },
            //            {"func19" , ()=> {
            //            return (GetParam("FoundersForeign") != "");
            //            } },
            //            {"func20" , ()=> {

            //            } },
            //            {"func21" , ()=> {
            //            double sum = GetParam("sum");
            //            double a = GetParam("s1001");
            //            double b = GetParam("s6004");
            //            return (a > (0.1 * b)) & (a > isum) & (a > 100000);

            //            } },
            //            {"func22" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } },
            //            {"funcname" , ()=> {

            //            } }


        };
        }
    }
}