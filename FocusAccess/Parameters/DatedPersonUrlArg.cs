using System;
using System.Globalization;

namespace FocusAccess
{
    public class DatedPersonUrlArg : Query //TODO Проблема!
    {
        public DatedPersonUrlArg(string innfl, string fio, DateTime date)
            :base(innfl,fio,date.ToString(CultureInfo.InvariantCulture)){}

        public DatedPersonUrlArg() : base("","","")
        {}

        public override string[] Keys { get; } = {"innfl", "fio", "date"};
    }
}