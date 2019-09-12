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
        private FocusScoringManager manager;

        public MarkersList()
        {
            Loaded +=  Init;
            InitializeComponent();
        }

        private void Init(object o, EventArgs args)
        {
             var mainWindow = ((MainWindow) ((Grid) Parent).Parent);
            manager = mainWindow.FocusManager;
            mainWindow.Markers = this;
        }

        public MarkersList(FocusScoringManager manager)
        {
            InitializeComponent();
        }

        public void ShowNewMarkers(CompanyData companyData)
        {
            if (companyData.IsChecked)
            {
                companyData.Check(manager);
                MarkersListView.ItemsSource = companyData.Company.Markers.Select(MarkerSubData.Create);
            }
            else MarkersListView.ItemsSource = new MarkerSubData[0]; 
            
            MarkersListView.Items.Refresh();
        }
        
        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if (MarkersListView.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData)MarkersListView.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker,MarkersListView.Items.Cast<MarkerSubData>().ToArray());
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
