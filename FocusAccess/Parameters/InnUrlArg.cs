namespace FocusAccess
{
    public class InnUrlArg : Query
    {
        public INN Inn { get; }
        
        public InnUrlArg(INN inn) : base(inn.ToString()) => Inn = inn;

        public InnUrlArg() : base("")
        {}

        public override string[] Keys { get; } = {"inn"};

        public static implicit operator InnUrlArg(INN inn)
        {
            return new InnUrlArg(inn);
        }
    }
}