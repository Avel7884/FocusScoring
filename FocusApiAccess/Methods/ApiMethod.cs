using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess.Methods
{

    internal abstract class ApiMethod<TData,TQuery> : IApiMethod <TData,TQuery> where TData : IParameterValue
        where TQuery : IQueryComponents
    {
        protected readonly IJsonAccess access;
        protected readonly IJsonAccess forcedAccess;

        public string MakeAlias()
        {
            return Url.Replace("/", "");
        }

        protected internal ApiMethod(IJsonAccess access, IJsonAccess forcedAccess, string url, bool discCache = true)
        {
            this.access = access;
            this.forcedAccess = forcedAccess;
            Url = url;
            DiscCache = discCache;
        }

        public string Url { get; }
        public bool DiscCache { get; }
    }
}