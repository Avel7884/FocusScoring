using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MarkerDialog : Window
    {
        private readonly Marker marker;

        public MarkerDialog(Marker marker)
        {
            this.marker = marker;
            InitializeComponent();
            Name.Text = marker.Name;
            //Colour.ItemsSource = ((MarkerColour[]) Enum.GetValues(typeof(MarkerColour))).Select(MainWindow.ColourCode);
            Colour.SelectedIndex = (int) marker.Colour % 3;
            IsAffiliated.IsChecked = (int) marker.Colour >= 3;
            Importance.ItemsSource = Enumerable.Range(1, 5);
            Importance.SelectedIndex = marker.Score - 1;

            Description.Text = marker.Description;

            Code.Text = marker.Code ?? "Unavalable, not implemented!";
        }

        private void Cansel_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            marker.Name = Name.Text; //TODO Check for names interception
            marker.Colour = (MarkerColour)((IsAffiliated.IsChecked.Value ? 3 : 0) + Colour.SelectedIndex);
            marker.Score = Importance.SelectedIndex + 1;
            marker.Description = Description.Text;
            marker.Code = Code.Text;
            //TODO make marker change after update
            marker.Save();
            //serializer.Serialize(File.Open("./markers",FileMode.OpenOrCreate),markersList.ToArray());
//            using (var file = File.Open("./markers", FileMode.OpenOrCreate))
//            {
//                var tmp = (Marker[])serializer.Deserialize(file);
//                file.Position = 0;
//                serializer.Serialize(file,tmp.Concat(new[]{marker}).ToArray());
//            }

            Close();
        }
    }
}
