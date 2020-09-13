namespace FocusAccess
{
    public class KzUrlArg : Query
    {
        public KzUrlArg(string query) : base(query)
        {}

        public KzUrlArg()
        {}

        public override string[] Keys { get; } = {"bin"};

    }
}