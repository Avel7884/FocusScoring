using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess.Methods
{
    public interface IApiMultiRequestMethod <out TData, in TQuery> :
        IApiSingleValueMethod<TData, TQuery>
        where TData : IParameterValue
        where TQuery : IQueryComponents, new()
    {
        TData[] MakeMultiRequest(params TQuery[] query);
    }
}