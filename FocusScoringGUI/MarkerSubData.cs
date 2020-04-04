using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FocusApiAccess;
using FocusScoring;

namespace FocusScoringGUI
{
        public class MarkerSubData
        {
            private MarkerSubData(Marker marker)
            {
                Marker = marker;
                Verbose = "";
                Update();
            }

            private MarkerSubData(MarkerResult result)
            {
                Marker = result.Marker;
                Verbose = result.Verbose;
                Update();
            }
            
            public static MarkerSubData Create(Marker marker)=>
                new MarkerSubData(marker);
            
            public static MarkerSubData Create(MarkerResult result)=>
                new MarkerSubData(result);
            
            public Marker Marker { get; }
            public BitmapImage Colour { get; private set; }
            public string Name { get; private set;}
            public string Description { get; private set;}
            public string Verbose { get; }

            public void Update()
            {
                Colour =  new BitmapImage(ColourCode(Marker.Colour));
                Description = Marker.Description;
                Name = Marker.Name;
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