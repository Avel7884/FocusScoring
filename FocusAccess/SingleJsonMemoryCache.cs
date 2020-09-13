using System;
using System.Xml;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal class SingleJsonMemoryCache : IJsonCache
    {
        private IQuery q;
        private string method;
        private bool cleared = false; 
        private object document; //TODO something

        public bool TryGetJson<TQuery>(ApiMethodEnum method, TQuery args, out string json)
            where TQuery : IQuery
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
            where TQuery : IQuery
        {
            cleared = false;
            q = args;
            this.method = method.ToString();//.Url;//TODO Make proper property
            document = json;
        }

        public void Clear<TQuery>(ApiMethodEnum method, TQuery args)
            where TQuery : IQuery
        {
            cleared = true;
        }
    }
}