using System.Linq;

namespace FocusAccess
{
    public abstract class SingleQuerys : IQuery
    {
        private const string filter = " /\\?:*\"\'<>|";
        protected readonly string query;
        protected abstract string key { get; }

        public SingleQuerys(string query)
        {
            this.query = query;
        }
        
        public virtual string AssembleQuery()
        {
            return key + "=" + query;
        }

        public string[] Keys => new[] {key};// TODO Maybe make static-ish 
        public string[] Values => new[] {query}; 

        public virtual string MakeAlias()
        {
            return string.Join("",query.Where(x=>!filter.Contains(x)));
        }
    }
}