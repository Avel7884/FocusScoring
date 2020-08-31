using System;
using System.Globalization;

namespace FocusAccess
{
    public class DatedQueryUrlArg : QueryComponents
    {

        public DatedQueryUrlArg(string query, DateTime date) 
            : base(query,date.ToString(CultureInfo.InvariantCulture))
        {}
        
        public override string[] Keys { get; } = {"q","date"};

    }
}