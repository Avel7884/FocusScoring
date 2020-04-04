/*
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using FocusApiAccess.Methods;

namespace FocusApiAccess
{
    public class ParameterProvider// : IParameterProvider
    {
        private XmlAccess access;

        public ParameterProvider(XmlAccess access)
        {
            this.access = access;
        }

        //public Dictionary<string, ApiParameter> ParametersByName=>paramDict;


        public T GetParameter<T>(FullUrl<T> url) where T : IParameterValue
        {
            url.Parameter.
            if (access.TryGetXml(url.Method, url.Query, out var document))
                return  document.SelectNodes(url.Parameter.Path)
                    .Cast<XmlNode>();
        }
            */

        /*

        public T GetParam<T>(ApiParameter<T> param, IQueryComponents args)
        {            
            if (access.TryGetXml(param.Method,args, out var document))
            {
                //TODO Kostyl!!!
                var result = document.SelectSingleNode(param.Path)?.InnerText;
                if (result != null)
                    return result;
                if(param.Method != methods[1])//req
                    return "";
                return document.SelectSingleNode(param.Path.Replace("UL","IP"))?.InnerText ?? "";
            }
            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }
        
        public T GetParams<T>(ApiParameter<T> param, IQueryComponents args)
        {
            if (access.TryGetXml(param.Method, args, out var document))
                return document.SelectNodes(param.Path).Cast<XmlNode>().Select(n => n.InnerText).ToArray();

            return new[] {"Ошибка! Проверьте подключение к интернет и повторите попытку."};
        }

        public T GetMultiParam<T>(ApiParameter<T> param, IQueryComponents args)
        {
            if (access.TryGetXml(param.Method, args, out var document))
                return GetChild(document, param.Path).ToArray();
            return new[] {"Ошибка! Проверьте подключение к интернет и повторите попытку."};
        }

        private IEnumerable<string> GetChild(XmlDocument document, string node)
        {    
            var splitedNode = node.Split(new[] { '/' }, 4);
            var parent = '/' + splitedNode[1] + '/' + splitedNode[2];

            foreach (XmlNode child in document.SelectNodes(parent))
            { 
                var nodes = child.SelectNodes(splitedNode[3]);
                if (nodes.Count > 0)
                    yield return nodes.Item(0).InnerText;
                else
                    yield return "";
            }
        }

        internal static List<ApiMethod> methods = new List<ApiMethod>
        {
            new ApiMethod("analytics"),
            new ApiMethod("req"),
            new ApiMethod("buh"),
            new ApiMethod("egrDetails"),
            new ApiMethod("contacts"),
            new ApiMethod("licences"),
            new ApiMethod("companyAffiliates/analytics",stackable:false),
            new ApiMethod("companyAffiliates/egrDetails",stackable:false),
            new ApiMethod("companyAffiliates/req",stackable:false),
            new ApiMethod("briefReport"),
            new ApiMethod("taxes"),
            new ApiMethod("sites"),
            new ApiMethod("excerpt"),
            new ApiMethod("finan"),
            new ApiMethod("fnsBlockedBankAccounts"),
            new ApiMethod("monList",false),
            new ApiMethod("grDetails/mon",false),
            new ApiMethod("req/mon",false),
            new ApiMethod("fizBankr"),
            new ApiMethod("personBankruptcy"),
            new ApiMethod("checkPassport"),
            new ApiMethod("reqBase"),
            new ApiMethod("fssp"),
            new ApiMethod("fsa"),
            new ApiMethod("stat",false),
            new ApiMethod("sites")
        }; 
            //TODO there must be a better way...
        internal static Dictionary<string, (ApiMethodEnum, string)> paramTupDict = new Dictionary<string, (ApiMethodEnum, string)>()
        {
            {"ReportRed",(ApiMethodEnum.briefReport,"/ArrayOfbriefReport/briefReport/briefReport/summary/redStatements") },
            {"ReportYellow",(ApiMethodEnum.briefReport,"/ArrayOfbriefReport/briefReport/briefReport/summary/yellowStatements") },
            {"ReportGreen",(ApiMethodEnum.briefReport,"/ArrayOfbriefReport/briefReport/briefReport/summary/greenStatements") },
            {"Report",(ApiMethodEnum.briefReport,"/ArrayOfbriefReport/briefReport/briefReport/href") },
            {"Short",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/legalName/short") },
            {"Full",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/legalName/full") },
            {"FIO",(ApiMethodEnum.req,"/ArrayOfreq/req/IP/fio") },
            {"Activities", (ApiMethodEnum.egrDetails, "/ArrayOfegrDetails/egrDetails/UL/activities/principalActivity/text")},
            {"Dissolving", (ApiMethodEnum.req,"/ArrayOfreq/req/UL/status/dissolving")},
            {"Dissolved", (ApiMethodEnum.req,"/ArrayOfreq/req/UL/status/dissolved")},
            {"Status", (ApiMethodEnum.req,"/ArrayOfreq/req/UL/status/statusString")},
            {"phone",(ApiMethodEnum.contacts, "/ArrayOfcontacts/contacts/contactPhones/phones/string")},
            {"site", (ApiMethodEnum.sites, "/ArrayOfsites/sites/sites/string")},
            {"legalAddress",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/history/legalAddresses/legalAddress") },
            {"legalName",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/history/legalNames/legalName") },
            {"kppWithDate",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/history/kpps/kppWithDate") },
            {"Reorganizing",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/status/reorganizing") },
            {"head",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/heads/head/innfl") },
            {"headName",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/heads/head/fio")},
            {"regDate",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/registrationDate") },
            {"managementCompany",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/managementCompanies/managementCompany") },
            {"RegistrationDate",(ApiMethodEnum.req, "/ArrayOfreq/req/UL/registrationDate") },
            {"Branches",(ApiMethodEnum.req, "ArrayOfreq/req/UL/branches/name") },
            {"m7013",(ApiMethodEnum.analytics, "/ArrayOfanalytics/analytics/analytics/m7013")},
            {"m7014",(ApiMethodEnum.analytics, "/ArrayOfanalytics/analytics/analytics/m7014")},
            {"m7016",(ApiMethodEnum.analytics, "/ArrayOfanalytics/analytics/analytics/m7016") },
            {"s1001",(ApiMethodEnum.analytics, "/ArrayOfanalytics/analytics/analytics/s1001") },
            {"s6003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s6003") },
            {"s6004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s6004") },
            {"s6008",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s6008") },
            {"Sum",(ApiMethodEnum.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/statedCapital/sum") },
            {"s2003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s2003") },
            {"s2004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s2004") },
            {"s2001",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s2001") },
            {"s2002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s2002") },
            {"m4001",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m4001") },
            {"m5008",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5008") },
            {"m7022",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m7022") },
            {"FounderFL",(ApiMethodEnum.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersFL/founderFL/innfl") },
            {"FoundersForeign",(ApiMethodEnum.egrDetails,"/ArrayOfegrDetails/egrDetails/UL/foundersForeign/founderForeign") },
            {"m1003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m1003") },
            {"m1004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m1004") },
            {"m1005",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m1005") },
            {"m1006",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m1006") },
            {"s1007",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s1007") },
            {"s1008",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/s1008") },
            {"q2003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q2003") },
            {"q2004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q2004") },
            {"q2001",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q2001") },
            {"q2002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q2002") },
            {"m5002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5002") },
            {"m5003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5003") },
            {"m5004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5004") },
            {"m5005",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5005") },
            {"m5006",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5006") },
            {"m5007",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m5007") },
            {"m6002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m6002") },
            {"m7001",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m7001") },
            {"m7002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m7002") },
            {"m7003",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m7003") },
            {"m7004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/m7004") },
            {"q4002",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q4002") },
            {"q4004",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q4004") },
            {"q7005",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7005") },
            {"q7006",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7006") },
            {"q7007",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7007") },
            {"q7008",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7008") },
            {"q7009",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7009") },
            {"q7017",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7017") },
            {"q7018",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7018") },
            {"q7019",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7019") },
            {"q7020",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7020") },
            {"q7021",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q7021") },
            {"q9001",(ApiMethodEnum.analytics,"/ArrayOfanalytics/analytics/analytics/q9001") },
            {"DissolvingAffiliates", (ApiMethodEnum.companyAffiliatesreq, "/ArrayOfreq/req/UL/status/dissolving")},
            {"DissolvedAffiliates", (ApiMethodEnum.companyAffiliatesreq, "/ArrayOfreq/req/UL/status/dissolved")},
            {"InnAffilalates",(ApiMethodEnum.companyAffiliatesreq,"/ArrayOfreq/req/inn")},
            {"m7013Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7013")},
            {"m7014Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7014")},
            {"m7016Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/m7016")},
            {"s6003Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/s6003")},
            {"s6004Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/s6004")},
            {"q7005Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q7005")},
            {"q4002Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q4002")},
            {"q4004Affiliates", (ApiMethodEnum.companyAffiliatesanalytics,"/ArrayOfanalytics/analytics/analytics/q4004")},
            {"SumAffiliates", (ApiMethodEnum.companyAffiliatesegrDetails, "/ArrayOfegrDetails/egrDetails/UL/statedCapital/sum")},
            {"s1001Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s1001")},
            {"s2003Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s2003")},
            {"s2001Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/s2001")},
            {"s1003Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1003")},
            {"s1004Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1004")},
            {"s1005Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1005")},
            {"s1006Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m1006")},
            {"m5002Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5002")},
            {"m5004Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5004")},
            {"m5006Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5006")},
            {"m5007Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m5007")},
            {"m7001Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m7001")},
            {"m7004Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m7004")},
            {"q2003Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/q2003")},
            {"q2001Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/q2001")},
            {"m6002Affiliates", (ApiMethodEnum.companyAffiliatesanalytics, "/ArrayOfanalytics/analytics/analytics/m6002")},
            {"regDateAffiliates", (ApiMethodEnum.companyAffiliatesreq, "/ArrayOfreq/req/UL/registrationDate")},
            {"LegalAddress",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/history/legalAddress/firstDate") },
            {"LegalName",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/legalName/date") },
            {"kpp",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/history/kpps/date") },
            {"heads",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/history/heads/firstDate") },
            {"managementCompanies",(ApiMethodEnum.req,"/ArrayOfreq/req/UL/history/managementCompanies/firstDate") },
        };

        private static Dictionary<string, ApiMethod> methDict = methods.ToDictionary(x => x.Name);
        
        private static Dictionary<string, ApiParameter> paramDict = paramTupDict
            .Select(x => new ApiParameter(x.Key, "", methDict[x.Value.Item1.ToString()], x.Value.Item2))
            .ToDictionary(x => x.Name);
        static ParameterProvider()
        {
            /*var dict = new Dictionary<ApiMethod,List<ApiParameter>>();
            foreach (var parameter in paramDict.Values)
                if(dict.TryGetValue(parameter.Method,out var list))
                    list.Add(parameter);
                else dict[parameter.Method] = new List<ApiParameter>{parameter};
            foreach (var pair in dict)
                pair.Key.Parameters = pair.Value.ToArray();
        }
    }
}*/
