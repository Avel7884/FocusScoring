/*using System;
using System.Linq;
using System.Xml;
using FocusApiAccess.Methods;

namespace FocusApiAccess //TODO UNCOMMENT
{
    internal class AvailabilityAccess : IXmlCache
    {
        private ApiMethod[] availableMethods;

        public AvailabilityAccess(ApiMethod[] availableMethods)
        {
            this.availableMethods = availableMethods;
        }
        
        public bool TryGetXml(ApiMethod method, IQueryComponents args, out XmlDocument document)
        {
            document=new XmlDocument();
            return !availableMethods.Contains(method);
        }

        public void Update(ApiMethod method, IQueryComponents args, XmlDocument doc)
        {
            
        }

        public void Clear(ApiMethod method, IQueryComponents args)
        {}
    }
}*/