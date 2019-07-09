using System;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        private class MarkerSubData
        {

            public MarkerSubData(Marker marker)
            {
                Marker = marker;
                Colour = ColourCode(marker.Colour);
                Description = marker.Desctiption;
            }
            
            public static MarkerSubData Create(Marker marker)=>
                new MarkerSubData(marker);
            
            public Marker Marker { get; }
            public string Colour { get; }
            public string Description { get; }
        }
        
        internal static string ColourCode(MarkerColour colour)
        {
            switch (colour)
            {
                case MarkerColour.Green: return "Ok";
                case MarkerColour.Red: return "X";
                case MarkerColour.Yellow: return "*";
                default: throw new AggregateException();
            }
        }
    }
}