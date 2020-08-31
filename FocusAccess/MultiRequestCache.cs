namespace FocusAccess
{
    public class MultiRequestCache : IJsonCache
    {
        private readonly JsonFileSystemCache cache;

        internal MultiRequestCache(JsonFileSystemCache cache)
        {
            this.cache = cache;
        }

        public bool TryGetJson<TQuery>(ApiMethodEnum parameter, TQuery args, out string json) where TQuery : IQueryComponents
        {
            throw new System.NotImplementedException();
        }

        public void Update<TQuery>(ApiMethodEnum method, TQuery args, string json) where TQuery : IQueryComponents
        {
            throw new System.NotImplementedException();
        }

        public void Clear<TQuery>(ApiMethodEnum method, TQuery args) where TQuery : IQueryComponents
        {
            throw new System.NotImplementedException();
        }
    }
}