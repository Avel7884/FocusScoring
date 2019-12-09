using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkersList : UserControl
    {
        //public FocusKeyManager Manager { get; set; }

        public MarkersList()
        {
            //Loaded +=  Init;
            InitializeComponent();
        }

        private bool mode;
        public bool IsBaseMode
        {
            get => mode;
            set
            {
                pdfWebViewer.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MarkersListView.Visibility = value ? Visibility.Hidden : Visibility.Visible;
                mode = value;
            }
        }

        public void ShowNewMarkers(Company company)
        {
            if (IsBaseMode)
            {
                var url = company.GetParam("Report");
                pdfWebViewer.Navigate(url != "" ? url : "about:blank");
            }
            else
            {
                MarkersListView.ItemsSource = company.Markers.Select(MarkerSubData.Create);
                MarkersListView.Items.Refresh();
            }
            /*if (companyData.IsChecked)
            {
                companyData.Check(Manager);
                MarkersListView.ItemsSource = companyData.Company.Markers.Select(MarkerSubData.Create);
            }
            else MarkersListView.ItemsSource = new MarkerSubData[0]; */
        }
        
        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if (MarkersListView.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData)MarkersListView.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker,MarkersListView.Items.Cast<MarkerSubData>().ToArray(),true);
            dialog.Show();
            dialog.Closed += (ev, ob) =>
            {
                markerData.Update();
                MarkersListView.Items.Refresh();
            };
        }

        //TODO make it uncomment probably with new object
        
        /*

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(manager);
            a.Owner = this;
            a.Show();
        }*/
    }
}
