
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    [Serializable]
    public class CompanyData
    {
        public CompanyData(){}
        
        public CompanyData(Company company, List<string> settings)
        {
            Inn = company.Inn;
            Source = company;
            InitParameters(settings);
        }

        private void InitParameters(List<string> settings)
        {
            Parameters = settings.Select(Convert).ToArray();
        }
        
        [XmlAttribute]
        public string Inn { get; set; }
        
        [XmlAttribute]
        public Light CLight { get; set; }
        
        [XmlIgnore]
        public Company Source { get; set; }
        
        [XmlArray]
        public string[] Parameters { get; set; }

        public Company MakeSource(ICompanyFactory factory)
        {
            Source = factory.CreateFromInn(Inn);
            return Source;
        }
        
        public string Convert(string parameter)
        {

            switch (parameter)
            {
                //case "CLight": return InitLight(company.Score); //return new DataTemplate new Image{Source = new BitmapImage(InitLight(company.Score))};
                case "Имя": return Source.CompanyName(); 
                case "Инн": return Inn;
                case "Адрес": return Source.CompanyAddress();
                case "Рейтинг": return Source.Score.ToString();
                default:
                    var (param,parser)= LibraryParamsDict[parameter];
                    return parser(Source.GetParam(param));
                /*var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
        return methodInfo == null ? value : methodInfo.Invoke(value, new object[0]);*/
            }
        }
        
        
        private static Dictionary<string,(string,Func<string,string>)> LibraryParamsDict = new Dictionary<string, (string,Func<string,string>)>
        {
            {"ФИО учеридителя", ("headName",s=>s)},
            {"Осн. вид деятельности.", ("Activities",s=>s)},
            //{"Адресс", ("legalAddress",s=>s)},
            //{"Деректор",("head",s=>s)},
            {"Статус" , ("Status",s=>s)},//("Reorganizing",s=>s=="" ? "" : "В состоянии реорганизации")}
            {"Дата Регистрации", ("regDate", s=>s)},
            {"Телефон",("phone",s=>s)},
            {"Возможный сайт",("site",s=>s)}
        };
        
        public void Recheck(List<string> settings)
        {
            Source.ForcedMakeScore();
            InitParameters(settings);
        }
                
        public static IEnumerable<string> GetAvailableParameters(FocusKeyManager manager) =>
            new[] {"Имя", "Инн", "Рейтинг", "Адрес"}.Concat(LibraryParamsDict
                .Where(p => manager.IsParamAvailable(p.Value.Item1))
                .Select(p => p.Key));


        private void InitLight(int score)
        {
            if (score < 0)
                return;
            
            if (score <= 39)
            {
                CLight = Light.Red;
                //CLight = new BitmapImage(ShieldCode(Light.Red));                    
            }
            if (score > 39 && score <= 69)
            {
                CLight = Light.Yellow;
                //CLight = new BitmapImage(ShieldCode(Light.Yellow));
            }

            if (score > 69)
            {
                CLight = Light.Green;
                //CLight = new BitmapImage(ShieldCode(Light.Green));
            }
        }
        
        [XmlIgnore]
        public Uri ShieldCode
        {
            get
            {
                switch (CLight)
                {
                    case Light.Green: return new Uri("pack://application:,,,/src/green-shield.png");
                    case Light.Red: return new Uri("pack://application:,,,/src/red-shield.png");
                    case Light.Yellow: return new Uri("pack://application:,,,/src/yellow-shield.png");
                    default: throw new AggregateException();
                }
            }
        }        
    }
}


/*using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    [Serializable]
    public class CompanyData
    {
        //TODO Attempt to remove this class
        
        
        [XmlAttribute]
        private int score;
        internal Company Company { get; set; }
        
        public CompanyData(){}//Required for XML serialization
        
        public CompanyData(string Inn,FocusScoringManager manager)
        {
            this.Inn = Inn;
            Company = manager.CreateFromInn(Inn);
            Name = Company.CompanyName();
            /*IsChecked = false;#1#
            score = -1;
        }
         /*

        [XmlAttribute]
        public bool IsChecked { get; set; }#1#
        [XmlIgnore]
        public string Inn { get; set; }
        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore] public string Score => score == -1 ? "" : score.ToString();

        [XmlIgnore]
        public Light Light { get; set; }

        [XmlIgnore] public BitmapImage CLight => /*IsChecked ?#1# new BitmapImage(ShieldCode(Light));/* : null;#1#

        public void Check(FocusScoringManager manager, bool force = false)
        {
/*
            IsChecked = true;
#1#
            Company = Company ?? manager.CreateFromInn(Inn);
            if(force)
                Company.ForcedMakeScore();
            else
                Company.MakeScore();
            score = Company.Score;
            InitLight();
        }

        private void InitLight()
        {
            if (score < 0)
                return;
            
            if (score <= 39)
            {
                Light = Light.Red;
                //CLight = new BitmapImage(ShieldCode(Light.Red));                    
            }
            if (score > 39 && score <= 69)
            {
                Light = Light.Yellow;
                //CLight = new BitmapImage(ShieldCode(Light.Yellow));
            }

            if (score > 69)
            {
                Light = Light.Green;
                //CLight = new BitmapImage(ShieldCode(Light.Green));
            }
        }
        
        private Uri ShieldCode(Light light)
        {
            switch (light)
            {
                case Light.Green: return new Uri("pack://application:,,,/src/green-shield.png");
                case Light.Red: return new Uri("pack://application:,,,/src/red-shield.png");
                case Light.Yellow: return new Uri("pack://application:,,,/src/yellow-shield.png");
                default: throw new AggregateException();
            }
        }
    }
    
}*/