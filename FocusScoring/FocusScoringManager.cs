using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml;

namespace FocusScoring
{
    public class FocusScoringManager //TODO very weird class, consider somethin' else
    {
        private string focusKey;
        private XmlDownload downloader;
        //private ApiMethod[] availableMethods;
        
        //public int UsagesLeft { get; internal set; }
        internal XmlAccess Access { get; set; }
        internal Scorer Scorer {
            get;
            set;
        }

        public Marker[] GetAllMarkers => Scorer.GetAllMarkers;

        
        public string Usages => CheckUsages();
        
        public static FocusScoringManager StartAccess(string focusKey)
        {
            Settings.DefaultManager = new FocusScoringManager(focusKey);
            return Settings.DefaultManager;
        }

        public ListMonitorer StartMonitor()
        {
            return new ListMonitorer(focusKey,downloader);
        }

        public Company CreateFromInn(string inn)
        {        //TODO bring it here with constructor
            return Company.CreateINN(inn);
        }

        public FocusScoringManager(string focusKey)
        {
            this.focusKey = focusKey;
            Scorer = new Scorer();
            downloader = new XmlDownload(focusKey);
            Access = new XmlAccess(
                new List<IXmlCache>()
                {
                    new AvailabilityAccess(GetAvailableMethods()), 
                    new SingleXmlMemoryCache(), 
                    new XmlFileSystemCache()
                }, downloader);
        }
        
        private ApiMethod[] GetAvailableMethods()
        {    //TODO pass error here somehow 
            if (!downloader.TryGetXml("https://focus-api.kontur.ru/api3/stat?xml&key=" + focusKey, out var doc))
                return new ApiMethod[0]; // "Ошибка! Проверьте подключение к интернет и повторите попытку.";
            var methods = 
                doc.SelectNodes("/ArrayOfstat/stat/methodName")
                    .Cast<XmlNode>()
                    .SelectMany(x => x.InnerText.Split(new[] {" & "}, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => string.Join("", x.Split('/').Skip(1)))
                    .ToArray();
            return ((ApiMethod[]) Enum.GetValues(typeof(ApiMethod)))
                .Where(x => methods.Contains(x.ToString()))
                .ToArray();
        }  
        
        private string CheckUsages()
        {
            if (!downloader.TryGetXml("https://focus-api.kontur.ru/api3/stat?xml&key=" + focusKey, out var doc))
                return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
            
            var nominator = doc.SelectNodes("/ArrayOfstat/stat/spent")
                .Cast<XmlNode>()
                .Select(x => x.InnerText)
                .Select(int.Parse).Max().ToString();
            var denominator = doc.SelectSingleNode("/ArrayOfstat/stat/limit").InnerText;
            
            return $"{nominator} из {denominator}";
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
                default: throw new ArgumentException("Unknown method wtf");
            }
        }
        
    }
}