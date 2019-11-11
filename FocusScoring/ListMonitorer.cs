using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Timers;
using System.Xml;

namespace FocusScoring
{
    public class ListMonitorer
    {
        private readonly string key;
        private readonly XmlDownload dow;
        private Timer timer;

        internal ListMonitorer(string key, XmlDownload dow)
        {
            this.key = key;
            this.dow = dow;
            timer = new Timer(30*60*1000);
            timer.Elapsed += (o, e) =>
            {
                if (Check(out var changed)) 
                    CompaniesUpdated(o, new MonitorEventArgs(changed));
            };
        }

        public event Action<object, MonitorEventArgs> CompaniesUpdated; 
        
        private bool Check(out string[] changedCompanies)
        {
            IEnumerable<string> egrInns = new string[0];
            IEnumerable<string> reqInns = new string[0];;  //TODO shorten
            if (dow.TryGetXml($"https://focus-api.kontur.ru/api3/egrDetails/mon?key={key}&date={DateTime.Today.ToString()}&xml",out var doc))
                egrInns = doc.SelectNodes("/ArrayOfmonListItem/monListItem/inn").Cast<XmlNode>().Select(x => x.InnerText);
            if(dow.TryGetXml($"https://focus-api.kontur.ru/api3/req/mon?key={key}&date={DateTime.Today.ToString()}&xml",out doc))
                reqInns = doc.SelectNodes("/ArrayOfmonListItem/monListItem/inn").Cast<XmlNode>().Select(x=>x.InnerText);

            changedCompanies = egrInns.Concat(reqInns).ToArray();
            return changedCompanies.Length != 0;
        }
        
        public string[] MonitoringCompanies
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Update(IEnumerable<string> inns,bool append = true)
        {
            var data = Encoding.ASCII.GetBytes(string.Join(" ", inns));
            var request = WebRequest.Create($"https://focus-api.kontur.ru/api3/monList?append={append}&key={key}");
            
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
                stream.Write(data, 0, data.Length);

            var response = (HttpWebResponse)request.GetResponse();
            return (int)response.StatusCode >= 200 && (int)response.StatusCode < 300;
        }

        public void Delete()
        {
            
        }

        public class MonitorEventArgs : EventArgs
        {
            public string[] ChangedCompanies { get; }

            public MonitorEventArgs(string[] changedCompanies)
            {
                ChangedCompanies = changedCompanies;
            }
        }
    }
}