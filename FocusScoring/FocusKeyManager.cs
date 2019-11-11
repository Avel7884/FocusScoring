using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml;

namespace FocusScoring
{
    public class FocusKeyManager //TODO very weird class, consider somethin' else
    {
        private readonly string focusKey;
        private readonly XmlDownload downloader;
        //private ApiMethod[] availableMethods;
        
        //public int UsagesLeft { get; internal set; }
        internal XmlAccess Access { get; }
        internal Scorer Scorer {get;}

        public Marker[] GetAllMarkers => Scorer.GetAllMarkers;

        public void RemoveMarker(string name) => Scorer.RemoveMarker(name);
        public void AddMarker(Marker marker) => Scorer.AddMarker(marker);

        //public CompanyParameter[] GetAllParameters => paramDict.Values.ToArray();
        
        public string Usages => CheckUsages();
        
        public static FocusKeyManager StartAccess(string focusKey)
        {
            Settings.DefaultManager = new FocusKeyManager(focusKey);
            return Settings.DefaultManager;
        }

        public ListMonitorer CreateMonitor()
        {
            return new ListMonitorer(focusKey,downloader);
        }

        public ICompanyFactory CreateCompanyFactory()
        {
            return new CompanyFactory(this);
        }
/*

        public Company CreateFromInn(string inn)
        {        //TODO Create factory for dis remove comments
            return new Company(inn,paramDict,this);
        }*/

        public FocusKeyManager(string focusKey)
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
            
            //TODO make it return numbers, somehow
            return $"{nominator} из {denominator}";
        }
 
    }
}