using System;
using System.Collections.Generic;
using System.Xml;

namespace FocusScoring
{
    public class Company
    {
        private string inn;
        
        private Dictionary<string,(ApiMethod,string)> paramDict = new Dictionary<string, (ApiMethod, string)>()
        {
            {"Dissolving", (ApiMethod.req,"/ArrayOfreq/req/UL/status/dissolving")}
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
    }
}