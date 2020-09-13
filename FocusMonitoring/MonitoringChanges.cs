using System;

namespace FocusMonitoring
{
    public class MonitoringChanges<TResultValue>
    {
        
        public MonitoringChange[] Changes { get; }

        public string[] GetChanges()
        {
            throw new NotImplementedException();
        }
    }
    
    public class MonitoringChange
    {
        public MonitoringChange(DateTime date, string diff )
        {
            Date = date;
            Diff = diff;
        }
        
        public DateTime Date { get; }
        public string Diff { get; }
    }
}