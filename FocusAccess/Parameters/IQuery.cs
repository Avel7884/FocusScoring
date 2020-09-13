namespace FocusAccess
{
    public interface IQuery
    {
        string AssembleQuery();
        string[] Keys { get; }
        string[] Values { get; }
        string MakeAlias();
    }

    public interface IQueryOf<out TTarget> : IQuery
    {
        TTarget Target { get; }
    }
}  