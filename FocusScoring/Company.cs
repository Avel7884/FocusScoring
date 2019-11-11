using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml;
using FocusScoring;

namespace FocusScoring
{
    public class Company //TODO 2 responsability class is baaad
    {
        
        public string Inn { get; set; }
        
        private XmlAccess access;
        private Scorer scorer;
        private Dictionary<string,CompanyParameter> paramDict;

        internal Company(string inn, Dictionary<string, CompanyParameter> paramDict, FocusKeyManager manager = null)
        {    //TODO remove singleton
            manager = manager ?? Settings.DefaultManager;
            access = manager.Access;
            scorer = manager.Scorer;
            Inn = inn;
            this.paramDict = paramDict;
            Score = -1;
            MakeScore();
        }

        public void MakeScore()
        {
            Markers = scorer.CheckMarkers(this);
            Score = scorer.CountScore2(Markers.Select(x => x.Marker).ToArray());
        }

        public void ForcedMakeScore()
        {
            //TODO Check connection
            foreach (var method in (ApiMethod[])Enum.GetValues(typeof(ApiMethod)))
                access.Clear(Inn,method);
            MakeScore();
        }

        public int Score { get; private set; }
        public MarkerResult[] Markers { get; private set; }

        public string GetParam(string paramName)
        {
            var param = paramDict[paramName];
            if(access.TryGetXml(Inn,param.Method,out var document))
                return document.SelectSingleNode(param.Path)?.InnerText ?? "";
            return "Ошибка! Проверьте подключение к интернет и повторите попытку.";
        }
        
        public string[] GetParams(string paramName)
        {
            var param = paramDict[paramName];
            if (access.TryGetXml(Inn, param.Method, out var document))
                return document.SelectNodes(param.Path).Cast<XmlNode>().Select(n => n.InnerText).ToArray();

            return new[] {"Ошибка! Проверьте подключение к интернет и повторите попытку."};
        }

        public string[] GetMultiParam(string paramName)
        {
            var param = paramDict[paramName];
            if (access.TryGetXml(Inn, param.Method, out var document))
                return GetChild(document, param.Path).ToArray();
            return new[] {"Ошибка! Проверьте подключение к интернет и повторите попытку."};
        }

        private IEnumerable<string> GetChild(XmlDocument document, string node)
        {    
            var splitedNode = node.Split(new[] { '/' }, 4);
            var parent = '/' + splitedNode[1] + '/' + splitedNode[2];

            foreach (XmlNode child in document.SelectNodes(parent))
            { 
                var nodes = child.SelectNodes(splitedNode[3]);
                if (nodes.Count > 0)
                    yield return nodes.Item(0).InnerText;
                else
                    yield return "";
            }
        }
        
        public string CompanyName()
        {
            string _short = GetParam("Short");
            string full = GetParam("Full");
            string fio = GetParam("FIO");
            if (_short != "")
            {
                _short = _short.Replace("Общество с ограниченной ответственностью", "ООО");
                _short = _short.Replace("Закрытое акционерное общество", "ЗАО");
                _short = _short.Replace("Акционерное общество", "АО");
                return _short;
            }
            if (full != "")
            {
                full = full.Replace("ОБЩЕСТВО С ОГРАНИЧЕННОЙ ОТВЕТСТВЕННОСТЬЮ", "ООО");
                full = full.Replace("Общество с ограниченной ответственностью", "ООО");
                full = full.Replace("Закрытое акционерное общество", "ЗАО");
                full = full.Replace("Акционерное общество", "АО");
                return full;
            }

            if (fio != "")
            {
                return "ИП" + " " + fio;
            }

            return "";
        }
    }
}
