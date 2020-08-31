using System.Collections.Generic;

namespace FocusAccess
{
    public class PassportsUrlArg : QueryComponents
    {
        public PassportsUrlArg(string[] query) 
            : base(string.Join(",",query)) {}

        public PassportsUrlArg()
        {}

        public override string[] Keys { get; } = {"passportNumber"};
    }
}