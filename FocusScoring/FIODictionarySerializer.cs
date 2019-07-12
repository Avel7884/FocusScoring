using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace FocusScoring
{
    internal static class FIODictionarySerializer //TODO dry dictionary serializers
    {
        public static void Serialize(Dictionary<string,(string,DateTime)> dict, FileStream stream)
        {
            var serializer = new XmlSerializer(typeof(xmlFIOItem[]), 
                new XmlRootAttribute() { ElementName = "items" });
            serializer.Serialize(stream, 
                dict.Select(kv=> new xmlFIOItem()
            {
                inn = kv.Key,
                fio = kv.Value.Item1,
                time = kv.Value.Item2
            }).ToArray());
        }
        
        
        public static Dictionary<string,(string,DateTime)> Deserialize(FileStream stream)
        {
            var serializer = new XmlSerializer(typeof(xmlFIOItem[]), 
                new XmlRootAttribute() { ElementName = "items" });
            return ((xmlFIOItem[])serializer
                    .Deserialize(stream))
                .ToDictionary(x => x.inn, x => (x.fio,x.time));
        }
    }
    
    public class xmlFIOItem
    {
        //public static xmlItem Create(KeyValuePair<string,(string,DateTime)> kv)
        [XmlAttribute] public string inn;
        [XmlAttribute] public string fio;
        [XmlAttribute] public DateTime time;
    }
    
}