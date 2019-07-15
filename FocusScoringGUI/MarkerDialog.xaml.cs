using System;
using System.Linq;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerDialog : Window
    {
        public MarkerDialog(Marker marker)
        {
            InitializeComponent();
            Name.Text = marker.Name;
            
            //Colour.ItemsSource = ((MarkerColour[]) Enum.GetValues(typeof(MarkerColour))).Select(MainWindow.ColourCode);
            Colour.SelectedIndex = (int)marker.Colour;
            
            Importance.ItemsSource = Enumerable.Range(1, 5);
            Importance.SelectedIndex = marker.Score - 1;

            Description.Text = marker.Description;

            Code.Text = "Unavalable, not implemented!";
        }

        private void Cansel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
            //TODO They shoud do sOmething!
        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
