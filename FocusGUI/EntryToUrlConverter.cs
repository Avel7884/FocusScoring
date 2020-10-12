using System;
using System.Globalization;
using System.Windows.Data;
using FocusAccess;
using FocusApp;

namespace FocusGUI
{
    public class EntryToUrlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DataEntry<INN> entry))
                return value;
            if (entry.Score <= 39)
            {
                return "pack://application:,,,/src/red-shield.png";
            }
            if (entry.Score > 39 && entry.Score <= 69)
            {           
                return "pack://application:,,,/src/yellow-shield.png";
            }

            if (entry.Score > 69)
            {
                return "pack://application:,,,/src/green-shield.png";
            }
            throw new ArgumentException("Score were out of bounds: " + entry.Score);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}