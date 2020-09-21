using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;

namespace FocusAccess
{
    public class FocusKey : IUrlQueryArg
    {
        private string key;
        private Api api;

        //public IApi3 Api => new Api(this);// ?? throw new InvalidOperationException("Should start api access before any action");
        //public IDifferentialApi DifferentialApi => new DifferentialApi(this);

        public FocusKey(string key)
        {
            this.key = key;
            Access = CreateAccess();
            DifferentialAccess = CreateDifferentialAccess();
            api = new Api(this);
        }
        

        private JsonAccess CreateAccess()
        {
            var downloader = new JsonDownload(this);
            var a = new List<IJsonCache>()
            {
                //TODO Get it back
                //new AvailabilityAccess(GetAvailableMethods()), 
                new SingleJsonMemoryCache(),
                new JsonFileSystemCache()
            };
            return new JsonAccess(a, downloader);
        }
        
        private JsonAccess CreateDifferentialAccess()
        {
            var downloader = new JsonDownload(this);
            var a = new List<IJsonCache>()
            {
                new DifferentialCache(new JsonFileSystemCache())
            };
            return new JsonAccess(a, downloader);
        }
        
        public string ToQueryArg()
        {
            return "key=" + key;
        }

        /*public IApi3 StartApiAccess()
        {
            var downloader = new JsonDownload(this);
            var a = new List<IJsonCache>()
            {
                //new AvailabilityAccess(GetAvailableMethods()), 
                new SingleJsonMemoryCache(),
                new JsonFileSystemCache()
            };
            var access = new JsonAccess(a, downloader);
            var forced = new ForcedJsonAccess(a,downloader);
            api = new Api(access,forced);
            return Api;
        }*/

        
        /*public int IsCompanyUsed(IList<string> inns)
         { //TODO figure it out
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
        }*/

        private bool CheckValidity()
        {
            return ExpirationDate < DateTime.Now;
        }
        
        public long Nominator { get; private set; }
        public long Denominator { get; private set; }
        
        private DateTime expirationDate = DateTime.MinValue;
        internal JsonAccess DifferentialAccess { get; }

        internal JsonAccess Access { get; }

        public DateTime ExpirationDate
        {
            get
            {
                if (expirationDate == DateTime.MinValue)
                    expirationDate = api.Stat()
                        .Select(x => x.PeriodEndDate).Select(DateTime.Parse).Min();
                return expirationDate;
            }
        }

        private bool CheckUsages()
        {
            var stat = api.Stat();
            Nominator = stat.Select(x=>x.Spent).Max() ?? throw new Exception();
            Denominator = stat[0].Limit ?? throw new Exception();

            expirationDate = stat.Select(x=>x.PeriodEndDate).Select(DateTime.Parse).Min();
            
            //TODO make it return numbers, somehow
            return Nominator < Denominator;
        }
        public bool AbleToUseMore(int more) => 
            Nominator + more <= Denominator && expirationDate >= DateTime.Today;
    }
}