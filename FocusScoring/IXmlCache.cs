using System.Xml;

namespace FocusScoring
{
    internal interface IXmlCache : IXmlAccess
    {
        void Update(string inn, ApiMethod method, XmlDocument doc);
    }
}