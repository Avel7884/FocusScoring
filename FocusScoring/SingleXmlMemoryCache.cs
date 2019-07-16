using System;
using System.Xml;

namespace FocusScoring
{
    public class SingleXmlMemoryCache : IXmlCache
    {
        private string Inn = string.Empty;
        private ApiMethod method;
        private XmlDocument document;

        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            if (Inn == inn && method == this.method)
            {
                document = this.document;
                return true;
            }

            document = new XmlDocument();
            return false;
        }

        public void Update(string inn, ApiMethod method, XmlDocument doc)
        {
            Inn = inn;
            this.method = method;
            document = doc;
        }
    }
}