using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.IO.MemoryMappedFiles;


namespace FocusScoring
{
    internal class XmlCache : IDisposable, IXmlAccess
    {
        private readonly TimeSpan cacheTTL;
        private readonly Dictionary<(string, ApiMethod), (long,int,DateTime)> spansDict;
        private MemoryMappedFile cacheFile;
        private Action recache;
        private long position = 0; 

        public XmlCache(string cachePath="./cache", TimeSpan cacheTTL=default(TimeSpan), long initialCapacity = 10*1024*1024)
        {
            this.cacheTTL = cacheTTL == default(TimeSpan) ? TimeSpan.FromDays(7) : cacheTTL;
            this.spansDict = new Dictionary<(string, ApiMethod), (long,int,DateTime)>();
            cacheFile = MemoryMappedFile.CreateFromFile(cachePath, FileMode.Open,"ImgA", initialCapacity);
            recache = () =>
            {
                initialCapacity *= 2;
                cacheFile = MemoryMappedFile.CreateFromFile(cachePath, FileMode.Open,"ImgA", initialCapacity);
            };
        }

        public bool TryGetXml(string inn, ApiMethod method, out XmlDocument document)
        {
            document = new XmlDocument();

            (long offset, int count, DateTime date) span;
            if (!spansDict.TryGetValue((inn, method), out span) || DateTime.Today-span.date > cacheTTL)
                return false;
            //TODO cache deletion
            
            using (var stream = cacheFile.CreateViewStream(span.offset, span.count))
                using (var reader = XmlReader.Create(stream))
                    document.Load(reader);
            
              return true;
        }

        public int WriteCache(string inn, ApiMethod method, XmlDocument doc)
        {                                              //TODO file size
            using (var stream = cacheFile.CreateViewStream(position,1024*256))
            {
                doc.Save(stream);
                spansDict[(inn, method)] = (position,(int)stream.Position,DateTime.Today);
                position+=(int)stream.Position;
                return (int)stream.Position;
            }
        }

        public void Dispose()
        {
            cacheFile.Dispose();
        }
    }
}