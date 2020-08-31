using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FocusAccess;
using FocusApp;

namespace FocusGUI
{
    public class EntryToParameterConverter<TTarget> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DataEntry<TTarget> entry) || !(parameter is int parameterIndex))
                return null;

            /*if (parameterIndex >= entry.Data.Count)
                return "";//TODO Kostil!!*/
            
            return entry.Data[parameterIndex];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("EntryToParameterConverter can only be used for one way conversion.");
        }
    }
}