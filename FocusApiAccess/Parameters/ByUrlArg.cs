namespace FocusApiAccess
{
    public class ByUrlArg : QueryComponents
    {
        public ByUrlArg(string query) : base(query)
        {}

        public ByUrlArg() {}
        public override string[] Keys { get; } = {"unp"};
    }
}