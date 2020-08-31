namespace FocusAccess
{
    public class InnUrlArg : QueryComponents
    {

        public InnUrlArg(INN inn) : base(inn.ToString())
        {}

        public InnUrlArg() : base("")
        {}

        public override string[] Keys { get; } = {"inn"};

        public static implicit operator InnUrlArg(INN inn)
        {
            return new InnUrlArg(inn);
        }
    }
}