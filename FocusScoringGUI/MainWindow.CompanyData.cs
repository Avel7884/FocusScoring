using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    { 
        [Serializable]
        public class CompanyData
        {
            
            [XmlAttribute]
            public int score;
            internal Company Company { get; set; }

            public CompanyData(){}
            
            public CompanyData(string Inn,FocusScoringManager manager)
            {
                this.Inn = Inn;
                Company = manager.CreateFromInn(Inn);
                Name = Company.CompanyName();
                IsChecked = false;
                score = -1;
            }
             
            [XmlAttribute]
            public bool IsChecked { get; set; }
            [XmlAttribute]
            public string Inn { get; set; }
            [XmlAttribute]
            public string Name { get; set; }

            [XmlAttribute] public string Score => score == -1 ? "" : score.ToString();

            [XmlAttribute]
            public Light Light { get; set; }
            [XmlAttribute]
            public bool Autoupdate { get; set; }

            [XmlIgnore] public BitmapImage CLight => IsChecked ? new BitmapImage(ShieldCode(Light)) : null;

            public void Check(FocusScoringManager manager, bool force = false)
            {
                IsChecked = true;
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
                if (score>39 && score <= 69)
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
                    case Light.Green: return new Uri("pack://application:,,,/src/GreenDot.png");
                    case Light.Red: return new Uri("pack://application:,,,/src/RedDot.png");
                    case Light.Yellow: return new Uri("pack://application:,,,/src/YellowDot.png");
                    default: throw new AggregateException();
                }
            }
        }
    }
}