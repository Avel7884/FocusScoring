using System.Xml;

namespace FocusScoring
{
    internal interface IXmlAccess
    {
        bool TryGetXml(INN inn, ApiMethod method, out XmlDocument document);
    }
}