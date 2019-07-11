using System;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        internal class MarkerSubData
        {

            public MarkerSubData(Marker marker)
            {
                Marker = marker;
                Colour = ColourCode(marker.Colour);
                Description = marker.Desctiption;
                Name = marker.Name;
            }
            
            public static MarkerSubData Create(Marker marker)=>
                new MarkerSubData(marker);
            
            public Marker Marker { get; }
            public string Colour { get; }
            public string Name { get; }
            public string Description { get; }
        }
        
        private static string ColourCode(MarkerColour colour)
        {
            switch (colour)
            {
                case MarkerColour.Green: return "Ok";
                case MarkerColour.Red: return "X";
                case MarkerColour.Yellow: return "*";
                case MarkerColour.GreenAffiliates: return "OkOk";
                case MarkerColour.RedAffiliates: return "XX";
                case MarkerColour.YellowAffiliates: return "**";
                default: throw new AggregateException();
            }
        }
    }
}