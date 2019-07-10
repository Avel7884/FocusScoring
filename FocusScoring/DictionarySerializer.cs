using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace FocusScoring
{
    internal static class DictionarySerializer
    {
        public static void Serialize(Dictionary<(string node, ApiMethod method), (long position,int count,DateTime time)> dict, FileStream stream)
        {
            var serializer = new XmlSerializer(typeof(xmlItem[]), 
                new XmlRootAttribute() { ElementName = "items" });
            serializer.Serialize(stream, 
                dict.Select(kv=>new xmlItem(){node = kv.Key.node,method= kv.Key.method,pos= kv.Value.position,count = kv.Value.count,time = kv.Value.time}).ToArray() );
        }
        
        
        public static Dictionary<(string, ApiMethod), (long,int,DateTime)> Deserialize(FileStream stream)
        {
            var serializer = new XmlSerializer(typeof(xmlItem[]), 
                new XmlRootAttribute() { ElementName = "items" });
            return ((xmlItem[])serializer
                    .Deserialize(stream))
                    .ToDictionary(x => (x.node,x.method), x => (x.pos,x.count,x.time));
        }
    }
    
    public class xmlItem
    {
        [XmlAttribute]
        public string node;
        [XmlAttribute]
        public ApiMethod method;
        [XmlAttribute]
        public long pos;
        [XmlAttribute]
        public int count;
        [XmlAttribute]
        public DateTime time;
    }
}