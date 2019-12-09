using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Timers;
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
            var requisites = Settings.OgrnEnabled ? "ogrn" : "inn";
            return TryGetXml(
                $"https://focus-api.kontur.ru/api3/{GetMethodName(method)}?key={focusKey}&{requisites}={inn}&xml",
                out document);
        }

        internal bool TryGetXml(string request, out XmlDocument document)
        {
            document = new XmlDocument();try
            {               //TODO make errors more informative
                var webStream = WebRequest.Create(request).GetResponseAsync();
                webStream.Wait(5000);
                if (!webStream.IsCompleted)
                {
                    FillDoc(document);
                    return true;
                }
                using (var reader = XmlReader.Create(webStream.Result.GetResponseStream())) { document.Load(reader); }
            }
            catch { return false; }

            return true;
        }

        private void FillDoc(XmlDocument doc)
        {
            //TODO Do not kil meself
            //(1) the xml declaration is recommended, but not mandatory
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration( "1.0", "UTF-8", null );
            XmlElement root = doc.DocumentElement;
            doc.InsertBefore( xmlDeclaration, root );

            //(2) string.Empty makes cleaner code
            XmlElement element1 = doc.CreateElement( string.Empty, "body", string.Empty );
            doc.AppendChild( element1 );

            XmlElement element2 = doc.CreateElement( string.Empty, "level1", string.Empty );
            element1.AppendChild( element2 );

            XmlElement element3 = doc.CreateElement( string.Empty, "level2", string.Empty );
            XmlText text1 = doc.CreateTextNode( "text" );
            element3.AppendChild( text1 );
            element2.AppendChild( element3 );

            XmlElement element4 = doc.CreateElement( string.Empty, "level2", string.Empty );
            XmlText text2 = doc.CreateTextNode( "other text" );
            element4.AppendChild( text2 );
            element2.AppendChild( element4 );
        }
        
        
        internal static string GetMethodName(ApiMethod method)
        {
            switch (method)
            {
                case ApiMethod.analytics: return "analytics";
                case ApiMethod.req: return "req";
                case ApiMethod.buh: return "buh";
                case ApiMethod.contacts: return "contacts";
                case ApiMethod.licences: return "licences";
                case ApiMethod.egrDetails: return "egrDetails";
                case ApiMethod.companyAffiliatesanalytics: return "companyAffiliates/analytics";
                case ApiMethod.companyAffiliatesegrDetails: return "companyAffiliates/egrDetails";
                case ApiMethod.companyAffiliatesreq: return "companyAffiliates/req";
                case ApiMethod.briefReport: return "briefReport";
                case ApiMethod.sites: return "sites";
                default: throw new ArgumentException("Unknown method wtf");
            }
        }
    }
}