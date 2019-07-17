using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FocusScoring
{
    internal class ParamAccess
    {
        private readonly List<IXmlCache> caches;
        private readonly IXmlAccess source;

        internal ParamAccess(List<IXmlCache> caches,IXmlAccess source)
        {
            this.caches = caches;
            this.source = source;
        }

        private bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            for (int i = 0; i < caches.Count; i++)
                if (caches[i].TryGetXml(inn, method, out document))
                {
                    for (int j = 0; j < i; j++)
                        caches[j].Update(inn, method, document);
                    return true;
                }

            if (!source.TryGetXml(inn, method, out document)) 
                return false;
            foreach (var cache in caches)
                cache.Update(inn, method, document);
            return true;
        }


        internal IEnumerable<string> GetParams(ApiMethod method, string inn, string node)
        {
            if (TryGetXml(inn, method, out var document))
            {
                foreach (XmlNode n in document.SelectNodes(node))
                    yield return n.InnerText;
                yield break;
            }
            yield return "Ошибка! Проверьте подключение к интернет и повторите попытку." ;            
        }

        internal IEnumerable<string> GetMultiParam(ApiMethod method, string inn, string node)
        {
            if (TryGetXml(inn, method, out var document))
                GetChild(document, node);
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
        
        public string GetParam(ApiMethod method, string inn, string node)
        {
            if(TryGetXml(inn,method,out var document))
                return document.SelectSingleNode(node)?.InnerText ?? "";
            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }
//
//        public IEnumerable<string> GetMultiParam(ApiMethod method, string inn, string node)
//        {
//            if (memoryCache.TryGetXml(inn, method, out var d))
//                return GetChild(d, node);
//
//            if (discCache.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn, method, d);
//                return GetChild(d, node);
//            }
//
//            if (download.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn, method, d);
//                discCache.Update(inn, method, d);
//                return GetChild(d, node);
//            }
//            return new[] { "Ошибка! Проверьте подключение к интернет и повторите попытку." };
//        }
//
//        public IEnumerable<string> GetParams(ApiMethod method, string inn, string node)
//        {
//            if (memoryCache.TryGetXml(inn, method, out var d))
//                return GetParams(d, node);
//
//            if (discCache.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn, method, d);
//                return GetParams(d, node);
//            }
//
//            if (download.TryGetXml(inn, method, out d))
//            {
//                memoryCache.Update(inn, method, d);
//                discCache.Update(inn, method, d);
//                return GetParams(d, node);
//            }
//            return new[] { "Ошибка! Проверьте подключение к интернет и повторите попытку." };
//
//        }

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