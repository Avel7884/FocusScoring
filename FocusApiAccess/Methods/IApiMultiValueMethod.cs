using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess.Methods
{
    public interface IApiMultiValueMethod <out TData, in TQuery>
        where TData : IParameterValue
        where TQuery : IQueryComponents
    {
        TData[] MakeRequest(TQuery query,bool forced = false);
    }
}