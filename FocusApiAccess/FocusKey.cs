using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;

namespace FocusAccess
{
    public class FocusKey_past : IUrlQueryArg
    {
        private string key;
        private Api3 api;

        public Api3 Api => api ?? throw new InvalidOperationException("Should start api access before any action");

        public FocusKey(string key)
        {
            this.key = key;
        }
        
        public string ToQueryArg()
        {
            return "key=" + key;
        }

        public Api3 StartApiAccess()
        {
            var downloader = new JsonDownload(this);
            var a = new List<IJsonCache>()
            {
                //TODO Get it back
                //new AvailabilityAccess(GetAvailableMethods()), 
                new SingleJsonMemoryCache(),
                new JsonFileSystemCache()
            };
            var access = new JsonAccess(a, downloader);
            var forced = new ForcedJsonAccess(a,downloader);
            api = new Api3(access,forced);
            return Api;
        }
        
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

        public DateTime ExpirationDate
        {
            get
            {
                if (expirationDate == DateTime.MinValue)
                    expirationDate = Api.Stat.MakeRequest(new EmptyUrlArg())
                        .Select(x => x.PeriodEndDate).Select(DateTime.Parse).Min();
                return expirationDate;
            }
        }


        private bool CheckUsages()
        {
            var stat = Api.Stat.MakeRequest(new EmptyUrlArg());
            
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