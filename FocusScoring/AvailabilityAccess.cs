using System;
using System.Linq;
using System.Xml;

namespace FocusScoring
{
    internal class AvailabilityAccess : IXmlCache
    {
        private ApiMethod[] availableMethods;

        public AvailabilityAccess(ApiMethod[] availableMethods)
        {
            this.availableMethods = availableMethods;
        }
        
        public bool TryGetXml(INN inn, ApiMethod method, out XmlDocument document)
        {
            document=new XmlDocument();
            return !availableMethods.Contains(method);
        }

        public void Update(INN inn, ApiMethod method, XmlDocument doc)
        {
            
        }

        public void Clear(INN inn, ApiMethod method)
        {}
    }
}