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

            public CompanyData()
            {
                
            }
            
            public CompanyData(string Inn)
            {
                this.Inn = Inn;
                Company = Company.CreateINN(Inn);
                Name = Company.CompanyName();
            }

            public void ReInit()
            {
                Company = Company ?? Company.CreateINN(Inn);
                Company.MakeScore();
                Score = Company.Score;
                if (Score <= 39)
                    CLight = new BitmapImage(ShieldCode(Light.Red));
                if (Score <= 69)
                    CLight = new BitmapImage(ShieldCode(Light.Yellow));
                if (Score > 69)
                    CLight = new BitmapImage(ShieldCode(Light.Green));

            }
            
            [XmlAttribute]
            public string Inn { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public int Score { get; set; }

            internal BitmapImage CLight { get; set; }

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