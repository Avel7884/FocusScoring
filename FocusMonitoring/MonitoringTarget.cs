using FocusAccess;

namespace FocusMonitoring
{
    public class MonitoringTarget 
    {
        public MonitoringTarget(ApiMethodEnum method, IQuery target)
        {
            Target = target;
            Method = method;
        }
        
        public IQuery Target { get; set; }
        public ApiMethodEnum Method { get; set; }

        public string MakeFileName()
        {
            return string.Join(",", Target.Values) + '.' + Method;
        }
    }
}