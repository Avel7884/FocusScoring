using System.Collections.Generic;
using System.Linq;

namespace FocusAccess
{
    public abstract class Query : IQuery //TODO try make it just query
    {
        public Query(params string[] values)
        {
            Values = values;
        }
        
        public string AssembleQuery()
        {
            return string.Join("&", Keys.Zip(Values, (x, y) => x + "=" + y));
        }

        public abstract string[] Keys { get; }
        public string[] Values { get; set; } //TODO no abstract
        public virtual string MakeAlias()
        {
            return string.Join("_", Values);
        }
        
        public static TQuery Unify<TQuery>(IEnumerable<TQuery> query) // TODO move to TQuery class 
            where TQuery : Query, new()
        {
            using var enumerator = query.GetEnumerator();
            if(!enumerator.MoveNext()) return new TQuery();
            var args = enumerator.Current.Values.Select(x => new List<string> {x}).ToArray();
            while (enumerator.MoveNext())
                for (int i = 0; i < args.Length; i++)
                    args[i].Add(enumerator.Current.Values[i]);
            return new TQuery{Values = args.Select(x=>string.Join(",",x)).ToArray()};
        }
    }
}