using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal interface IJsonAccess //TODO rename
    {
        bool TryGetJson<TQuery>(ApiMethodEnum parameter, TQuery args, out string json) //TODO rename
            where TQuery : IQuery;
    }
}