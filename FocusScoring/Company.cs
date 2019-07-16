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
            {"LegalAddress",(ApiMethod.req,"/ArrayOfreq/req/UL/history/legalAddress/firstDate") },
            {"LegalName",(ApiMethod.req,"/ArrayOfreq/req/UL/legalName/date") },
            {"kpp",(ApiMethod.req,"/ArrayOfreq/req/UL/history/kpps/date") },
            {"heads",(ApiMethod.req,"/ArrayOfreq/req/UL/history/heads/firstDate") },
            {"managementCompanies",(ApiMethod.req,"/ArrayOfreq/req/UL/history/managementCompanies/firstDate") },
        };

        public string Inn { get; set; }
        private ParamAccess access;

        private Company(ParamAccess paramAccess = null)
        {
            access = ParamAccess.Start();
        }

        public static Company CreateINN(string inn)
        {
            var c = new Company();
            c.Inn = inn;
            c.Score = -1;
            return c;
        }

        public int Score { get; set; }

        public MarkerResult[] Markers { get; set; }

        public string GetParam(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParam(method, Inn, node);
        }

        public string[] GetMultiParam(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetParams(method, Inn, node).ToArray();
        }
        //TODO Rename
        public string[] GetMultiParam2(string paramName)
        {
            (ApiMethod method, string node) = paramDict[paramName];
            return access.GetMultiParam(method, Inn, node).ToArray();
        }

        public string CompanyName()
        {
            string _short = GetParam("Short");
            string full = GetParam("Full");
            string fio = GetParam("FIO");
            if (_short != "")
            {
                _short = _short.Replace("Общество с ограниченной ответственностью", "ООО");
                _short = _short.Replace("Закрытое акционерное общество", "ЗАО");
                _short = _short.Replace("Акционерное общество", "АО");
                return _short;
            }
            if (full != "")
            {
                full = full.Replace("ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ", "ООО");
                full = full.Replace("Общество с ограниченной ответственностью", "ООО");
                full = full.Replace("Закрытое акционерное общество", "ЗАО");
                full = full.Replace("Акционерное общество", "АО");
                return full;
            }

            if (fio != "")
            {
                return "ИП" + " " + fio;
            }

            return "";
        }
    }
}
