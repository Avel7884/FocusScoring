using System.Xml;

namespace FocusScoring
{
    internal interface IXmlCache : IXmlAccess
    {
        void Update(INN inn, ApiMethod method, XmlDocument doc);
        void Clear(INN inn, ApiMethod method);
    }
}