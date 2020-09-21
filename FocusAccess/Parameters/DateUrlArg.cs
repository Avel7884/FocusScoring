using System;
using System.Globalization;

namespace FocusAccess
{
    public class DateUrlArg : Query
    {
        public DateUrlArg(DateTime query) 
            : base(query.ToString("yyyy-mm-dd",CultureInfo.InvariantCulture))
        {}

        public override string[] Keys { get; } = {"date"};
        public static implicit operator DateUrlArg(DateTime date) => 
            new DateUrlArg(date);
    }
}