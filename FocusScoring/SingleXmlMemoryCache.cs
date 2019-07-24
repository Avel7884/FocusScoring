using System;
using System.Xml;

namespace FocusScoring
{
    internal class SingleXmlMemoryCache : IXmlCache
    {
        private string Inn = string.Empty;
        private ApiMethod method;
        private bool cleared = false; 
        private XmlDocument document;

        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            if (!cleared && Inn == inn && method == this.method)
            {
                document = this.document;
                return true;
            }

            document = new XmlDocument();
            return false;
        }

        public void Update(string inn, ApiMethod method, XmlDocument doc)
        {
            cleared = false;
            Inn = inn;
            this.method = method;
            document = doc;
        }

        public void Clear(string inn, ApiMethod method)
        {
            cleared = true;
        }
    }
}