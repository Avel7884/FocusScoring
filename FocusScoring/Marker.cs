using System;

namespace FocusScoring
{
    public class Marker
    {
        private readonly Func<bool> check;

        public Marker(string Name,string desctiption, Func<bool> check)
        {
            this.check = check;
            this.Name = Name;
            Desctiption = desctiption;
        }
        
        public string Name { get; }
        public string Desctiption { get; }

        public bool Check() => check();
    }
}