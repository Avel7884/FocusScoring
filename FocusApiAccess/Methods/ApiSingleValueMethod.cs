using System;
using System.Collections.Generic;
using FocusApiAccess.ResponseClasses;
using Newtonsoft.Json;

namespace FocusApiAccess.Methods
{
    internal class ApiSingleValueMethod<TData, TQuery> : 
        ApiMethod<TData,TQuery>,
        IApiSingleValueMethod<TData, TQuery>
        where TData : IParameterValue 
        where TQuery : IQueryComponents
    {
        
        public TData MakeRequest(TQuery query,bool forced = false)
        {
            return JsonConvert.DeserializeObject<IList<TData>>(
                (forced ? forcedAccess : access).TryGetJson(this, query, out var obj) ? obj : default,Converter.Settings)[0];
            switch (ApiParameterType.Single)
            {
                case ApiParameterType.Single:
                    //data.InitializeFrom(obj.SelectSingleNode(parameter.Path));
                    break;
                case ApiParameterType.Multiple:
                    throw new NotImplementedException();    
                    break;
                case ApiParameterType.LastNodeMatch:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return default;//obj;
        }

        protected internal ApiSingleValueMethod(IJsonAccess access, IJsonAccess forcedAccess,  string url, bool discCache = true) : 
            base(access, forcedAccess, url, discCache)
        {
        }
    }
}