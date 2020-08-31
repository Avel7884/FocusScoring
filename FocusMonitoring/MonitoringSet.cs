using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace FocusMonitoring
{
    public class MonitoringSet : IRelivingChangesMonitoringSet
    {
        internal readonly string filePath;
        //private FileStream file;
        //private IRelivingChangesMonitoringSet source;

        public MonitoringSet(string filePath)
        {
            //this.filePath = filePath;
        }


        public bool HasNewChanges
        {
            get => source.HasNewChanges;
            set => source.HasNewChanges = value;
        }

        public IList<MonitoringTarget> Targets 
        {
            get => source.Targets;
            set => source.Targets = value;
        }
        public DateTime Date 
        {
            get => source.Date;
            set => source.Date = value;
        }
        
        public void Dispose()
        {
            using (var writer = new StreamWriter(file))
                writer.Write(JsonConvert.SerializeObject(source));
            file.Dispose();
        }
    }
}