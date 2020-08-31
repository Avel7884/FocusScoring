using System.Collections.Generic;
using System.Linq;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusAccess
{
    public class Api_Past
    {
        private readonly Api3 api3;

        internal Api_Past(Api3 api3)
        {
            this.api3 = api3;
        }

        public ReqValue[] Req(params InnUrlArg[] arg) =>
            api3.Req.MakeMultiRequest(arg);
        
        private TQuery UnifyQuery<TQuery>(TQuery[] query)
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