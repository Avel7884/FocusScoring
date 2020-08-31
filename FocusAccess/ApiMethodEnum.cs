using System;

namespace FocusAccess
{
    public enum ApiMethodEnum
    {
        req, contacts, egrDetails, analytics, licences, buh, companyAffiliatesanalytics, companyAffiliatesegrDetails, companyAffiliatesreq, stat,    
        sites, monList, briefReport,
    }
    
    static class ApiMethodEnumExtensions 
    {
        public static string Url(this ApiMethodEnum method)
        {
            throw method switch
            {
                _ => new NotImplementedException()
            };
        }
        public static bool DiscCache(this ApiMethodEnum method)
        {
            throw method switch
            {
                _ => new NotImplementedException()
            };
        }
        public static string Alias(this ApiMethodEnum method)
        {
            throw method switch
            {
                _ => new NotImplementedException()
            };
        }
    }
}