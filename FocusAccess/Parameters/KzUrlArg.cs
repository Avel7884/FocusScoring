namespace FocusAccess
{
    public class KzUrlArg : QueryComponents
    {
        public KzUrlArg(string query) : base(query)
        {}

        public KzUrlArg()
        {}

        public override string[] Keys { get; } = {"bin"};

    }
}