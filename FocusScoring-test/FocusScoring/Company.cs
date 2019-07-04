using System;
using System.Collections.Generic;
using System.Xml;

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
            {"Heads",(ApiMethod.req,"/ArrayOfreq/req/UL/heads/head") },
            {"managementCompany",(ApiMethod.req,"/ArrayOfreq/req/UL/managementCompanies/managementCompany") },
            {"RegistrationDate",(ApiMethod.req,"/ArrayOfreq/req/UL/registrationDate") },
            {"Bankruptcy",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7013")},
            {"BankruptcyLast12m",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7014")},
            {"BankruptcyEndProc",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7016") },
            {"s1001",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s1001") },
            {"RevenueP",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6003") },
            {"Revenue",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6004") },
            {"s6008",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s6008") },
            {"Sum",(ApiMethod.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/statedCapital/sum") },//If s1001 > 0.2 * s6004 & s1001 > Sum & s1001 > 100000 Then 'da'
            {"SumDel",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2003") },
            {"SumDelPast",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2004") },

           // If sumDelPast > sumDel & sumDel > (sumDelPast - sumDel) / 2 & sumDel > 2000000

            {"SumDel12",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2001") },
            {"SumDelPast12",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/s2002") },
            //If sumDelPast > sumDel & sumDel > (sumDelPast - sumDel) / 2 & sumDel > 2000000

             //7If sumDel(s2003) > 0.2 * revenu & sumDel > 500000 & sumDel > sum
             //8If sumDel(s2001) > 0.2 * revenu & sumDel > 500000 & sumDel > sum
            {"Badfaith",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m4001") },
            {"Disqualified",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m5008") },
            {"IndividualBnkruptcy",(ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/m7022") },
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

        private ParamAccess access;

        private Company(ParamAccess paramAccess = null)
        {
            access = paramAccess ?? new ParamAccess(Settings.FocusKey);
        }

        public static Company CreateINN(string inn)
        {
            var c = new Company();
            c.inn = inn;
            return c;
        }

        public static Company CreateByOGRN(string ogrn)
        {
            throw new NotImplementedException();
        }

        public string GetParam(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParam(method, inn, node);
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
                return "хо" + " " + fio;
            return "";
        }

        //public string Status()
        //{

        //}
    } 
}