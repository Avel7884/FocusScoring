using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace FocusScoring
{
    internal class XmlDownload : IXmlAccess
    {
        private readonly string focusKey;
        
        
        public XmlDownload(string focusKey)
        {
            this.focusKey = focusKey;
        }
        
        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            //TODO refactor
            var requisites = Settings.OgrnEnabled ? "ogrn" : "inn";
            var wrGETURL = WebRequest.Create($"https://focus-api.kontur.ru/api3/{GetMethodName(method)}?key={focusKey}&{requisites}={inn}&xml");
            Stream webStream = null;
            var netfail=false;
            try{webStream = wrGETURL.GetResponse().GetResponseStream();}
            catch { netfail = true;}
            document = new XmlDocument();
            try {using (var reader = XmlReader.Create(webStream)) { document.Load(reader);}}
            catch {netfail = true;}
            
            return !netfail;
        }

        private string GetMethodName(ApiMethod method)
        {
            switch (method)
            {
                case ApiMethod.analytics: return "analytics";
                case ApiMethod.req: return "req";
                case ApiMethod.buh: return "buh";
                case ApiMethod.contacts: return "contacts";
                case ApiMethod.licences: return "licences";
                case ApiMethod.egrDetails: return "egrDetails";
                default: throw new ArgumentException("Unknown method wtf");
            }
        }
    }
}