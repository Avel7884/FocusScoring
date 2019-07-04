using System.Xml;

namespace FocusScoring
{
    internal class ParamAccess 
    {
        private readonly XmlCache cache;
        private readonly XmlDownload download;

        public ParamAccess(string focusKey)
        {
            this.cache = new XmlCache();
            this.download = new XmlDownload(focusKey);
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
    }
}