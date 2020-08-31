using System;
using System.Collections.Generic;

namespace FocusMonitoring
{
    public interface IMonitoringSet: IDisposable
    {
        bool HasNewChanges { get; }
        IList<MonitoringTarget> Targets { get;}
        DateTime Date { get; }
    }

    internal interface IRelivingChangesMonitoringSet : IMonitoringSet
    {
        new bool HasNewChanges { get; set; }
        new IList<MonitoringTarget> Targets { get; set; }
        new DateTime Date { get; set; }
    }
}