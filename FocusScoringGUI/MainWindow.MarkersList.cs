using System.Windows;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if (MarkersList.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData)MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker);
            dialog.Show();
            dialog.Closed += (ev, ob) =>
            {
                markerData.Update();
                MarkersList.Items.Refresh();
            };
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(scorer.GetAllMarkers);
            a.Owner = this;
            a.Show();
        }
    }
}