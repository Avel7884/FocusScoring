using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using FocusScoring;


namespace FocusScoringGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<CompanyData> dataItemsSource;

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
        
        //public string Inn { get; set; }
    
        public MainWindow()
        {
            InitializeComponent();

            //var binding = new Binding {Source = Inn};

            FocusScoring.Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            dataItemsSource = new ObservableCollection<CompanyData>();
            dataItemsSource.Add(new CompanyData("6167110026") );
            dataItemsSource.Add(new CompanyData("3454001339") );
            dataItemsSource.Add(new CompanyData("3444162030") );
            Data.ItemsSource = dataItemsSource;
        }

        private void ButtonDataUpdate_Click(object s, RoutedEventArgs e)
        {
            dataItemsSource.Add(new CompanyData(Inn.Text) );
            Data.Items.Refresh();
        }
    }
}