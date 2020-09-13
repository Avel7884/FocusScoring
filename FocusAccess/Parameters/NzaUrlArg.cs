namespace FocusAccess
{
    public class NzaUrlArg : Query
    {
        public NzaUrlArg(string query)
        {}

        public NzaUrlArg()
        {}

        public override string[] Keys { get; } = {"nza"};
    }
}