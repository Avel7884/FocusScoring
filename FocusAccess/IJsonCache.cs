using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal interface IJsonCache : IJsonAccess
    {
        void Update<TQuery>(ApiMethodEnum method, TQuery args, string json)
            where TQuery : IQueryComponents;
        void Clear<TQuery>(ApiMethodEnum method, TQuery args)
           where TQuery : IQueryComponents;
    }
}