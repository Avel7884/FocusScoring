using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public class ListsCache<T>
    {
        private XmlSerializer serializer;
        private string companyListPath;

        public ListsCache(string companyListFolder = null)
        {
            companyListFolder = companyListFolder ?? typeof(T).Name + "Lists";
            serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute("item"));
            companyListPath = Settings.CachePath+ companyListFolder;
            
            if (!Directory.Exists(companyListPath))
                Directory.CreateDirectory(companyListPath);
        }
        
/*

        public IEnumerable<T> GetAllCompanies()
        {
            foreach (var filePath in Directory.GetFiles("./CompanyLists"))
                using (var file = File.Open(filePath,FileMode.OpenOrCreate))
                    foreach (var company in (T[]) serializer.Deserialize(file))
                        yield return company;
        }
        
        //Depricated
        public Dictionary<string,List<T>> GetLists()
        {                                     //TODO get it thought constructor
            var dict = new Dictionary<string,List<T>>();
            foreach (var filePath in Directory.GetFiles(companyListPath))
            {
                using (var file = File.Open(filePath,FileMode.OpenOrCreate))
                {
                    dict[filePath.Split('\\').Last()] = ((T[]) serializer
                        .Deserialize(file)).ToList();
                }
            }
            return dict;
        }*/

        public List<string> GetNames()
        {
            return Directory.GetFiles(companyListPath).Select(x => x.Split('\\').Last()).ToList();
        }

        public List<T> GetList(string name)
        {
            if (!File.Exists(companyListPath + "/" + name)) throw new FileNotFoundException();
            using (var file = File.Open(companyListPath + "/" + name, FileMode.OpenOrCreate))
                return ((T[]) serializer.Deserialize(file)).ToList();
        }

        public void UpdateList(string name, IEnumerable<T> data)
        {
            DeleteList(name);
            using (var file = File.Create(companyListPath + "\\" + name))
                serializer.Serialize(file, data.ToArray());
            
            /*if(File.Exists(companyListPath + "/" + name))
                using (var file = File.Open(companyListPath + "/"+name,FileMode.OpenOrCreate))
                {
                    var dict = ((T[]) serializer.Deserialize(file)).ToDictionary(x=>x.Inn);
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
            }*/
        }
        
        public void DeleteList(string name)
        {
            if(File.Exists(companyListPath + "/" + name))
                File.Delete(companyListPath + "/" + name);
        }
    }
}