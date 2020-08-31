using System.Linq;

namespace FocusAccess
{
    public class InnListUrlArg : QueryComponents, IPostArguments
    {
        public InnListUrlArg(params string[] inns)
        {
            Data = string.Join(",", inns);
        }
        public InnListUrlArg(params InnUrlArg[] inns)
        {
            Data = string.Join(",", inns.Select(x=>x.Values[0]));
        }

        public override string[] Keys { get; } = {};
        public string Data { get; }
    }
}