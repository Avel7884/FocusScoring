using System.Collections.Generic;
using System.Xml;
using FocusApiAccess.Methods;
using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess
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

        public bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, out string json)
            where TData : IParameterValue where TQuery : IQueryComponents
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
        
        //TODO manage
        /*public void Clear<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args)
            where TData : IParameterValue, new() where TQuery : IQueryComponents
        {
            foreach (var cache in caches)
                cache.Clear<TData,TQuery>(method,args);
        }*/
    }
}