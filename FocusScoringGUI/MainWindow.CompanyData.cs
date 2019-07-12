using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
<<<<<<< HEAD
    { 
=======
    {
>>>>>>> 790bc751e7648628f2a94a9d5546fc95113d4bc6
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