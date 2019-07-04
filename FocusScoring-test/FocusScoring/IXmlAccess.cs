using System.Xml;

namespace FocusScoring
{
    internal interface IXmlAccess
    {
        bool TryGetXml(string inn, ApiMethod method, out XmlDocument document);
    }
}