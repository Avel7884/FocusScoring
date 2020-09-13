using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace FocusMonitoring
{
    public class MonitoringSet : IRelivingChangesMonitoringSet
    {
        [JsonIgnore]
        private readonly MonitoringFactory factory;
        //private FileStream file;
        //private IRelivingChangesMonitoringSet source;

        internal MonitoringSet(MonitoringFactory factory)
        {
            this.factory = factory;
            //this.filePath = filePath;
        }
/*

        [JsonIgnore]
        public string FilePath { get; set; }
        */

        public bool HasNewChanges
        {
            get;
            set;
        }

        public IList<MonitoringTarget> Targets 
        {
            get;
            set;
        }
        
        public DateTime Date 
        {
            get;
            set;
        }

        public void Dispose() =>
            factory.SaveSet(this);
    }
}