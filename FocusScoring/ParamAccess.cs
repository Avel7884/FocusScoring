using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FocusScoring
{
    internal class ParamAccess 
    {
        private readonly XmlCache cache;
        private readonly XmlDownload download;
        private static ParamAccess paramAccess;

        public static ParamAccess Start()
        {
            return paramAccess ?? (paramAccess = new ParamAccess());
        }
        
        private ParamAccess()
        {
            this.cache = new XmlCache();
            this.download = new XmlDownload(Settings.FocusKey);
        }
        
        public string GetParam(ApiMethod method,string inn,string node)
        {
            var d = new XmlDocument();
            if (cache.TryGetXml(inn, method, out d))
                return d.SelectSingleNode(node)?.InnerText ?? "";

            if (download.TryGetXml(inn, method, out d))
            {
                cache.WriteCache(inn, method, d);
                return d.SelectSingleNode(node)?.InnerText ?? "";
            }
            
            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }

        public IEnumerable<string> GetParams(ApiMethod method, string inn, string node)
        {
            var d = new XmlDocument();
            XmlNodeList nodes = null; 
            if (cache.TryGetXml(inn, method, out d))
                nodes = d.SelectNodes(node);

            if (download.TryGetXml(inn, method, out d))
            {
                cache.WriteCache(inn, method, d);
                nodes = d.SelectNodes(node);
            }

            if (nodes == null)
            {
                yield return  "Ошибка! Проверьте подключение к интернет и повторите попытку.";
                yield break;                
            }

            foreach (XmlNode n in nodes)
                yield return n.InnerText;

        }
    }
}