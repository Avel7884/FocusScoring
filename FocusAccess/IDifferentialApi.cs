using Newtonsoft.Json.Linq;

namespace FocusAccess
{
    public interface IDifferentialApi
    {
            /*IDifferencesStack DifferencesStack { get; }
            string Req(InnUrlArg arg);
            string Req(InnUrlArg[] arg);
            string Analytics(InnUrlArg arg);
            string Analytics(InnUrlArg[] arg);
            string ByCompanyDetails(ByUrlArg arg);
            string ByCompanyDetails(ByUrlArg[] arg);
            string KzCompanyDetails(KzUrlArg arg);
            string KzCompanyDetails(KzUrlArg[] arg);
            string EgrDetails(InnUrlArg arg);
            string EgrDetails(InnUrlArg[] arg);
            string EgrDetailsMon(DateUrlArg date);
            string ReqMon(DateUrlArg date);
            string Stat();*/
            string GetValue(ApiMethodEnum method,IQuery arg);

            string GetValue<TQuery>(ApiMethodEnum method,TQuery[] arg)
                where TQuery : IQuery, new() //TODO Deserialization
            ;

            string GetValues(ApiMethodEnum method,IQuery arg);
        }

    public class DifferentialApi : IDifferentialApi
    {
        private JsonAccess access;

        public DifferentialApi(FocusKey key)
        {
            access = key.DifferentialAccess;
        }
        
        public string GetValue(ApiMethodEnum method, IQuery arg)
        {
            return access.TryGetJson(method, arg, out var json) ? json : default;
        }

        public string GetValue<TQuery>(ApiMethodEnum method, TQuery[] arg) where TQuery : IQuery, new()
        {
            throw new System.NotImplementedException();
        }

        public string GetValues(ApiMethodEnum method, IQuery arg)
        {
            throw new System.NotImplementedException();
        }
    }
}