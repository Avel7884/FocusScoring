
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FocusApiAccess;
using OfficeOpenXml;


namespace FocusScoringGUI
{
    [Serializable]
    public class CompanyData
    {
        public CompanyData(){}
        
        public CompanyData(Company company, IList<string> settings,bool IsBaseMode=false)
        {
            //innStr = company.Inn.ToString();
            Inn = company.Inn;
            Source = company;
            InitParameters(settings,IsBaseMode);
        }

        public void InitParameters(IList<string> settings,bool IsBaseMode=false)
        {
            if(IsBaseMode)
                InitBriefLight(Source);
            else
                InitLight(Source?.Score ?? -1);
            Parameters = settings.Select(getSetting).ToArray();
        }

        
        //private string innStr;
        
        [XmlIgnore]
        private INN inn;
        [XmlAttribute] 
        public string InnStr { get; set; }



        //private INN inn;
        [XmlIgnore]
        public INN Inn
        {
            get => inn ??= InnStr;
            set {
                inn = value;
                InnStr = value.ToString();
            }
        }


        [XmlAttribute]
        public Light CLight { get; set; }
        /*

        [XmlIgnore]
        public Company Source { get; set; }*/
        
        [XmlArray]
        public string[] Parameters { get; set; }
        /*

        public Company MakeSource(ICompanyFactory factory)
        {
            Source = factory.CreateFromInn(Inn);
            return Source;
        }*/
        
        public string getSetting(string parameter)
        {
            if(parameter == "Инн")
                return Inn.ToString();
            if (parameter == "Имя")
            {
                var name  = Source?.CompanyName() ?? "(Загрузка...    )";
                return name != "" ? name : "(Нет данных.)";
            }
            if (Source == null)
                throw new InvalidOperationException($"Impossible to access {parameter} parameter before company initialization ");//return "";//TODO Make an error here
            
            switch (parameter)
            {
                //case "CLight": return InitLight(company.Score); //return new DataTemplate new Image{Source = new BitmapImage(InitLight(company.Score))}; 
                //case "Инн": 
                //case "Имя": return Source.CompanyName();
                //case "Телефон": return string.Join(", ",Source.GetParams("phone"));
                //case "Сайт": return string.Join(", ",Source.GetParams("site"));
                case "Адрес": return Source.CompanyAddress();
                case "Рейтинг": return Source.Score.ToString();
                default:
                    var (param,parser) = LibraryParamsDict[parameter];
                    return parser(string.Join(", ",Source.GetParams(param)));
                /*var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
        return methodInfo == null ? value : methodInfo.Invoke(value, new object[0]);*/
            }
        }
        
        
        private static Dictionary<string,(string,Func<string,string>)> LibraryParamsDict = new Dictionary<string, (string,Func<string,string>)>
        {
            {"ФИО директора", ("headName",s=>s)},
            {"Осн. вид деятельности.", ("Activities",s=>s)},
            //{"Адресс", ("legalAddress",s=>s)},
            //{"Деректор",("head",s=>s)},
            {"Статус" , ("Status",s=>s)},//("Reorganizing",s=>s=="" ? "" : "В состоянии реорганизации")}
            {"Дата Регистрации", ("regDate", s => s != "" ? DateTime.Parse(s).ToString("dd.MM.yyyy") : s)},//s.Replace('-','.'))},//
            {"Телефон",("phone",s=>s)},
            {"Сайт",("site",s=>s)}
        };

        public void Recheck(IList<string> settings,bool IsBaseMode=false)
        {/*
            var tmp = Source;
            Source = null;
            InitParameters(settings, IsBaseMode);//TODO remove tmp
            tmp.ForcedMakeScore();
            Source = tmp;*/
            Source.ForcedMakeScore(IsBaseMode);
            InitParameters(settings, IsBaseMode);
        }

        public static IEnumerable<string> GetAvailableParameters(FocusKeyManager manager)
        {//TODO make sure (, "Телефон", "Сайт") are corrent
            var tmp = manager.IsBaseMode() ? new[] {"Имя", "Инн", "Адрес"} : new[] {"Имя", "Инн", "Рейтинг", "Адрес", "Телефон", "Сайт"};
            return tmp.Concat(LibraryParamsDict
                .Where(p => manager.IsParamAvailable(p.Value.Item1))
                .Select(p => p.Key));
        }

        private void InitBriefLight(Company company)
        {
            if (company.GetParam("ReportRed") == "true")
            {
                CLight = Light.Red;
                return;
            }
            
            if (company.GetParam("ReportYellow") == "true")
            {
                CLight = Light.Yellow;
                return;
            }
            
            if (company.GetParam("ReportGreen") == "true")
            {
                CLight = Light.Green;
                return;
            }

            CLight = Light.Loading;
        }

        public void InitLight(int score)
        {
            if (score < 0)
                CLight = Light.Loading;
            
            if (score>= 0 && score <= 39)
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
                    case Light.Loading: return new Uri("pack://application:,,,/src/loading.png");
                    default: throw new AggregateException();
                }
            }
        }        
    }
}


/*using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using FocusApiAccess;

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