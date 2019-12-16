using System.Collections.Generic;
using System.Xml;

namespace FocusScoring
{
    internal class XmlAccess : IXmlAccess
    {
        private readonly List<IXmlCache> caches;
        private readonly IXmlAccess source;

        internal XmlAccess(List<IXmlCache> caches,IXmlAccess source)
        {
            this.caches = caches;
            this.source = source;
        }

        public bool TryGetXml(INN inn, ApiMethod method, out XmlDocument document)
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

        public void Clear(INN inn, ApiMethod method)
        {
            foreach (var cache in caches)
                cache.Clear(inn,method);
        }
    }
}