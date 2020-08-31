using System.Collections.Generic;
using System.Linq;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusAccess.Methods
{
    internal class ApiMultiRequestMethod<TData,TQuery> : 
        ApiSingleValueMethod<TData,TQuery> ,
        IApiMultiRequestMethod<TData,TQuery>
        where TData : IParameterValue
        where TQuery : QueryComponents, new()
    {
        protected internal ApiMultiRequestMethod(IJsonAccess access, IJsonAccess forcedAccess, string url, bool b = true) 
            : base(access,forcedAccess , url, b)
        {
        }

        public TData[] MakeMultiRequest(params TQuery[] query)
        {
            var json = access.TryGetJson(this, UnifyQuery(query), out var obj) ? obj : default;
            return JsonConvert.DeserializeObject<IList<TData>>(json,Converter.Settings).ToArray();
        }
        
        /*
        public TData MakeRequest(TQuery query)
        {
            return JsonConvert.DeserializeObject<IList<TData>>(
                access.TryGetJson(this, query, out var obj) ? obj : default)[0];
        }*/

        private TQuery UnifyQuery(TQuery[] query)
        {
            var args = query[0].Values.Select(x => new List<string> {x}).ToArray();
            foreach (var q in query.Skip(1))
                for (int i = 0; i < args.Length; i++)
                    args[i].Add(q.Values[i]);
            return new TQuery{Values = args.Select(x=>string.Join(",",x)).ToArray()};
        }
    }
}