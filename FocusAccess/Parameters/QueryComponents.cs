using System.Linq;

namespace FocusAccess
{
    public abstract class QueryComponents : IQueryComponents //TODO try make it just query
    {
        public QueryComponents(params string[] values)
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
    }
}