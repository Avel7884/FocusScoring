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
        public static IEnumerable<string> GetAvailableParameters(FocusKeyManager manager) =>
            new[] {"Имя", "Инн", "Рейтинг"}.Concat(LibraryParamsDict
                .Where(p => manager.IsParamAvailable(p.Value.Item1))
                .Select(p => p.Key));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if (!(value is CompanyData companyData) || !(parameter is int parameterIndex))
                return value;

            if (parameterIndex >= companyData.Parameters.Length)
                return "";//TODO Kostil!!
            
            return companyData.Parameters[parameterIndex];

            /*
            if(value==null)
                if (parameter == "CLight")
                    return InitLight(0); //TODO Create new shield
                else
                    return "";
            
            if (!(value is Company company) || !(parameter is string parameterName))
                return value;

            switch (parameterName)
            {
                case "CLight": return InitLight(company.Score); //return new DataTemplate new Image{Source = new BitmapImage(InitLight(company.Score))};
                case "Имя": return company.CompanyName(); 
                case "Инн": return company.Inn;
                case "Рейтинг": return company.Score;
                default:
                    var (param,parser)= LibraryParamsDict[parameterName];
                    return parser(company.GetParam(param));
                    /*var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
            return methodInfo == null ? value : methodInfo.Invoke(value, new object[0]);#1#
            }*/
        }
        

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
        
        private static Dictionary<string,(string,Func<string,string>)> LibraryParamsDict = new Dictionary<string, (string,Func<string,string>)>
        {
            {"ФИО учеридителя", ("headName",s=>s)},
            {"Осн. вид деятельности.", ("Activities",s=>s)},
            {"Адресс", ("legalAddress",AddrParser)},
            //{"Деректор",("head",s=>s)},
            {"Статус" , ("Status",s=>s)},//("Reorganizing",s=>s=="" ? "" : "В состоянии реорганизации")}
            {"Дата Регистрации", ("regDate", s=>s)},
            {"Телефон",("phone",s=>s)},
            {"Возможный сайт",("site",s=>s)}
        };


        static List<(string,string)> locations = new List<(string, string)>
        {
            ("облобласть"," обл."),
            ("ггород"," г."),
            ("пр-ктпроспект"," пр-кт "),
            ("р-нрайон"," р-н"),
            ("рпрабочий поселок"," рп "),
            ("домдом"," дом "),
            ("улулица"," ул."),
            ("стрстроение"," стр."),
            ("перпереулок","пер.")
        };
        private static string AddrParser(string addr)
        {
            if (addr == null || addr.Length == 0)
                return addr;
            addr = addr.TrimStart("1234567890".ToCharArray());
            foreach (var (full,shor) in locations)
                addr = addr.Replace(full, shor); //TODO Optimize!!!!
            return addr.Substring(0,addr.Length-20);
        }
        

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