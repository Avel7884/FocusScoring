using System.Collections.Generic;
using System.Linq;
using FocusApiAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusApiAccess.Methods
{
    internal class ApiMultiValueMethod<TData, TQuery> : 
        ApiMethod<TData, TQuery>,
        IApiMultiValueMethod<TData, TQuery> 
        where TData : IParameterValue 
        where TQuery : IQueryComponents
    {

        protected internal ApiMultiValueMethod(IJsonAccess access, IJsonAccess forcedAccess, string url, bool discCache = true) : 
            base(access, forcedAccess, url, discCache)
        {}

        public TData[] MakeRequest(TQuery query,bool forced = false)
        {
            return JsonConvert.DeserializeObject<IList<TData>>(
                (forced ? access : forcedAccess).TryGetJson(this, query, out var obj) ? obj : default,Converter.Settings).ToArray();
        }
    }
}