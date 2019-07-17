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
                Name = Company.GetParam("Full");
                Score = Company.Score;
            }

            public void ReInit()
            {
                Company = Company ?? Company.CreateINN(Inn);
                Company.MakeScore();
                Score = Company.Score;   
            }
            
            [XmlAttribute]
            public string Inn { get; set; }
            [XmlAttribute]
            public string Name { get; set; }
            [XmlAttribute]
            public int Score { get; set; }
            [XmlAttribute]
            public Light Light { get; set; }
        }
    }
}