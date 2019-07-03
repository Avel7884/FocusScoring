using System.IO;
using System.Net;
using System.Xml;

namespace FocusScoring
{
    internal class XmlDownload : IXmlAccess
    {
        private readonly string focusKey;
        //private static XmlReader reader;

        public XmlDownload(string focusKey)
        {
            this.focusKey = focusKey;
        }
        
        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
        //public XmlDocument GetXml(string sUrl)
            //TODO refactor
            var wrGETURL = WebRequest.Create($"https://focus-api.kontur.ru/api3/{method}?key={focusKey}&inn={inn}&xml");
            Stream webStream = null;
            var netfail=false;
            try{webStream = wrGETURL.GetResponse().GetResponseStream();}
            catch { netfail = true;}
            document = new XmlDocument();
            try {using (var reader = XmlReader.Create(webStream)) { document.Load(reader);}}
            catch {netfail = true;}

            //if (netfail)
                //return null;//new[] {"Ошибка! Проверьте подключение к интернет и повторите попытку."};
            
            return !netfail;
        }
    }
}