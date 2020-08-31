namespace FocusAccess
{
    public class InnflUrlArg : QueryComponents
    {
        public InnflUrlArg(string query) : base(query) {}
        public override string[] Keys { get; } = {"innfl"};
    }
}