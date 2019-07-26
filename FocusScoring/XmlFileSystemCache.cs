using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace FocusScoring
{
    internal class XmlFileSystemCache : IXmlCache
    {
        private readonly string cacheFolder;

        public XmlFileSystemCache(string cacheFolder = null)
        {
            this.cacheFolder = cacheFolder ?? Settings.XMLCacheFolder;
            
            if (!Directory.Exists(this.cacheFolder))
                Directory.CreateDirectory(this.cacheFolder);
        }

        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            document = new XmlDocument();
            var path = $"{cacheFolder}/{inn}.{method}";
            if (!File.Exists(path)) return false;
            document.Load(path);
            return true;
        }  //TODO cache fluency

        public void Update(string inn, ApiMethod method, XmlDocument doc)
        {
            doc.Save($"{cacheFolder}/{inn}.{method}");
        }

        public void Clear(string inn, ApiMethod method)
        {
            File.Delete($"{cacheFolder}/{inn}.{method}");
        }
    }
}