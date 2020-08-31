using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusAccess
{
    public interface IApi3
    {
        //IDifferencesStack DifferencesStack { get; }
        ReqValue Req(InnUrlArg arg);
        ReqValue[] Req(InnUrlArg[] arg);
        ByCompanyDetailsValue ByCompanyDetails(ByUrlArg arg);
        ByCompanyDetailsValue[] ByCompanyDetails(ByUrlArg[] arg);
        KzCompanyDetailsValue KzCompanyDetails(KzUrlArg arg);
        KzCompanyDetailsValue[] KzCompanyDetails(KzUrlArg[] arg);
        EgrDetailsValue EgrDetails(InnUrlArg arg);
        EgrDetailsValue[] EgrDetails(InnUrlArg[] arg);
        MonValue[] EgrDetailsMon(DateUrlArg date);
        MonValue[] ReqMon(DateUrlArg date);
        StatValue[] Stat();
        IParameterValue GetValue(ApiMethodEnum method,QueryComponents arg);

        IParameterValue[] GetValue<TQuery>(ApiMethodEnum method,TQuery[] arg)
            where TQuery : QueryComponents, new() //TODO Deserialization
        ;

        IParameterValue[] GetValues(ApiMethodEnum method,QueryComponents arg);
    }

    public class Api : IApi3
    {
        private readonly IJsonAccess access;
        //private readonly IJsonAccess forcedAccess;

        internal Api(IJsonAccess access)//, IJsonAccess forcedAccess)
        {
            this.access = access;
            //this.forcedAccess = forcedAccess;    
        }

        public Api(FocusKey key)
        {
            
            access = key.Access;  
        }
        
        public IDifferencesStack DifferencesStack { get; }

        public ReqValue Req(InnUrlArg arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<ReqValue>(json,Converter.Settings);
        }
        public ReqValue[] Req(InnUrlArg[] arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, UnifyQuery(arg), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<ReqValue>>(json,Converter.Settings).ToArray();
        }
        public ByCompanyDetailsValue ByCompanyDetails(ByUrlArg arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<ByCompanyDetailsValue>(json,Converter.Settings);
        }
        public ByCompanyDetailsValue[] ByCompanyDetails(ByUrlArg[] arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, UnifyQuery(arg), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<ByCompanyDetailsValue>>(json,Converter.Settings).ToArray();
        }
        public KzCompanyDetailsValue KzCompanyDetails(KzUrlArg arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<KzCompanyDetailsValue>(json,Converter.Settings);
        }
        public KzCompanyDetailsValue[] KzCompanyDetails(KzUrlArg[] arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, UnifyQuery(arg), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<KzCompanyDetailsValue>>(json,Converter.Settings).ToArray();
        }
        public EgrDetailsValue EgrDetails(InnUrlArg arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<EgrDetailsValue>(json,Converter.Settings);
        }
        public EgrDetailsValue[] EgrDetails(InnUrlArg[] arg)
        {
            string json = access.TryGetJson(ApiMethodEnum.req, UnifyQuery(arg), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<EgrDetailsValue>>(json,Converter.Settings).ToArray();
        }

        public MonValue[] EgrDetailsMon(DateUrlArg date)
        {
            throw new NotImplementedException();
        } 
        
        public MonValue[] ReqMon(DateUrlArg date)
        {
            throw new NotImplementedException();
        } 
        
        
        public StatValue[] Stat()
        {
            throw new NotImplementedException();
        }

        public IParameterValue GetValue(ApiMethodEnum method,QueryComponents arg)
        {
            string json = access.TryGetJson(method, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IParameterValue>(json, Converter.Settings);
        }
        
        public IParameterValue[] GetValue<TQuery>(ApiMethodEnum method,TQuery[] arg)
            where TQuery : QueryComponents, new() //TODO Deserialization
        {
            string json = access.TryGetJson(method, UnifyQuery(arg), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<IParameterValue>>(json,Converter.Settings).ToArray();
        }
        
        public IParameterValue[] GetValues(ApiMethodEnum method,QueryComponents arg)
        {
            string json = access.TryGetJson(method, arg, out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IParameterValue[]>(json, Converter.Settings);
        }
        
        private TQuery UnifyQuery<TQuery>(TQuery[] query) // TODO move to TQuery class 
            where TQuery : QueryComponents, new()
        {
            var args = query[0].Values.Select(x => new List<string> {x}).ToArray();
            foreach (var q in query.Skip(1))
                for (int i = 0; i < args.Length; i++)
                    args[i].Add(q.Values[i]);
            return new TQuery{Values = args.Select(x=>string.Join(",",x)).ToArray()};
        }
    }
}