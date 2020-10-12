/*using System.Collections.Generic;
using System.Linq;

namespace FocusAccess.ResponseClasses
{
    public class MultiQuery : IQuery
    {
        public  MultiQuery(IQuery[] queries)
        {
            
        }
        
        private static TQuery UnifyQuery<TQuery>(TQuery[] query) 
            where TQuery : Query, new()
        {
            var args = query[0].Values.Select(x => new List<string> {x}).ToArray();
            foreach (var q in query.Skip(1))
                for (int i = 0; i < args.Length; i++)
                    args[i].Add(q.Values[i]);
            return new TQuery{Values = args.Select(x=>string.Join(",",x)).ToArray()};
        }

        public string AssembleQuery()
        {
            throw new System.NotImplementedException();
        }

        public string[] Keys { get; }
        public string[] Values { get; }
        public string MakeAlias()
        {
            throw new System.NotImplementedException();
        }
    }
}*/