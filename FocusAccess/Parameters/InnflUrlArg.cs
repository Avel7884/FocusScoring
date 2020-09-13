namespace FocusAccess
{
    public class InnflUrlArg : Query
    {
        public InnflUrlArg(string query) : base(query) {}
        public override string[] Keys { get; } = {"innfl"};
    }
}