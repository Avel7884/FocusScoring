namespace FocusApiAccess
{
    public interface IQueryComponents //TODO try make constrains on alias with abstract class
    {
        string AssembleQuery();
        string[] Keys { get; }
        string[] Values { get; }
        string MakeAlias();
    }
}  