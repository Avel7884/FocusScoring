using System.Linq;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if (MarkersList.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData)MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker,MarkersList.Items.Cast<MarkerSubData>().ToArray());
            dialog.Show();
            dialog.Closed += (ev, ob) =>
            {
                markerData.Update();
                MarkersList.Items.Refresh();
            };
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(manager);
            a.Owner = this;
            a.Show();
        }
    }
}