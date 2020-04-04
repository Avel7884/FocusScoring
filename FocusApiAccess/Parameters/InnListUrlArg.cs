namespace FocusApiAccess
{
    public class InnListUrlArg : QueryComponents, IPostArguments
    {
        public InnListUrlArg(params string[] inns)
        {
            Data = string.Join(",", inns);
        }

        public override string[] Keys { get; } = {};
        public string Data { get; }
    }
}