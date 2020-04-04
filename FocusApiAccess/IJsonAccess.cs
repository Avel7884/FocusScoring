using System.Xml;
using FocusApiAccess.Methods;
using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess
{
    internal interface IJsonAccess //TODO rename
    {
        bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> parameter, TQuery args, out string json) //TODO rename
            where TData : IParameterValue where TQuery : IQueryComponents;
    }
}