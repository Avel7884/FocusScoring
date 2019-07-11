using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FocusScoring
{
    internal class ParamAccess 
    {
        private readonly XmlDiscCache discCache;
        private readonly XmlDownload download;
        private static ParamAccess paramAccess;
        private readonly SingleXmlMemoryCache memoryCache;

        public static ParamAccess Start()
        {
            return paramAccess ?? (paramAccess = new ParamAccess());
        }
        
        private ParamAccess()
        {
            this.discCache = new XmlDiscCache();
            this.download = new XmlDownload(Settings.FocusKey);
            memoryCache = new SingleXmlMemoryCache();
        }
         
        //TODO DRY
        public string GetParam(ApiMethod method,string inn,string node)
        {
            if (memoryCache.TryGetXml(inn, method, out var d))
                return d.SelectSingleNode(node)?.InnerText ?? "";

            if (discCache.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                return d.SelectSingleNode(node)?.InnerText ?? "";
            }

            if (download.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                discCache.Update(inn, method, d);
                return d.SelectSingleNode(node)?.InnerText ?? "";
            }

            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }

<<<<<<< HEAD
        public IEnumerable<string> GetMultiParam(ApiMethod method, string inn, string node)
=======
        public IEnumerable<string> GetMultiParam(ApiMethod method,string inn,string node)
>>>>>>> 14deb810c328f87fa6e1d6fa30bb348d5d6eaa24
        {
            if (memoryCache.TryGetXml(inn, method, out var d))
                return GetChild(d, node);

            if (discCache.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                return GetChild(d, node);
            }

            if (download.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                discCache.Update(inn, method, d);
                return GetChild(d, node);
            }
            return  new []{"Ошибка! Проверьте подключение к интернет и повторите попытку."};                
        }

<<<<<<< HEAD
        private IEnumerable<string> GetChild(XmlDocument document, string node)
        {    //TODO naming
            var heres = node.Split(new[] { '/' }, 4);
            var adr1 = '/' + heres[1] + '/' + heres[2];

            foreach (XmlNode n in document.SelectNodes(adr1))
            { 
                var nodes = n.SelectNodes(heres[3]);
            if (nodes.Count > 0)
                yield return nodes.Item(0).InnerText;
            else
                yield return "";
=======
        private IEnumerable<string> GetChild(XmlDocument document,string node)
        {
            var heres = node.Split(new[] {'/'}, 4);
            var adr1 = '/' + heres[1] + '/' + heres[2];

            foreach (XmlNode n in document.SelectNodes(adr1))
            {
                var nodes = n.SelectNodes(heres[3]);
                if (nodes.Count > 0)
                    yield return nodes.Item(0).InnerText;
                else
                    yield return "";
>>>>>>> 14deb810c328f87fa6e1d6fa30bb348d5d6eaa24
            }
        }

        public IEnumerable<string> GetParams(ApiMethod method, string inn, string node)
        {
            if(memoryCache.TryGetXml(inn, method, out var d))
                return GetParams(d, node);

            if (discCache.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                return GetParams(d, node);
            }

            if (download.TryGetXml(inn, method, out d))
            {
                memoryCache.Update(inn,method,d);
                discCache.Update(inn, method, d);
                return GetParams(d, node);
            }
            return  new []{"Ошибка! Проверьте подключение к интернет и повторите попытку."};                

        }
        
        public IEnumerable<string> GetParams(XmlDocument document, string node)
        {
            foreach (XmlNode n in document.SelectNodes(node))
                yield return n.InnerText;
        }
        
//        
//        //TODO dry
//
//        public IEnumerable<string> GetParams(ApiMethod method, string inn, string multiNode,string node)
//        {
//            if(memoryCache.TryGetXml(inn, method, out  var d))
//                return GetParams(d, multiNode, node);
//
//            if (discCache.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn,method,d);
//                return GetParams(d, multiNode, node);
//            }
//
//            if (download.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn,method,d);
//                discCache.Update(inn, method, d);
//                return GetParams(d, multiNode, node);
//            }
//
//            return  new []{"Ошибка! Проверьте подключение к интернет и повторите попытку."};                
//        }
//
//        public IEnumerable<string> GetParams(XmlDocument document, string multiNode,string node)
//        {
//            foreach (XmlNode n in document.SelectNodes(multiNode))
//                yield return n.SelectSingleNode(node)?.InnerText ?? "";
//        }

    }
}