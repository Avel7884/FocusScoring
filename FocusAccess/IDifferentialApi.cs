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
            string GetValue(ApiMethodEnum method,IQueryComponents arg);

            string GetValue<TQuery>(ApiMethodEnum method,TQuery[] arg)
                where TQuery : IQueryComponents, new() //TODO Deserialization
            ;

            string GetValues(ApiMethodEnum method,IQueryComponents arg);
        }

    public class DifferentialApi : IDifferentialApi
    {
        private JsonAccess access;

        public DifferentialApi(FocusKey key)
        {
            access = key.Access;
        }
        
        public string GetValue(ApiMethodEnum method, IQueryComponents arg)
        {
            throw new System.NotImplementedException();
        }

        public string GetValue<TQuery>(ApiMethodEnum method, TQuery[] arg) where TQuery : IQueryComponents, new()
        {
            throw new System.NotImplementedException();
        }

        public string GetValues(ApiMethodEnum method, IQueryComponents arg)
        {
            throw new System.NotImplementedException();
        }
    }
}