using System;
using System.Globalization;

namespace FocusApiAccess
{
    public class DatedQueryUrlArg : QueryComponents
    {

        public DatedQueryUrlArg(string query, DateTime date) 
            : base(query,date.ToString(CultureInfo.InvariantCulture))
        {}
        
        public override string[] Keys { get; } = {"q","date"};

    }
}