using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void MarkersList_Click(object sender, MouseButtonEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return;
            var marker = ((MainWindow.MarkerSubData) MarkersList.SelectedItem).Marker;
            new MarkerDialog(marker).ShowDialog(); 
        }
    }
}
