using System;
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
            var markerData = ((MainWindow.MarkerSubData) MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker);
            dialog.ShowDialog();
            dialog.Closed += (ev,ob) =>
            {
                markerData.Update();
                MarkersList.Items.Refresh();
            };
        }

        public void NewMarkerButton_Click(object obj, EventArgs args)
        {
            var marker = new Marker();
            var dialog= new MarkerDialog(marker);
            dialog.Show();
            dialog.Closed += (ev,ob) =>
            {
                MarkersList.Items.Add(marker);
                MarkersList.Items.Refresh();
            };
        }
    }
}
