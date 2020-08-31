using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace FocusMonitoring
{
    public static class MonitoringFactory
    {
        public static IMonitoringSet OpenSet(string filePath) =>
            OpenRelivingSet(filePath);
        
        internal static IRelivingChangesMonitoringSet OpenRelivingSet(string filePath)
        {
            var file = AwaitFile(filePath);
            using var reader = new StreamReader(file);
            return JsonConvert.DeserializeObject<IRelivingChangesMonitoringSet>(reader.ReadToEnd());   
        } 
        
        private static FileStream AwaitFile(string filePath)
        {
            Exception exception = default;
            for(var i =0;i<10;i++)
            {
                try
                {
                    return File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException e)
                {
                    exception = e;
                    Thread.Sleep (100);
                }
            }
            throw exception ?? throw new Exception("Be i damned if that happened!");
        }
        
        internal 
    }
}