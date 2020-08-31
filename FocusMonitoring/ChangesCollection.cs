using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Timers;

namespace FocusMonitoring
{
    public interface IChangesCollection : IReadOnlyCollection<MonitoringChange>, INotifyCollectionChanged//, IDisposable
    {
    }
    
    public class ChangesCollection : IReadOnlyCollection<MonitoringChange>
    {
        private readonly string path;
        /*private readonly IMonitorer monitorer;

        public ChangesCollection(IMonitorer monitorer, DateTime? updateTime = null)
        {
            this.monitorer = monitorer;
            var timerTime = (updateTime ?? DateTime.Today.AddHours(14)) - DateTime.Now;
            if (timerTime < TimeSpan.Zero)
                monitorer.PerformMonitoring();
            else
                new Timer(timerTime.TotalMilliseconds).Elapsed += (s, a) =>
                    monitorer.PerformMonitoring();
        }*/
        public ChangesCollection(string path = "./") => 
            this.path = path;

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public IEnumerator<MonitoringChange> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}