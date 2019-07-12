using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    { 
        public class CompanyData
        {
            private FocusScoring.Company company;

            public CompanyData()
            {
                
            }
            
            public CompanyData(string Inn)
            {
                this.Inn = Inn;
                company = Company.CreateINN(Inn);
                Name = company.GetParam("Full");
                Score = 0;
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