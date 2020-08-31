using FocusAccess.ResponseClasses;

namespace FocusAccess.Methods
{
    public interface IApiMultiRequestMethod <out TData, in TQuery> :
        IApiSingleValueMethod<TData, TQuery>
        where TData : IParameterValue
        where TQuery : IQueryComponents, new()
    {
        TData[] MakeMultiRequest(params TQuery[] query);
    }
}