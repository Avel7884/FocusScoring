using System.Linq;

namespace FocusAccess
{
    public class SkipableInnUrlArg : QueryComponents
    {

        public SkipableInnUrlArg(INN inn, int skip ) 
            : base(inn.ToString(), skip.ToString())
        {}

        public override string[] Keys { get; } = {"inn","skip" };
    }
}