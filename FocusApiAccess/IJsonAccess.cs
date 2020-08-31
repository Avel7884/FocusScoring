using System.Xml;
using FocusAccess.Methods;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal interface IJsonAccess //TODO rename
    {
        bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> parameter, TQuery args, out string json) //TODO rename
            where TData : IParameterValue where TQuery : IQueryComponents;
    }
}