/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Xml;

namespace FocusApiAccess
{
    public class FocusKeyManager
    {
        private readonly string focusKey;
        private readonly JsonDownload downloader;
        //private ApiMethod[] availableMethods;
        
        //public int UsagesLeft { get; internal set; }
        internal JsonAccess Access { get; }
        //public IParameterProvider Provider { get; }
        /*
        
        internal Scorer Scorer {get;}

        public Marker[] GetAllMarkers => Scorer.GetAllMarkers;

        public void RemoveMarker(string name) => Scorer.RemoveMarker(name);
        public void AddMarker(Marker marker) => Scorer.AddMarker(marker);
#1#

        //public CompanyParameter[] GetAllParameters => paramDict.Values.ToArray();
        
        //public string Usages => CheckUsages();
        
        public static FocusKeyManager StartAccess(string focusKey)
        {
            Settings.DefaultManager = new FocusKeyManager(focusKey);
            return Settings.DefaultManager; 
        }

        /*public ICompanyFactory CreateCompanyFactory()
        {
            return new CompanyFactory(this);
        }
        #1#

        public bool IsBaseMode()
        {
            return !IsParamAvailable("m1003");
        }

        public Api3 GetApi()
        {
            return new Api3(Access);
        }
        
        public FocusKeyManager(string focusKey)
        {
            /*this.focusKey = focusKey;
            downloader = new JsonDownload(focusKey);
            Access = new JsonAccess(
                new List<IJsonCache>()
                {    //TODO Get it back
                    //new AvailabilityAccess(GetAvailableMethods()), 
                    new SingleJsonMemoryCache(), 
                    new JsonFileSystemCache()
                }, downloader);#1#
            //Provider = new ParameterProvider(Access);
        }

        /*public int IsCompanyUsed(IList<string> inns)
        {
            var usagesCount = 0;
            for (var i = 0; i < inns.Count; i += 50)
            {
                var innString = string.Join(",", inns.Skip(i).Take(50));
                if (!downloader.TryGetXml(
                    $"https://focus-api.kontur.ru/api3/req/expectedLimitUsage?key={focusKey}&inn={innString}&xml",
                    out var doc))
                    return 0;
                var usagesResult = doc.SelectNodes("/expectedLimitUsage/count").Cast<XmlNode>().First().InnerText;
                usagesCount += int.Parse(usagesResult);
            }
            return usagesCount;
        }

        private ApiMethod[] availableMethods;
        public ApiMethod[] GetAvailableMethods()
        {    //TODO pass error here somehow 
            if (availableMethods != null)
                return availableMethods;
            if (!downloader.TryGetXml("https://focus-api.kontur.ru/api3/stat?xml&key=" + focusKey, out var doc))
            {
                availableMethods = new ApiMethod[0];
                return availableMethods; // "Ошибка! Проверьте подключение к интернет и повторите попытку.";
            }
            var methods = 
                doc.SelectNodes("/ArrayOfstat/stat/methodName")
                    .Cast<XmlNode>()
                    .SelectMany(x => x.InnerText.Split(new[] {" & "}, StringSplitOptions.RemoveEmptyEntries))
                    .Select(x => string.Join("", x.Split('/').Skip(1)))
                    .ToArray();
            availableMethods = ((ApiMethod[]) Enum.GetValues(typeof(ApiMethod)))
                .Where(x => methods.Contains(x.ToString()))
                .ToArray();
            return availableMethods;
        }#1#

        public bool IsParamAvailable(string paramName)
        {        //TODO Something terrible
            throw new NotImplementedException();
            /*if (!ParameterProvider.paramTupDict.TryGetValue(paramName,out var t))
                throw new ArgumentException("InvalidParamName");
            return GetAvailableMethods().Select(x=>x.GetType().Name).Contains(t.Item1.ToString());//TODO unreflex here#1#
        }
/*

        private int nominator = int.MinValue;
        private int denominator = int.MaxValue;
        private DateTime expirationDate;
        private string CheckUsages()
        {
            if (!downloader.TryGetXml("https://focus-api.kontur.ru/api3/stat?xml&key=" + focusKey, out var doc))
                return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
            
            nominator = int.Parse(doc.SelectNodes("/ArrayOfstat/stat/spent")
                .Cast<XmlNode>()
                .Select(x => x.InnerText)
                .Select(int.Parse).Max().ToString());
            denominator = int.Parse(doc.SelectSingleNode("/ArrayOfstat/stat/limit").InnerText);

            expirationDate = doc.SelectNodes("/ArrayOfstat/stat/periodEndDate")
                .Cast<XmlNode>()
                .Select(x => x.InnerText)
                .Select(DateTime.Parse).Min();
            
            //TODO make it return numbers, somehow
            return $"{nominator} из {denominator}";
        }
        public bool AbleToUseMore(int more) => 
            nominator + more <= denominator && expirationDate >= DateTime.Today;
#1#

    }
}*/