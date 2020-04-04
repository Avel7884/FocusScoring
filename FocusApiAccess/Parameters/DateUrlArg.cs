using System;
using System.Globalization;

namespace FocusApiAccess
{
    public class DateUrlArg : QueryComponents
    {
        public DateUrlArg(DateTime query) 
            : base(query.ToString(CultureInfo.InvariantCulture))
        {}

        public override string[] Keys { get; } = {"date"};
    }
}