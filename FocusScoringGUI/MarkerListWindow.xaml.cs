using System.Linq;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerListWindow : Window
    {
        public MarkerListWindow()
        {
            InitializeComponent();

            MarkersList.ItemsSource = Company.DummyCompany.GetAllMarkers.Select(MainWindow.MarkerSubData.Create);

        }
    }
}
