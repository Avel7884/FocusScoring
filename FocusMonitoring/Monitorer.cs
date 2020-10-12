using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FocusAccess;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusMonitoring
{/*
    public interface IMonitorer
    {
        void PutOnMonitor(ApiMethodEnum method, IQueryComponents correspondingComponent);
        void RemoveFromMonitor(ApiMethodEnum method, IQueryComponents correspondingComponent);
    }*/

    public interface IMonitorer
    {
        void PerformMonitoring();
    }

    public class Monitorer : IMonitorer
    {
        private readonly IApi3 api;
        private readonly IDifferentialApi differentialApi;
        private readonly MonitoringFactory factory; //TODO access with interface
        private readonly string monitoringFolder;
        private readonly string monitoringSetFile;

        private MonValue[] reqMon;
        private MonValue[] egrMon;
        
        /*private readonly IReadOnlyCollection<MonitoringChange> onMonitoringPast;
        private readonly IReadOnlyCollection<MonitoringChange> onMonitoring;*/
        //private readonly IMonitoringSet monitoringSet;

        public Monitorer(IApi3 api, IDifferentialApi differentialApi, MonitoringFactory factory, bool enableShortLog = true)//,string onMonitoringPastFile = "OnMonitoringPast")
        {
            this.api = api;/*
            onMonitoringPast = new ChangesCollection(monitoringFolder + onMonitoringPastFile);
            onMonitoring = new ChangesCollection(monitoringFolder + onMonitoringFile);*/
            this.differentialApi = differentialApi;
            this.factory = factory;
            //this.monitoringFolder = monitoringFolder;
            //this.monitoringSetFile = monitoringSetFile;
            //monitoringSet = new MonitoringSet(monitoringFolder + monitoringSetFile);
        }

        public void PerformMonitoring()
        {
            //var monitoringSet = JsonConvert.DeserializeObject<MonitoringSet>(File.ReadAllText(monitoringFolder + monitoringSetFile));
            using var monitoringSet = factory.OpenRelivingSet();
            EnsureMonitoringList(monitoringSet);

            var updateTime = DateTime.Today + TimeSpan.FromHours(9);
            if (DateTime.UtcNow > updateTime || monitoringSet.Date.ToUniversalTime() > updateTime)
                return;// new MonitoringResult[0];
            
            reqMon = api.ReqMon(monitoringSet.Date);
            egrMon = api.EgrDetailsMon(monitoringSet.Date);
            string diff;
            var targets = new List<MonitoringResult>();
            
            foreach (var t in monitoringSet.Targets)
                if((diff = TryExtractDifference(t)) != "")
                { //TODO non interruptible region needed
                    using (var file = File.AppendText(t.MakeFileName()))
                        file.WriteLine(DateTime.Today + " " + diff);
                    targets.Add(new MonitoringResult{Target = t, Changes = diff});
                }

            monitoringSet.HasNewChanges = false;
            monitoringSet.Date = DateTime.Now;
            factory.WriteShortLog(targets);
        }

        private string TryExtractDifference(MonitoringTarget target) => //To avoid using braces in switch
            target.Method switch
            {
                ApiMethodEnum.req => (reqMon.Any(m => m.Ogrn == target.Target.Values[0])
                    ? differentialApi.GetValue(target.Method,target.Target as InnUrlArg)
                    : ""),
                ApiMethodEnum.egrDetails => (reqMon.Any(m => m.Ogrn == target.Target.Values[0])
                    ? differentialApi.GetValue(target.Method, target.Target as InnUrlArg)
                    : ""),
                _ => differentialApi.GetValue(target.Method, target.Target)
            };

        private void EnsureMonitoringList(IRelivingChangesMonitoringSet monitoringSet)
        {
            /*var ad = onMonitoringPast.ToHashSet();
            var sa = onMonitoring.ToHashSet();
            if(onMonitoringPast.Zip(onMonitoring, (x,y)=>x==y).All(x=>x))
                api.GetValue(ApiMethodEnum.mon,)*/
            if (monitoringSet.HasNewChanges)
                api.GetValue(ApiMethodEnum.monList,
                    new InnListUrlArg(monitoringSet.Targets
                        .Where(t => t.Method == ApiMethodEnum.req || t.Method == ApiMethodEnum.egrDetails)
                        .Select(t => t.Target as InnUrlArg)
                        .ToArray()));
            monitoringSet.HasNewChanges = false;
        }
    }
}