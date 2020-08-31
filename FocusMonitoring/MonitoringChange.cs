using System;

namespace FocusMonitoring
{
    public class MonitoringChange
    {
        public DateTime Date { get; }
        public string[] ChangedField { get; }
        public Type FieldType { get; } //removal is possible 
        public string PastValue { get; }
        public string NewValue { get; }
    }
}