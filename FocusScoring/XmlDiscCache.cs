/*using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO.MemoryMappedFiles;


namespace FocusScoring
{
    internal class XmlDiscCache : IDisposable, IXmlCache
    {
        private readonly string cachePath;
        private readonly TimeSpan cacheTTL;
        private readonly Dictionary<(INN, ApiMethod), (long, int, DateTime)> spansDict;
        private MemoryMappedFile cacheFile;

        private readonly FileStream dictCacheFile;
        //private Action recache;
        private long position = 0;

        public XmlDiscCache(string cachePath = null, TimeSpan cacheTTL = default(TimeSpan), long initialCapacity = 10 * 1024 * 1024)
        {
            this.cachePath = cachePath ?? Settings.CachePath;
            this.cacheTTL = cacheTTL == default(TimeSpan) ? TimeSpan.FromDays(7) : cacheTTL;

            if (File.Exists(cachePath + "cacheDict"))
            {
                dictCacheFile = File.Open(cachePath + "cacheDict", FileMode.OpenOrCreate);
                spansDict = CacheDictionarySerializer.Deserialize(dictCacheFile);
            }
            else
            {
                dictCacheFile = File.Open(cachePath + "cacheDict", FileMode.OpenOrCreate);
                spansDict = new Dictionary<(INN, ApiMethod), (long, int, DateTime)>();
            }

            cacheFile = MemoryMappedFile.CreateFromFile(cachePath + "cache", FileMode.OpenOrCreate, "ImgA", initialCapacity);
            //            recache = () =>
            //            {
            //                initialCapacity *= 2;
            //                cacheFile = MemoryMappedFile.CreateFromFile(cachePath+"cache", FileMode.Open,"ImgA", initialCapacity);
            //            };
        }

        public bool TryGetXml(INN inn, ApiMethod method, out XmlDocument document)
        {
            document = new XmlDocument();

            (long offset, int count, DateTime date) span;
            if (!spansDict.TryGetValue((inn, method), out span) || DateTime.Today - span.date > cacheTTL)
                return false;
            //TODO cache deletion

            using (var stream = cacheFile.CreateViewStream(span.offset, span.count))
            using (var reader = XmlReader.Create(stream))
                document.Load(reader);

            return true;
        }

        public void Update(INN inn, ApiMethod method, XmlDocument doc)
        {                                              //TODO file size
            using (var stream = cacheFile.CreateViewStream(position, 0))
            {
                doc.Save(stream);
                spansDict[(inn, method)] = (position, (int)stream.Position, DateTime.Today);
                position += (int)stream.Position;
                dictCacheFile.Position = 0;
                CacheDictionarySerializer.Serialize(spansDict, dictCacheFile);
                //return (int)stream.Position;
            }
        }

        public void Clear(INN inn, ApiMethod method)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            cacheFile.Dispose();
            dictCacheFile.Position = 0;
            CacheDictionarySerializer.Serialize(spansDict, dictCacheFile);
        }
    }
}*/