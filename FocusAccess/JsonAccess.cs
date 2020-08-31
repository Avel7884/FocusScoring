using System.Collections.Generic;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal class JsonAccess : IJsonAccess
    {
        private readonly List<IJsonCache> caches;
        private readonly IJsonAccess source;

        internal JsonAccess(List<IJsonCache> caches,IJsonAccess source)
        {
            this.caches = caches;
            this.source = source;
        }

        public bool TryGetJson< TQuery>(ApiMethodEnum method, TQuery args, out string json)
            where TQuery : IQueryComponents
        {
            for (int i = 0; i < caches.Count; i++)
                if (caches[i].TryGetJson(method,args, out json))
                {
                    for (int j = 0; j < i; j++)
                        caches[j].Update(method,args, json);
                    return true;
                }

            if (!source.TryGetJson(method,args, out json)) 
                return false;
            foreach (var cache in caches)
                cache.Update(method,args , json);
            return true;
        }
    }
}