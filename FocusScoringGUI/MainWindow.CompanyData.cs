using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    { 
        private class CompanyData
        {
            private FocusScoring.Company company;
            public CompanyData(string Inn)
            {
                this.Inn = Inn;
                company = Company.CreateINN(Inn);
                Name = company.GetParam("Full");
                Score = company.Score;
            }

            public string Inn { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
            public Light Light { get; set; }
        }
    }
}