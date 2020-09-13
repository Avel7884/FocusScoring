namespace FocusAccess
{
    public class QueryUrlArg : Query //TODO rename to fio
    {
        public QueryUrlArg(string fio) : base(fio) {}

        public QueryUrlArg() : base("")
        {}

        public override string[] Keys { get; } = {"fio"};
        public override string MakeAlias()
        {
            return Values[0].Replace(" ", "");
        }
    }
}