using System;

namespace FocusScoring
{
    public class Marker
    {
        private readonly Func<bool> check;
        //TODO manage access
        public int Score { get; set; }

        public Marker(string Name, MarkerColour colour, string desctiption, int Score, Func<bool> check)
        { 
            this.check = check;
            this.Name = Name;
            Colour = colour;
            Desctiption = desctiption;
        }
        
        public string Name { get; }
        public MarkerColour Colour { get;}
        public string Desctiption { get; }

        public bool Check() => check();
    }

    public enum MarkerColour
    {
        Green,Yellow,Red,RedAffiliates,GreenAffiliates,YellowAffiliates
    }
}