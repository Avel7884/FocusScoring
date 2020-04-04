using System;
using System.Xml;
using FocusApiAccess.Methods;
using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess
{
    internal class SingleJsonMemoryCache : IJsonCache
    {
        private IQueryComponents qComponents;
        private string method;
        private bool cleared = false; 
        private object document; //TODO something

        public bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, out string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            /*if (!cleared && qComponents == args && method.Url == this.method)
            {
                document = this.document;
                return true;
            }

            document = new XmlDocument();*/
            json = default;
            return false;
        }

        public void Update<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            cleared = false;
            qComponents = args;
            this.method = method.Url;//TODO Make proper property
            document = json;
        }

        public void Clear<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            cleared = true;
        }
    }
}