/*using System;
using System.Xml;
using FocusApiAccess.ParameterValues;

namespace FocusApiAccess.Methods
{
    public class ParamQueryTransfer<TData,TQuery> where TData : IParameterValue , new() where TQuery : IQueryComponents
    {
        private readonly ApiParameter<TData> parameter;
        private readonly XmlAccess access;

        internal ParamQueryTransfer(ApiParameter<TData> parameter, XmlAccess access)
        {
            this.parameter = parameter;
            this.access = access;
        }

        public TData MakeRequest(TQuery query)
        {
            var data = access.TryGetXml(parameter,query,out var obj);
            switch (parameter.Type)
            {
                case ApiParameterType.Single:
                    data.InitializeFrom(obj.SelectSingleNode(parameter.Path));
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
            return data;
        }
    }
}*/