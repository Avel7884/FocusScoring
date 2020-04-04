using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FocusApiAccess;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerListWindow : Window
    {
        private readonly FocusKeyManager manager;
        private List<MarkerSubData> markersList;

        public MarkerListWindow(FocusKeyManager manager)
        {
            this.manager = manager;
            InitializeComponent();

            markersList = manager.GetAllMarkers.Select(MarkerSubData.Create).ToList();
            
            MarkersList.ItemsSource = markersList;

        }

        private void MarkersList_Click(object sender, MouseButtonEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData) MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker, markersList, true);
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
            var dialog= new MarkerDialog(marker,MarkersList.Items.Cast<MarkerSubData>().ToArray());
            dialog.Show();
            dialog.Save.Click += (ev,ob) =>
            {
                manager.AddMarker(marker);
                markersList.Add(MarkerSubData.Create(marker));
                MarkersList.ItemsSource = markersList;
                MarkersList.Items.Refresh();
            };
        }

        public void DeleteMarkerButton_Click(object obj, EventArgs args)
        {
            var marker = (MarkerSubData) MarkersList.SelectedItem;
            if(marker== null) return;
            markersList.Remove(marker);
            marker.Marker.Delete();
            manager.RemoveMarker(marker.Name);
            MarkersList.Items.Refresh();
        }
    }
}
