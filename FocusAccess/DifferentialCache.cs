using System.Collections.Generic;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace FocusAccess
{
    public interface IDifferencesStack
    {
        JToken Pop();
    }
    
    internal class DifferentialCache : IJsonCache
    {
        private readonly JsonFileSystemCache cache;
        private readonly Stack<JToken> stack = new Stack<JToken>();

        internal DifferentialCache(JsonFileSystemCache cache)
        {
            this.cache = cache;
        }
    
        public bool TryGetJson<TQuery>(ApiMethodEnum parameter, TQuery args, out string json) where TQuery : IQueryComponents
        {
            json = default;
            return false;
        }

        public void Update<TQuery>(ApiMethodEnum method, TQuery args, string json) where TQuery : IQueryComponents
        {
            if (!cache.TryGetJson(method, args, out var pastJson))
            {
                cache.Update(method,args,json);
                return;
            }

            var jdp = new JsonDiffPatch();
            JToken diff = jdp.Diff(json, pastJson);
            stack.Push(diff);
        }

        public void Clear<TQuery>(ApiMethodEnum method, TQuery args) where TQuery : IQueryComponents
        {
            throw new System.NotImplementedException();
        }
    }
}