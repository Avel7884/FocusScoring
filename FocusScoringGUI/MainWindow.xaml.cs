using System.Collections.ObjectModel;
using FocusScoring;


namespace FocusScoringGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public class CompanyData
        {
            private FocusScoring.Company company;
            public CompanyData(string Inn)
            {
                this.Inn = Inn;
                company = Company.CreateINN(Inn);
                Name = company.GetParam("Full");
                Score = 10;
            }

            public string Inn { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
            public Light Light { get; set; }
        }
    
        public MainWindow()
        {
            InitializeComponent();

            FocusScoring.Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            var list = new ObservableCollection<CompanyData>();
            list.Add(new CompanyData("6167110026") );
            list.Add(new CompanyData("3454001339") );
            list.Add(new CompanyData("3444162030") );
            this.dataGrid1.ItemsSource = list;
        }
    }
}