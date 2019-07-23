using System;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    { 
        public class CompanyData
        {
            internal Company Company { get; set; }

            public CompanyData(){}
            
            public CompanyData(string Inn,FocusScoringManager manager)
            {
                this.Inn = Inn;
                Company = manager.CreateFromInn(Inn);
                Name = Company.CompanyName();
                IsChecked = false;
                Score = -1;
            }
             
            [XmlAttribute]
            public bool IsChecked { get; set; }
            [XmlAttribute]
            public string Inn { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public int Score { get; set; }
            [XmlAttribute]
            public Light Light { get; set; }
            
            internal BitmapImage CLight { get; set; }

            public void Check(FocusScoringManager manager)
            {
                IsChecked = true;
                Company = Company ?? manager.CreateFromInn(Inn);
                Company.MakeScore();
                Score = Company.Score;
                InitLight();
            }

            private void InitLight()
            {
                if (Score < 0)
                    return;
                
                if (Score <= 39)
                {
                    Light = Light.Red;
                    CLight = new BitmapImage(ShieldCode(Light.Red));                    
                }

                if (Score <= 69)
                {
                    Light = Light.Yellow;
                    CLight = new BitmapImage(ShieldCode(Light.Yellow));
                }

                if (Score > 69)
                {
                    Light = Light.Green;
                    CLight = new BitmapImage(ShieldCode(Light.Green));
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