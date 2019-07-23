using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace FocusScoring
{
    public class FocusScoringManager //TODO very weird class, consider somethin' else
    {
        private string focusKey;
        private XmlDownload downloader;
            
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
            Access =new XmlAccess(new List<IXmlCache>() {new SingleXmlMemoryCache(), new XmlFileSystemCache()},downloader);
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
            
            return $"{nominator}/{denominator}";
        }
    }
}