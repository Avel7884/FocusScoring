using FocusAccess.ResponseClasses;

namespace FocusAccess.Methods
{
    internal interface IApiMethod<out TData, in TQuery>
        where TData : IParameterValue
        where TQuery : IQueryComponents
    {
        string MakeAlias();
        string Url { get; }
        bool DiscCache { get; }
    }
}