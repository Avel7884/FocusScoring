namespace FocusAccess
{
    public class ByUrlArg : Query , IQueryOf<UNP>
    {
        public ByUrlArg(UNP target) : base(target.ToString())
        {
            Target = target;
        }
        
        public override string[] Keys { get; } = {"unp"};
        public UNP Target { get; }
    }

    public class UNP
    {
        private string value;
        public static implicit operator UNP(string val) => new UNP{value = val};
        public override string ToString()=>value;
    }
}