using System;
using System.Collections.Generic;
using FocusAccess.Response;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    public enum ApiMethodEnum
    {
        req, contacts, egrDetails, analytics, licences, buh, companyAffiliatesanalytics, companyAffiliatesegrDetails, companyAffiliatesreq, stat,    
        sites, monList, briefReport,
        reqMon,
        reqUsage,
        egrMon
    }
    
    public static class ApiMethodEnumExtensions 
    {
        public static string Url(this ApiMethodEnum method)
        {
            return method switch
            {
                ApiMethodEnum.req => "req",
                ApiMethodEnum.analytics => "analytics",
                ApiMethodEnum.egrDetails => "egrDetails",
                ApiMethodEnum.contacts => "contacts",
                ApiMethodEnum.reqMon => "req/mon",
                ApiMethodEnum.egrMon => "egrDetails/mon",
                ApiMethodEnum.reqUsage => "req/expectedLimitUsage",
                ApiMethodEnum.licences => "licences", 
                ApiMethodEnum.companyAffiliatesreq => "companyAffiliates/req",
                ApiMethodEnum.companyAffiliatesanalytics => "companyAffiliates/analytics",
                ApiMethodEnum.companyAffiliatesegrDetails => "companyAffiliates/egrDetails",
                ApiMethodEnum.stat => "stat",
                _ => throw new NotImplementedException()
            };
        }

        public static bool DiscCache(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => true,
                ApiMethodEnum.analytics => true,
                ApiMethodEnum.egrDetails => true,
                ApiMethodEnum.contacts => true,
                ApiMethodEnum.licences => true, 
                ApiMethodEnum.companyAffiliatesreq => true,
                ApiMethodEnum.companyAffiliatesanalytics => true,
                ApiMethodEnum.companyAffiliatesegrDetails => true,
                ApiMethodEnum.reqMon => false,
                ApiMethodEnum.egrMon => false,
                ApiMethodEnum.reqUsage => false,
                ApiMethodEnum.stat => false,
                _ => throw new NotImplementedException()
            };

        public static bool GoodDbg(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => true,
                //ApiMethodEnum.analytics => true,
                ApiMethodEnum.egrDetails => true,
                //ApiMethodEnum.contacts => true,
                _ => false
            };

        public static string Alias(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.req => "req",
                ApiMethodEnum.analytics => "analytics",
                ApiMethodEnum.egrDetails => "egrDetails",
                ApiMethodEnum.contacts => "contacts",
                ApiMethodEnum.licences => "licences", 
                ApiMethodEnum.companyAffiliatesreq => "companyAffiliatesreq",
                ApiMethodEnum.companyAffiliatesanalytics => "companyAffiliatesanalytics",
                ApiMethodEnum.companyAffiliatesegrDetails => "companyAffiliatesegrDetails",
                ApiMethodEnum.reqMon => "reqMon",
                ApiMethodEnum.egrMon => "egrMon",
                ApiMethodEnum.reqUsage => "reqUsage",
                ApiMethodEnum.stat => "stat",
                _ => throw new NotImplementedException()
            };

        public static ApiMethodEnum OriginalMethodOf(this Type type) //TODO extenseion of type is not good
        {
            switch (type.Name)
            {
                case "ReqValue":
                    return ApiMethodEnum.req;
                case "AnalyticsValue":
                    return ApiMethodEnum.analytics;
                case "EgrDetailsValue":
                    return ApiMethodEnum.egrDetails;
                case "ContactsValue":
                    return ApiMethodEnum.contacts;
                case "ReqValue[]":
                    return ApiMethodEnum.companyAffiliatesreq;
                case "AnalyticsValue[]":
                    return ApiMethodEnum.companyAffiliatesanalytics;
                case "EgrDetailsValue[]":
                    return ApiMethodEnum.companyAffiliatesreq;
                case "LicencesValue":
                    return ApiMethodEnum.licences;
                default:
                {
                    throw new ArgumentException("Not type of method.");
                }
            }
        }
        
        public static bool IsMultiValue(this ApiMethodEnum method) =>
            method switch
            {
                ApiMethodEnum.companyAffiliatesreq => true,
                ApiMethodEnum.companyAffiliatesanalytics => true,
                ApiMethodEnum.companyAffiliatesegrDetails => true,
                _ => false
            };

        public static Type ValueType(this ApiMethodEnum method)
        {
            return method switch
            {
                ApiMethodEnum.req => typeof(IList<ReqValue>),
                ApiMethodEnum.analytics =>typeof(IList<AnalyticsValue>),
                ApiMethodEnum.egrDetails => typeof(IList<EgrDetailsValue>),
                ApiMethodEnum.contacts => typeof(IList<ContactsValue>),
                ApiMethodEnum.licences => typeof(IList<LicencesValue>), 
                ApiMethodEnum.companyAffiliatesreq => typeof(IList<ReqValue>),
                ApiMethodEnum.companyAffiliatesanalytics => typeof(IList<AnalyticsValue>),
                ApiMethodEnum.companyAffiliatesegrDetails => typeof(IList<EgrDetailsValue>),
                ApiMethodEnum.reqMon => typeof(IList<MonValue>),
                ApiMethodEnum.egrMon => typeof(IList<MonValue>),
                ApiMethodEnum.reqUsage => typeof(IList<MonValue>),
                ApiMethodEnum.stat => typeof(IList<StatValue>),
                _ => throw new NotImplementedException()
            };
        }
    }
}