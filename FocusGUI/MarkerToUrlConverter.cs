using System;
using System.Globalization;
using System.Windows.Data;
using FocusAccess;
using FocusScoring;

namespace FocusGUI
{
    public class MarkerToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MarkerResult<INN> result))
                return value;
            switch (result.Marker.Colour)
            {
                case MarkerColour.Green: return new Uri("pack://application:,,,/src/GreenDot.png");
                case MarkerColour.Red: return new Uri("pack://application:,,,/src/RedDot.png"); 
                case MarkerColour.Yellow: return new Uri("pack://application:,,,/src/YellowDot.png");
                case MarkerColour.GreenAffiliates: return new Uri("pack://application:,,,/src/GreenDotAffiliates.png");
                case MarkerColour.RedAffiliates: return new Uri("pack://application:,,,/src/RedDotAffiliates.png");
                case MarkerColour.YellowAffiliates: return new Uri("pack://application:,,,/src/YellowDotAffiliates.png");
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}