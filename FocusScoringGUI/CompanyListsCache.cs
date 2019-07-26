using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public class CompanyListsCache
    {
        private static CompanyListsCache cache;
        private XmlSerializer serializer;
        private string companyListPath;

        public static CompanyListsCache Create(string companyListFolder ="CompanyLists" )
        {
            
            var retCache = cache ?? (cache = new CompanyListsCache());
            retCache.companyListPath = Settings.CachePath+ companyListFolder;
            
            if (!Directory.Exists(retCache.companyListPath))
                Directory.CreateDirectory(retCache.companyListPath);
            
            return retCache;
        }

        private CompanyListsCache()
        {
            serializer = new XmlSerializer(typeof(MainWindow.CompanyData[]), new XmlRootAttribute("item"));
        }

        public IEnumerable<MainWindow.CompanyData> GetAllCompanies()
        {
            foreach (var filePath in Directory.GetFiles("./CompanyLists"))
                using (var file = File.Open(filePath,FileMode.OpenOrCreate))
                    foreach (var company in (MainWindow.CompanyData[]) serializer.Deserialize(file))
                        yield return company;
        }
        
        //Depricated
        public Dictionary<string,List<MainWindow.CompanyData>> GetLists()
        {                                     //TODO get it thought constructor
            var dict = new Dictionary<string,List<MainWindow.CompanyData>>();
            foreach (var filePath in Directory.GetFiles(companyListPath))
            {
                using (var file = File.Open(filePath,FileMode.OpenOrCreate))
                {
                    dict[filePath.Split('\\').Last()] = ((MainWindow.CompanyData[]) serializer
                        .Deserialize(file)).ToList();
                }
            }
            return dict;
        }

        public List<string> GetNames()
        {
            return Directory.GetFiles(companyListPath).Select(x => x.Split('\\').Last()).ToList();
        }

        public List<MainWindow.CompanyData> GetList(string name)
        {
            if (!File.Exists(companyListPath + "/" + name)) throw new FileNotFoundException();
            using (var file = File.Open(companyListPath + "/" + name, FileMode.OpenOrCreate))
                return ((MainWindow.CompanyData[]) serializer.Deserialize(file)).ToList();
        }

        public void UpdateList(string name, IEnumerable<MainWindow.CompanyData> data)
        {
            if(File.Exists(companyListPath + "/" + name))
                using (var file = File.Open(companyListPath + "/"+name,FileMode.OpenOrCreate))
                {
                    var dict = ((MainWindow.CompanyData[]) serializer.Deserialize(file)).ToDictionary(x=>x.Inn);
                    foreach (var company in data)
                        dict[company.Inn] = company;
                    file.Position = 0;
                    serializer.Serialize(file, dict.Values.ToArray());
                    file.Write(new byte[file.Length-file.Position],0,(int)(file.Length-file.Position));
                }
            else
            {
                using (var file = File.Open(companyListPath + "/" + name, FileMode.OpenOrCreate))
                    serializer.Serialize(file, data.ToArray());
            }
        }

        public void DeleteList(string name)
        {
            if(File.Exists(companyListPath + "/" + name))
                File.Delete(companyListPath + "/" + name);
        }
    }
}