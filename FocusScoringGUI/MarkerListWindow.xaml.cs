using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerListWindow : Window
    {
        private readonly FocusScoringManager manager;
        private List<MainWindow.MarkerSubData> markersList;

        public MarkerListWindow(FocusScoringManager manager)
        {
            this.manager = manager;
            InitializeComponent();

            markersList = manager.GetAllMarkers.Select(MainWindow.MarkerSubData.Create).ToList();
            
            MarkersList.ItemsSource = markersList;

        }

        private void MarkersList_Click(object sender, MouseButtonEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return;
            var markerData = ((MainWindow.MarkerSubData) MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker,markersList);
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
            var dialog= new MarkerDialog(marker,MarkersList.Items.Cast<MainWindow.MarkerSubData>().ToArray());
            dialog.Show();
            dialog.Save.Click += (ev,ob) =>
            {
                manager.AddMarker(marker);
                markersList.Add(MainWindow.MarkerSubData.Create(marker));
                MarkersList.ItemsSource = markersList;
                MarkersList.Items.Refresh();
            };
        }

        public void DeleteMarkerButton_Click(object obj, EventArgs args)
        {
            var marker = (MainWindow.MarkerSubData) MarkersList.SelectedItem;
            if(marker== null) return;
            markersList.Remove(marker);
            marker.Marker.Delete();
            manager.RemoveMarker(marker.Name);
            MarkersList.Items.Refresh();
        }
    }
}
