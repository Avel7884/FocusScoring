using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace FocusScoring
{
    internal class Program
    {
        private static string focuskey = "fuck";    

        static List<(ApiMethod,string)> companyParams = new List<(ApiMethod, string)>()
        {
            (ApiMethod.req,"/ArrayOfreq/req/UL/status/statusString"),
            //(ApiMethod.req,"/ArrayOfreq/req/UL/status/statusString"),
            //(ApiMethod.req,"/ArrayOfreq/req/IP/fio"),
            (ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7019"),
            (ApiMethod.req,"/ArrayOfreq/req/UL/status/statusString"),
            (ApiMethod.analytics,"/ArrayOfanalytics/analytics/analytics/q7018"),
            (ApiMethod.req,"/ArrayOfreq/req/UL/status/statusString"),

            
        };

        private static XmlCache cache;
        private static XmlDownload xmlDownload;

        public static void Main(string[] args)
        {
            var inns = Console.ReadLine().Split();
            WebResponse response;
            byte[] buf;

            cache = new XmlCache();
            xmlDownload = new XmlDownload(focuskey);

            foreach (var inn in inns)
            {
                foreach (var (method,node) in companyParams)
                    Console.WriteLine(GetParam(method,inn,node));
            }
        }

        public static string GetParam(ApiMethod method,string inn,string node)
        {
            var d = new XmlDocument();
            if (cache.TryGetXml(inn, method, out d))
                return d.SelectSingleNode(node).InnerText;

            if (xmlDownload.TryGetXml(inn, method, out d))
            {
                cache.WriteCache(inn, method, d);
                return d.SelectSingleNode(node).InnerText;
            }
            
            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }
    }
    
    
}