using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using FocusAccess.Methods;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusAccess
{
    internal class JsonDownload : IJsonAccess
    {
        private readonly FocusKey focusKey;

        public JsonDownload(FocusKey focusKey)
        {
            this.focusKey = focusKey;
        }

        public bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, out string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            var req = $"{Settings.ApiUrl}{method.Url}?{focusKey.ToQueryArg()}&{args.AssembleQuery()}";
            if (args is IPostArguments postArguments)
                return TryPostXml(req,postArguments.Data,out json);
            return TryGetXml(req, out json);
        }

        private bool TryGetXml(string request, out string json)
        {
            json = default;
            try
            {
                //TODO make errors more informative
                var webStream = WebRequest.Create(request).GetResponseAsync();
                webStream.Wait(5000); //TODO possible error here
                var code = ((HttpWebResponse) webStream.Result).StatusCode;
                if ((int)code < 200 && (int)code >= 300)
                {
                    
                    return false;
                }
                /*var array = JsonConvert.DeserializeObject<IList<TData>>(
                    new StreamReader(webStream.Result.GetResponseStream() ?? throw new ExternalException())
                        .ReadToEnd());*/
                json = new StreamReader(webStream.Result.GetResponseStream() ?? throw new ExternalException())
                    .ReadToEnd();
                /*if (!webStream.IsCompleted)
                {
                    FillDoc(obj);
                    return true;
                }
                using (var reader = XmlReader.Create(webStream.Result.GetResponseStream())) { obj.Load(reader); }*/
            }
            catch(WebException e)
            {
                
                return false;
            }
            
            
            return true;
        }

        private bool TryPostXml(string request, string Data, out string json)
        {
            json = default;
            try
            {
                var req = WebRequest.Create(request);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                Stream reqStream = req.GetRequestStream();
                byte[] postArray = Encoding.ASCII.GetBytes(Data);
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close(); //TODO dry
                
                var webStream = req.GetResponseAsync();
                webStream.Wait(5000);
                /*var array = JsonConvert.DeserializeObject<IList<TData>>(
                    new StreamReader(webStream.Result.GetResponseStream() ?? throw new ExternalException())
                        .ReadToEnd());*/
                json = new StreamReader(webStream.Result.GetResponseStream() ?? throw new ExternalException())
                    .ReadToEnd();
                /*if (!webStream.IsCompleted)
                {
                    FillDoc(obj);
                    return true;
                }
                using (var reader = XmlReader.Create(webStream.Result.GetResponseStream())) { obj.Load(reader); }*/
            }
            catch { return false; }

            return true;
        }
/*
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
        }*/
    }
}