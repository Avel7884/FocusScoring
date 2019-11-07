using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FocusScoring;

namespace FocusScoringGUI
{
    public class CompanyToParameterConverter : IValueConverter
    {
        public static IEnumerable<string> SettedParameters => 
            new[] {"Имя", "Инн", "Рейтинг" }.Concat(LibraryParamsDict.Keys);

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null || !(value is Company company) || !(parameter is string parameterName))
                return value;

            switch (parameterName)
            {
                case "CLight": return InitLight(company.Score); //return new DataTemplate new Image{Source = new BitmapImage(InitLight(company.Score))};
                case "Имя": return company.CompanyName(); 
                case "Инн": return company.Inn;
                case "Рейтинг": return company.Score;
                default:
                    return company.GetParam(LibraryParamsDict[parameterName]);
                    /*var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            return methodInfo == null ? value : methodInfo.Invoke(value, new object[0]);*/
            }
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
        
        private static Dictionary<string,string> LibraryParamsDict = new Dictionary<string, string>
        {
            {"ФИО учеридителя", "FIO"},
            {"Адресс", "legalAddress"}
        };
        
        private Uri InitLight(int score)
        {
            if (score <= 39)
            {
                return new Uri("pack://application:,,,/src/red-shield.png");
            }
            if (score > 39 && score <= 69)
            {           
                return new Uri("pack://application:,,,/src/yellow-shield.png");
            }

            if (score > 69)
            {
                return new Uri("pack://application:,,,/src/green-shield.png");
            }
            throw new ArgumentException("Score were out of bounds: " + score);
        }
    }
}