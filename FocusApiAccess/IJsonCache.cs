using System.Xml;
using FocusAccess.Methods;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal interface IJsonCache : IJsonAccess
    {
        void Update<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, string json)
            where TData : IParameterValue where TQuery : IQueryComponents;
        void Clear<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args)
            where TData : IParameterValue where TQuery : IQueryComponents;
    }
}