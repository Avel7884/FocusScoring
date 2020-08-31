using System.Collections.Generic;
using FocusAccess.Methods;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal class ForcedJsonAccess : IJsonAccess
    {
        private readonly List<IJsonCache> caches;
        private readonly IJsonAccess source;

        internal ForcedJsonAccess(List<IJsonCache> caches,IJsonAccess source)
        {
            this.caches = caches;
            this.source = source;
        }
        
        public bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, out string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            if (!source.TryGetJson(method, args, out json)) 
                return false;
            foreach (var cache in caches)
                cache.Update(method,args,json);
            return true;
        }
    }
}