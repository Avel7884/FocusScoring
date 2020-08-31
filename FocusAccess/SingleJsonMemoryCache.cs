using System;
using System.Xml;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal class SingleJsonMemoryCache : IJsonCache
    {
        private IQueryComponents qComponents;
        private string method;
        private bool cleared = false; 
        private object document; //TODO something

        public bool TryGetJson<TQuery>(ApiMethodEnum method, TQuery args, out string json)
            where TQuery : IQueryComponents
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

        public void Update<TQuery>(ApiMethodEnum method, TQuery args, string json)
            where TQuery : IQueryComponents
        {
            cleared = false;
            qComponents = args;
            this.method = method.ToString();//.Url;//TODO Make proper property
            document = json;
        }

        public void Clear<TQuery>(ApiMethodEnum method, TQuery args)
            where TQuery : IQueryComponents
        {
            cleared = true;
        }
    }
}