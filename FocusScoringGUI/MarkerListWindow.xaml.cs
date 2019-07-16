using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerListWindow : Window
    {
        public MarkerListWindow(Marker[] markers)
        {
            InitializeComponent();

            MarkersList.ItemsSource = markers.Select(MainWindow.MarkerSubData.Create);

        }

        private void MarkersList_Click(object sender, MouseButtonEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return;
            var marker = ((MainWindow.MarkerSubData) MarkersList.SelectedItem).Marker;
            var MarkerDialog = new MarkerDialog(marker);
            MarkerDialog.Owner = this;
            MarkerDialog.Show();
            MarkerDialog.Closed += (ev,ob) => this.MarkersList.Items.Refresh();
        }
    }
}
