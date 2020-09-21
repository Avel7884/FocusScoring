using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FocusMonitoring
{
    public interface IMonitoringFactory
    {
        IMonitoringSet OpenSet();
        MonitoringChange[] OpenChanges<TResultValue>(MonitoringTarget target);
    }

    public class MonitoringFactory : IMonitoringFactory
    {
        private readonly string monitoringFolder;
        private readonly string shortLog;
        private readonly string monitoringSetFilePath;

        public MonitoringFactory(string monitoringFolder = "./", string monitoringSetFile = "OnMonitoring", string shortLog = "ShortLog")
        {
            this.monitoringFolder = monitoringFolder;  
            this.shortLog = shortLog;
            monitoringSetFilePath = monitoringFolder + monitoringSetFile;
        }
        
        private FileStream file;

        public IMonitoringSet OpenSet() =>
            OpenRelivingSet();

        public MonitoringChange[] OpenChanges<TResultValue>(MonitoringTarget target) => 
            File.ReadAllLines(monitoringFolder + target.MakeFileName())
                .Select(x => new MonitoringChange(x))
                .ToArray();

        internal IRelivingChangesMonitoringSet OpenRelivingSet()
        { 
            file = AwaitFile(monitoringSetFilePath);
            using var reader = new StreamReader(file);
            var set = new MonitoringSet(this);
            var str = reader.ReadToEnd();
            JsonConvert.PopulateObject(str, set, Converter.Settings);
            return set;
        }

        internal void WriteShortLog(IEnumerable<MonitoringResult> results) => 
            File.WriteAllLines(
                monitoringFolder + "/" + shortLog,
                results.Select(JsonConvert.SerializeObject));

        private static FileStream AwaitFile(string filePath)
        {
            Exception exception = default;
            for(var i =0;i<10;i++)
            {
                try
                {
                    return EnsureJson(filePath);
                }
                catch (IOException e)
                {
                    exception = e;
                    Thread.Sleep(100);
                }
                
            }
            throw exception ?? throw new Exception("Be i damned if that happened!");
        }

        private static FileStream EnsureJson(string filePath)
        {
            var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            if (stream.Length != 0) return stream;
            using var writer = new StreamWriter(stream, Encoding.UTF8, 512, true);
            writer.Write("{\"HasNewChanges\":\"false\",\"Targets\":[],\"Date\":\"2011.08.11\"}");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        

        internal void SaveSet(object obj)
        {
            using var writer = new StreamWriter(monitoringSetFilePath);
            writer.Write(JsonConvert.SerializeObject(obj,Converter.Settings));
            file.Dispose();
        }
    }
}