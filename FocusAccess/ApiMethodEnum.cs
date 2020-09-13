using System;
using System.Collections.Generic;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    public enum ApiMethodEnum
    {
        req, contacts, egrDetails, analytics, licences, buh, companyAffiliatesanalytics, companyAffiliatesegrDetails, companyAffiliatesreq, stat,    
        sites, monList, briefReport,
    }
    
    public static class ApiMethodEnumExtensions 
    {
        public static string Url(this ApiMethodEnum method)
        {
            throw method switch
            {
                _ => new NotImplementedException()
            };
        }
        public static bool DiscCache(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => true,
                ApiMethodEnum.analytics => true,
                ApiMethodEnum.egrDetails => true,
                ApiMethodEnum.contacts => true,
                _ => throw new NotImplementedException()
            };

        public static bool GoodDbg(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => true,
                ApiMethodEnum.analytics => true,
                ApiMethodEnum.egrDetails => true,
                ApiMethodEnum.contacts => true,
                _ => false
            };

        public static string Alias(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => "req",
                ApiMethodEnum.analytics => "analytics",
                ApiMethodEnum.egrDetails => "egrDetails",
                ApiMethodEnum.contacts => "contacts",
                _ => throw new NotImplementedException()
            };

        public static Type ValueType(this ApiMethodEnum method)
        {
            return method switch
            {
                ApiMethodEnum.req => typeof(IList<ReqValue>),
                ApiMethodEnum.analytics =>typeof(IList<AnalyticsValue>),
                ApiMethodEnum.egrDetails => typeof(IList<EgrDetailsValue>),
                ApiMethodEnum.contacts => typeof(IList<ContactsValue>),
                _ => throw new NotImplementedException()
            };
        }
    }
}