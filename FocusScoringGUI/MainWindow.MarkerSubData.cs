using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
                Colour =  new BitmapImage(ColourCode(marker.Colour));
                Description = marker.Description;
                Name = marker.Name;
                Verbose = "";
            }
            
            public MarkerSubData(MarkerResult result)
            {
                Marker = result.Marker;
                Colour =  new BitmapImage(ColourCode(result.Marker.Colour));
                Description = result.Marker.Description;
                Verbose = result.Verbose;
                Name = result.Marker.Name;
            }
            
            public static MarkerSubData Create(Marker marker)=>
                new MarkerSubData(marker);
            
            public static MarkerSubData Create(MarkerResult result)=>
                new MarkerSubData(result);
            
            public Marker Marker { get; }
            public BitmapImage Colour { get; }
            public string Name { get; }
            public string Description { get; }
            public string Verbose { get; }
        }
        
        private static Uri ColourCode(MarkerColour colour)
        {
            switch (colour)
            {
                case MarkerColour.Green: return new Uri("pack://application:,,,/src/GreenDot.png");
                case MarkerColour.Red: return new Uri("pack://application:,,,/src/RedDot.png"); 
                case MarkerColour.Yellow: return new Uri("pack://application:,,,/src/YellowDot.png");
                case MarkerColour.GreenAffiliates: return new Uri("pack://application:,,,/src/GreenDotAffiliates.png");
                case MarkerColour.RedAffiliates: return new Uri("pack://application:,,,/src/RedDotAffiliates.png");
                case MarkerColour.YellowAffiliates: return new Uri("pack://application:,,,/src/YellowDotAffiliates.png");
                default: throw new AggregateException();
            }
        }
    }
}