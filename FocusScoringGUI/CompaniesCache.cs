/*using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FocusApiAccess;

namespace FocusScoringGUI
{
    public class CompaniesCache
    {
        private XmlSerializer serializer;
        private string companyListPath;

        public CompaniesCache(string companyListFolder = null)
        {
            companyListFolder = companyListFolder ?? typeof(T).Name + "Lists";
            serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute("item"));
            companyListPath = Settings.CachePath+ companyListFolder;
            
            if (!Directory.Exists(companyListPath))
                Directory.CreateDirectory(companyListPath);
        }

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
            }#1#
        }
        
        public void DeleteList(string name)
        {
            if(File.Exists(companyListPath + "/" + name))
                File.Delete(companyListPath + "/" + name);
        }
    }
}*/