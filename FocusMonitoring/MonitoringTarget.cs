using FocusAccess;

namespace FocusMonitoring
{
    public class MonitoringTarget 
    {
        public MonitoringTarget(ApiMethodEnum method, IQueryComponents target)
        {
            Target = target;
            Method = method;
        }
        
        public IQueryComponents Target { get; set; }
        public ApiMethodEnum Method { get; set; }

        public string MakeFileName()
        {
            return string.Join(",", Target.Values) + '.' + Method;
        }
    }
}