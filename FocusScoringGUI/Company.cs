using System;
using System.Collections.Generic;
using System.Linq;
using FocusAccess;
using FocusScoring;

namespace FocusScoringGUI
{
    public class Company_past //TODO 2 responsability class is baaad
    {
    
        public INN Inn { get; set; }
        
        //private XmlAccess access;
        //private Scorer scorer;
        private Dictionary<string,CompanyParameter> paramDict;

        internal Company(INN inn, Dictionary<string, CompanyParameter> paramDict, FocusKeyManager manager = null)
        {    //TODO remove singleton
            manager = manager ?? Settings.DefaultManager;
            //access = manager.Access;
            //scorer = manager.Scorer;
            Inn = inn;
            this.paramDict = paramDict;
           // Score = -1;
           // if(scoringNeeded) MakeScore();
        }
        
        

        public void Reinstance(FocusKeyManager manager)
        {
            //access = manager.Access;
            //scorer = manager.Scorer;
        }

        public void MakeScore()
        {
            Markers = scorer.CheckMarkers(this);
            Score = scorer.CountScore2(Markers.Select(x => x.Marker).ToArray());
        }

        public void ForcedMakeScore(bool scoreNeeded)
        {
            //TODO Check connection
            foreach (var method in (ApiMethod[])Enum.GetValues(typeof(ApiMethod)))
                access.Clear(Inn,method);
            if(scoreNeeded) MakeScore();
        }

        public int Score { get; private set; }
        public MarkerResult[] Markers { get; private set; }
*/

        /*public string GetParam(string paramName)
        {
            var param = paramDict[paramName];
            if (access.TryGetXml(Inn, param.Method, out var document))
            {
                //TODO Kostyl!!!
                var result = document.SelectSingleNode(param.Path)?.InnerText;
                if (result != null)
                    return result;
                if(param.Method != ApiMethod.req)
                    return "";
                return document.SelectSingleNode(param.Path.Replace("UL","IP"))?.InnerText ?? "";
            }
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

        public string CompanyAddress()
        {//TODO make it more complicated
            var b = "/ArrayOfreq/req/UL/legalAddress/parsedAddressRF/";
            var ends = new[]
            {
                ("regionName/topoValue","regionName/topoShortName" ),
                ("region/topoValue", "region/topoShortName"),
                ("city/topoShortName", "city/topoValue"),
                ("settlement/topoShortName", "settlement/topoValue"), 
                ("district/topoShortName", "district/topoValue"), 
                ("street/topoShortName", "street/topoValue"), 
                ("house/topoShortName", "house/topoValue"), 
                ("bulk/topoShortName", "bulk/topoValue")
            };
            var sb = new List<string>();
            if (access.TryGetXml(Inn, ApiMethod.req, out var document))
                foreach (var (end1,end2) in ends)
                {//TODO make it better!
                    var tmp1 = document.SelectSingleNode(b + end1)?.InnerText ?? "";
                    if (tmp1 == "") continue;
                    var tmp2 = document.SelectSingleNode(b + end2)?.InnerText ?? "";
                    if (tmp2 == "г")
                    {
                        sb.Add(tmp1); 
                        continue;
                    }
                    sb.Add(tmp1 + " " + tmp2);
                    //sb.Add(end.EndsWith("me") ? ".":" ");//lol
                }
            return string.Join(", ",sb);
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
        }*/
    }
}

        using FocusApiAccess;
        using System.Collections.Generic;
        using System.Linq;
        using System;
            
        namespace MarkersCheckers
        {
            public static class Болееполовинысвязныхорганизацийвстадииликвидации
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                       // var Dissolved = company.GetParams("DissolvedAffiliates").Length;
                        // var Dissolving = company.GetParams("DissolvingAffiliates").Length;
                        int affiliatesCount = company.GetParams("InnAffilalates").Length;
                        // if (affiliatesCount == 0) return false;
                        // return (Dissolved + Dissolving) / (double)affiliatesCount >= 0.5;
	          if (affiliatesCount < 5) return false;
                        int liquCount = company.GetParams("q7005").Length;
		return (liquCount/affiliatesCount >= 0.5);
                }
            }
            public static class Болееполовинысвязныхорганизацийимеютпризнакибанкротства
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        int affiliatesCount = company.GetParams("InnAffilalates").Length;
                        var m7013 = company.GetParams("m7013Affiliates").Length;
                        var m7014 = company.GetParams("m7014Affiliates").Length;
                        var m7016 = company.GetParams("m7016Affiliates").Length;
                        if (affiliatesCount == 0) return false;
                        return (m7013 + m7014 + m7016) / (double)affiliatesCount > 0.5;
                }
            }
            public static class Вероятноебанкротствоорганизации
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m7013") == "true" || company.GetParam("m7014") == "true" || company.GetParam("m7016") == "true";
                }
            }
            public static class Вреестренедобросовестныхпоставщиков
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m4001") == "true"; 
                }
            }
            public static class Выручкапогруппекомпанийснизиласьболеечемна30
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        if (Выручкапогруппекомпанийснизиласьболеечемна50.Function(company))
                            return false;
                        double s6004;
                        s6004 = company.GetParams("s6004Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => { 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        double s6003;
                        s6003 = company.GetParams("s6003Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => { 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        return s6004 < 0.3 * s6003;
                }
            }
            public static class Выручкапогруппекомпанийснизиласьболеечемна50
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     
                        double s6004;
                        s6004 = company.GetParams("s6004Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => { 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        double s6003;
                        s6003 = company.GetParams("s6003Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x =>{ 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        return s6004 < 0.5 * s6003;
                }
            }
            public static class Директориучредительоднофизическоелицо
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        var a = company.GetParams("head");
                        var b = company.GetParams("FounderFL");
                        return a.Any(x => b.Any(y => x == y));
                }
            }
            public static class Задолженностьпоуплатеналогов
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5004") == "true"; 
                }
            }
            public static class Значительнаясуммаарбитражныхделпогруппекомпаний
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                    if (Критическаясуммаарбитражныхделпогруппекомпаний.Function(company))
                        return false;

                    var casesIst = company.GetMultiParam("s2003Affiliates");
                    var sums = company.GetMultiParam("SumAffiliates");
                    var revs = company.GetMultiParam("s6004Affiliates");
                    var casesOtv = company.GetMultiParam("s2001Affiliates");

double rev;
double sum;
double caseIst;
double caseOtv;

                    var count = .0;
                    for (int i = 0; i < sums.Length; i++)
                        if (DoubleTryParse(revs[i], out rev) &&
                            DoubleTryParse(sums[i], out sum) &&
                            ((DoubleTryParse(casesIst[i], out caseIst) &&
                              (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                             (DoubleTryParse(casesOtv[i], out caseOtv) &&
                              (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                            count += 1;

                    return count / sums.Length > 0.3;
                }
            }
            public static class Значительнаясуммаисполнительныхпроизводств
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sum;
double a;
double b;
                        if (DoubleTryParse(company.GetParam("Sum"), out sum)
                            && DoubleTryParse(company.GetParam("s1001"), out a)
                            && DoubleTryParse(company.GetParam("s6004"), out b))
                            return a > 0.1 * b & a > sum & a > 100000;
                        return false;
                }
            }
            public static class Значительнаясуммаисполнительныхпроизводствпогруппекомпаний
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                                            if (Критическаясуммаисполнительныхпроизводствпогруппекомпаний.Function(company))
                            return false;


                        var revs = company.GetMultiParam("s6004Affiliates");
                        var cases = company.GetMultiParam("s1001Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");

double sum;
double a;
double b;

                        var count = .0;
                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(sums[i], out sum) &&
                                DoubleTryParse(cases[i], out a) &&
                                DoubleTryParse(revs[i], out b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;
                }
            }
            public static class Значительнаячастьорганизацийизгруппыкомпанийсуществуютболее5лет
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
            
            var dates = company.GetMultiParam("regDateAffiliates");
            if (dates.Length == 0)
                return false;
            DateTime date;
            int count = 0;

            foreach (var s in dates)
                if (DateTime.TryParse(s, out date))
                    if ((DateTime.Today - date).Days > (365 * 5 + 1))
                        count++;

            return count * 100 / dates.Length > 30;
                }
            }
            public static class ЗначительноеколвобывшихюрлицруководителясучетомИННФЛ
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    int count;
int totalCount;
return int.TryParse(company.GetParam("q7018"), out count) &&
                               int.TryParse(company.GetParam("q7020"), out totalCount) && (totalCount - count) > 20;
                }
            }
            public static class ЗначительноеколвобывшихюрлицруководителясучетомФИО
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    int count;
int totalCount;
return int.TryParse(company.GetParam("q7019"), out count) &&
                               int.TryParse(company.GetParam("q7021"), out totalCount) && (totalCount - count) > 80;
                }
            }
            public static class Значительноеколвоучрежденныхюрлиц
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     int count;
return int.TryParse(company.GetParam("q7017"), out count) && count > 10;
                }
            }
            public static class ЗначительноеколвоюрлицруководителясучетомИННФЛ
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    int count;
return int.TryParse(company.GetParam("q7018"), out count) && count > 10;
                }
            }
            public static class ЗначительноеколвоюрлицруководителясучетомФИО
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    int count;
return int.TryParse(company.GetParam("q7019"), out count) && count > 50;
                }
            }
            public static class Значительноеколичествокомпанийзарегистрированныхменее12месяцевназад
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    var zi = company.GetMultiParam("m7001Affiliates");
                        var count = .0;
                        for (int i = 0; i < zi.Length; i++)
                            if (zi[i] == "true")
                                count++;

                        return count / zi.Length > 0.3;
                }
            }
            public static class ЗначительноеколичествокомпанийнайденныхвособыхреестрахФНС
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     var zp = company.GetMultiParam("m5002Affiliates");
                        var na = company.GetMultiParam("m5004Affiliates");
                        var kp = company.GetMultiParam("m5006Affiliates");
                        var zi = company.GetMultiParam("m5007Affiliates");
                        var count = .0;
                        for (int i = 0; i < zp.Length; i++)
                            if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                                count++;

                        return count / zp.Length > 0.3;
                }
            }
            public static class Значительноеколичествокомпанийпокоторымтребуетсядополнительнаяпроверка
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     var zi = company.GetMultiParam("m7001Affiliates");
                        //TODO Rename
                        var count = .0;
                        for (int i = 0; i < zi.Length; i++)
                            if (zi[i] == "true")
                                count++;
                        return count / zi.Length > 0.1;
                }
            }
            public static class Значительноечислоликвидированныхсвязанныхкомпаний
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                    if (Статускомпаниисвязанспроизошедшейилипланируемойликвидацией.Function(company))
                    {
                        int Affiliatescount = company.GetParams("InnAffilalates").Count();
                        int q7005Count = company.GetParams("q7005Affiliates").Count();
                        return q7005Count > 5 && q7005Count > Affiliatescount * 0.2;
                    }

                    return false;
                }
            }
            public static class Значительноечислоюрлицпоэтомуадресу
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                    if (!Статускомпаниисвязанспроизошедшейилипланируемойликвидацией.Function(company)) return false;
                    int count1;
                    if (int.TryParse(company.GetParam("q7006"), out count1) && count1 > 10
                    ) //КоличествоНеЛиквидированныхСУчетомНомераОфиса
                        return true;
                    int count2;
                    if (int.TryParse(company.GetParam("q7007"), out count2) && count2 > 50
                    ) //КоличествоНеЛиквидированныхБезУчетаНомераОфиса
                        return true;

                    return false;
                }
            }
            public static class Значительноснизиласьвыручка
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        if (Выручкапогруппекомпанийснизиласьболеечемна50.Function(company))
                            return false;
                        double s6004;
                        s6004 = company.GetParams("s6004Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => { 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        double s6003;
                        s6003 = company.GetParams("s6003Affiliates").Select(x => x.Replace('.', ','))
                            .Sum(x => { 
			double a;
			if(double.TryParse(x,out a)) 
				return a; 
			else 
				return 0.0;});
                        return s6004 < 0.3 * s6003;
                }
            }
            public static class Значительнроечислокомпанийсособымиисполниельнымипроизводствами
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                    var zp = company.GetMultiParam("s1003Affiliates");
                    var na = company.GetMultiParam("s1004Affiliates");
                    var kp = company.GetMultiParam("s1005Affiliates");
                    var zi = company.GetMultiParam("s1006Affiliates");
                    var count = .0;
                    for (int i = 0; i < zp.Length; i++)
                        if (zp[i] == "true" || na[i] == "true" || kp[i] == "true" || zi[i] == "true")
                            count++;
                    return count / zp.Length > 0.3;
                }
            }
            public static class ИзменилсяКППдваждыилиболеезапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    //TODO check correctness 
DateTime date;
                        int count = 0;
                        var kpps = company.GetParams("kpp");
                        foreach (var e in kpps)
                        {
                            if (DateTime.TryParse(e, out date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                }
            }
            public static class Исполнительныепроизводствавзысканиезаложенногоимущества
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m1006") != ""; 
                }
            }
            public static class Исполнительныепроизводствакредитныеплатежи
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m1004") != "";
                }
            }
            public static class Исполнительныепроизводстваналогиисборы
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("s1007") != ""; 
                }
            }
            public static class Исполнительныепроизводстваналожениеареста
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m1003") != ""; 
                }
            }
            public static class Исполнительныепроизводствастраховыевзносы
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("s1008") != "";
                }
            }
            public static class Компаниясмениланазваниедваждыиболеезапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    //TODO check correctness 
DateTime date;
                        int count = 0;
                        var names = company.GetParams("LegalName");
                        foreach (var e in names)
                        {
                            if (DateTime.TryParse(e, out date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                }
            }
            public static class Компаниясмениланазваниезапоследние6месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                      DateTime date;
return DateTime.TryParse(company.GetParam("LegalName"), out date) &&
                               (DateTime.Today - date).Days < 365 / 2;
                }
            }
            public static class Компаниясмениларуководителяилиуправляющуюкомпаниюдваждызапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    DateTime date;
int companiesCount = 0;
                        int headsCount = 0;
                        var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount + headsCount >= 2;
                }
            }
            public static class Компаниясмениларуководителяилиуправляющуюкомпаниюзапоследние6месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    DateTime date;
int companiesCount = 0;
                        int headsCount = 0;
                        var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount > 0 || headsCount > 0;
                }
            }
            public static class Компаниясмениларуководителяилиуправляющуюкомпаниютриждыиболеезапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    DateTime date;
int companiesCount = 0;
int headsCount = 0;
 var managementCompanies = company.GetParams("managementCompanies");
                        var heads = company.GetParams("heads");
                        foreach (var affiliate in managementCompanies)
                        {
                            if (DateTime.TryParse(affiliate, out date) && (DateTime.Today - date).Days < 365 / 2)
                                companiesCount++;
                        }

                        foreach (var head in heads)
                        {
                            if (DateTime.TryParse(head, out date) && (DateTime.Today - date).Days < 365 / 2)
                                headsCount++;
                        }

                        return companiesCount + headsCount > 2;
                }
            }
            public static class Компаниясменилаучредителязапоследние6месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return false;
                }
            }
            public static class Компаниясменилаюрадресдваждызапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    DateTime date;  
int count = 0;
                        var address = company.GetParams("LegalAddress");
                        foreach (var e in address)
                        {
                            if (DateTime.TryParse(e, out date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count == 2;
                }
            }
            public static class Компаниясменилаюрадресзапоследние6месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                      DateTime date;
return DateTime.TryParse(company.GetParam("LegalAddress"), out date) &&
                               (DateTime.Today - date).Days < 365 / 2;
                }
            }
            public static class Компаниясменилаюрадрестриждыиболеезапоследние12месяцев
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    //TODO check correctness 
DateTime date;
                        int count = 0;
                        var address = company.GetParams("LegalAddress");
                        foreach (var e in address)
                        {
                            if (DateTime.TryParse(e, out date) && (DateTime.Today - date).Days < 365)
                                count++;
                        }

                        return count > 2;
                }
            }
            public static class Компаниятерпитубытки
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double value;
return double.TryParse(company.GetParam("s6008").Replace(",","."), out value) && value < 0;
                }
            }
            public static class Критическаясуммаарбитражныхделпогруппекомпаний
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        var casesIst = company.GetMultiParam("s2003Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");
                        var revs = company.GetMultiParam("s6004Affiliates");
                        var casesOtv = company.GetMultiParam("s2001Affiliates");

                        var count = .0;

double rev;
double sum;
double caseIst;
double caseOtv;

                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(revs[i], out rev) &&
                                DoubleTryParse(sums[i], out sum) &&
                                ((DoubleTryParse(casesIst[i], out caseIst) &&
                                  (caseIst > (0.2 * rev)) & (caseIst > 500000) & (caseIst > sum)) ||
                                 (DoubleTryParse(casesOtv[i], out caseOtv) &&
                                  (caseOtv > (0.2 * rev)) & (caseOtv > 500000) & (caseOtv > sum))))
                                count += 1;

                        return count / sums.Length > 0.3;
                }
            }
            public static class Критическаясуммаисполнительныхпроизводств
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sum;
double a;
double b;
                        if (DoubleTryParse(company.GetParam("Sum"), out sum) && DoubleTryParse(company.GetParam("s1001"), out a) &&
                            DoubleTryParse(company.GetParam("s6004"), out b))
                            return a > (0.2 * b) && a > sum & a > 500000;
                        return false;
                }
            }
            public static class Критическаясуммаисполнительныхпроизводствпогруппекомпаний
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        var revs = company.GetMultiParam("s6004Affiliates");
                        var cases = company.GetMultiParam("s1001Affiliates");
                        var sums = company.GetMultiParam("SumAffiliates");
                        var count = .0;

double sum;
double a;
double b;

                        for (int i = 0; i < sums.Length; i++)
                            if (DoubleTryParse(sums[i], out sum) &&
                                DoubleTryParse(cases[i], out a) &&
                                DoubleTryParse(revs[i], out b))
                                if (a > (0.2 * b) && a > sum & a > 100000)
                                    count += 1;

                        return count / sums.Length > 0.3;

                }
            }
            public static class Критическийростсуммыарбитражныхделвкачествеистца
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sumDel;
double sumDelPast;
                        if (DoubleTryParse(company.GetParam("s2003"), out sumDel) &&
                            DoubleTryParse(company.GetParam("s2004"), out sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                }
            }
            public static class Критическийростсуммыарбитражныхделвкачествеответчика
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sumDel;
double sumDelPast;
                        if (DoubleTryParse(company.GetParam("s2001"), out sumDel) &&
                            DoubleTryParse(company.GetParam("s2002"), out sumDelPast))
                            return (sumDelPast > sumDel) & (sumDel > ((sumDelPast - sumDel) / 2)) & (sumDel > 5000000);
                        return false;
                }
            }
            public static class Критическийсуммаарбитражныхделвкачествеистца
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sumDel;
double revenue;
double statedCapitalFocus;

                        if (DoubleTryParse(company.GetParam("s2001"), out sumDel) &&
                            DoubleTryParse(company.GetParam("s6004"), out revenue) &&
                            DoubleTryParse(company.GetParam("Sum"), out  statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                }
            }
            public static class Критическийсуммаарбитражныхделвкачествеответчика
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sumDel;
double revenue;
double statedCapitalFocus;

                        if (DoubleTryParse(company.GetParam("s2003"), out sumDel) &&
                            DoubleTryParse(company.GetParam("s6004"), out revenue) &&
                            DoubleTryParse(company.GetParam("Sum"), out  statedCapitalFocus))
                            return (sumDel > (0.2 * revenue)) & (sumDel > 500000) & (sumDel > statedCapitalFocus);
                        return false;
                }
            }
            public static class Наличиеарбитражнойпрактики
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     return company.GetParam("q2001") != "" || company.GetParam("q2003") != "";
                }
            }
            public static class Наличиебухформзапредыдущийотчетныйпериод
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m6002") != ""; 
                }
            }
            public static class Наличиегосконтрактов
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("q4002") != "" || company.GetParam("q4004") != "";
                }
            }
            public static class Наличиетоварныхзнаков
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("q9001") != "";
                }
            }
            public static class Наличиефилиаловилипредставительств
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParams("Branches").Count() > 0;
                }
            }
            public static class Недостоверныесведениеобадресе
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5006") == "true"; 
                }
            }
            public static class Недостоверныесведенияоруководителеилиучредителе
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5007") == "true"; 
                }
            }
            public static class Непредоставляетотчетностьболеегода
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5005") == "true"; 
                }
            }
            public static class Организациявпроцессереорганизации
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("Reorganizing") == "true"; 
                }
            }
            public static class Организациязарегистрированаболее5леттомуназад
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    DateTime date;
 return (DateTime.TryParse(company.GetParam("regDate"), out date) && (DateTime.Today - date).Days > 365 * 5 + 1);
                }
            }
            public static class Организациязарегистрированаменее12месназад
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m7004") == "true";
                }
            }
            public static class Организациязарегистрированаменее3месназад
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        return company.GetParam("m7004") != "true" && company.GetParam("m7003") != "true" &&
                               company.GetParam("m7002") == "true";
                }
            }
            public static class Организациязарегистрированаменее6месназад
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                     return company.GetParam("m7004") != "true" && company.GetParam("m7003") == "true"; 
                }
            }
            public static class Отсутствуетсвязьпоюрадресу
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5002") == "true"; 
                }
            }
            public static class Рекомендованадополнительнаяпроверка
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m7001") == "true";
                }
            }
            public static class Руководительлибоучредителькомпаниибанкрот
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m7022") == "true"; 
                }
            }
            public static class Руководствовреестредисквалифицированныхлиц
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("m5008") == "true";
                }
            }
            public static class Средиучредителейнайденыиностранныелица
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    return company.GetParam("FoundersForeign") == "true"; 
                }
            }
            public static class Существенноеснижениевыручки
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    long s6004;
long s6003;
                        if (long.TryParse(company.GetParam("s6004").Replace('.', ','), out s6004) &
                            long.TryParse(company.GetParam("s6003").Replace('.', ','), out s6003))
                            return s6004 < (0.5 * s6003);
                        return false;
                }
            }
            public static class Уболеечемполовинысвязанныхкомпанийнайденабухгалтерскаяотчетность
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        var m2 = company.GetMultiParam("m6002Affiliates");
                        return m2.Count(x => x != "") / (double) m2.Length > 0.5;
                }
            }
            public static class Угруппыкомпанийзамеченаактивностьварбитражныхделах
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                        var q1 = company.GetMultiParam("q2001Affiliates");
                        var q3 = company.GetMultiParam("q2003Affiliates");

                        var count = .0;
                        for (int i = 0; i < q1.Length; i++)
                            if (q1[i] != "" || q3[i] != "")
                                count++;

                        return count / q1.Length > 0.3;
                }
            }
            public static class Угруппыкомпанийзамеченаактивностьвгосударственныхторгах
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    
                        var q1 = company.GetMultiParam("q2001Affiliates");
                        var q3 = company.GetMultiParam("q2003Affiliates");

                        var count = .0;
                        for (int i = 0; i < q1.Length; i++)
                            if (q1[i] != "" || q3[i] != "")
                                count++;

                        return count / q1.Length > 0.2;
                }
            }
            public static class Уставныйкапиталболее100000руб
            {         
                public static string verbose = string.Empty;
                public static bool Function()
                {
                    double sum;
return double.TryParse(company.GetParam("Sum").Replace(".",","), out sum) && sum > 100000.0;
                }
            }}