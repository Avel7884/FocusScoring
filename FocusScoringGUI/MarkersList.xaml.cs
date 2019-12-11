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
            markersTab = TabControl.Items[1];
        }

        private bool mode;
        private object markersTab;
        public bool IsBaseMode
        {
            get => mode;
            set
            {
                if (mode == value) return;
                if (value)
                    TabControl.Items.RemoveAt(0);
                else
                    TabControl.Items.Insert(0,markersTab);
                mode = value;
                /*pdfWebViewer.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                MarkersListView.Visibility = value ? Visibility.Hidden : Visibility.Visible;*/
            }
        }
        
        

        
        public void ShowNewMarkers(Company company)
        { 
            if (!IsBaseMode)
            {
                MarkersListView.ItemsSource = company.Markers.Select(MarkerSubData.Create);
                MarkersListView.Items.Refresh();
            }
            var url = company.GetParam("Report");
            pdfWebViewer.Navigate(url != "" ? url : "about:blank");
            
            /*if ((TabControl.SelectedItem as TabItem).Name == "Маркеры")
            {
                MarkersListView.ItemsSource = company.Markers.Select(MarkerSubData.Create);
                MarkersListView.Items.Refresh();
            }
            else
            {
                var url = company.GetParam("Report");
                pdfWebViewer.Navigate(url != "" ? url : "about:blank");
            }
            if (companyData.IsChecked)
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
