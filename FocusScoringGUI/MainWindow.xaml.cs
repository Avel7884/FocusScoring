using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;


namespace FocusScoringGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MarkerSubData[] dataMarkersSource;
        private List<CompanyData> CurrentList; 
        private Dictionary<string, List<CompanyData>> Lists;
        
        public class CompanyData
        {
            private FocusScoring.Company company;
            public CompanyData(string Inn)
            {
                this.Inn = Inn;
                company = Company.CreateINN(Inn);
                Name = company.GetParam("Full");
                Score = 10;
            }

            public string Inn { get; set; }
            public string Name { get; set; }
            public int Score { get; set; }
            public Light Light { get; set; }
        }

        public class MarkerSubData
        {
            public MarkerSubData(Marker marker)
            {
                Colour = ColourCode(marker.Colour);
                Description = marker.Desctiption;
            }
            
            public static MarkerSubData Create(Marker marker)=>
                new MarkerSubData(marker);
            
            public string Colour { get; }
            public string Description { get; }
        }
        
        //public string Inn { get; set; }
    
        public MainWindow()
        {
            InitializeComponent();
            
            //var binding = new Binding {Source = Inn};
            
            Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            Lists = new Dictionary<string, List<CompanyData>>
            {
                {"Good People", new List<CompanyData> {new CompanyData("6167110026"), new CompanyData("3454001339")}},
                {"Baad People", new List<CompanyData> {new CompanyData("3444162030"), new CompanyData("3454001339")}}
            };

            ListView.ItemsSource = Lists.Keys;
            CurrentList = Lists["Good People"];
            CompanyTable.ItemsSource = CurrentList;

            //MarkersTable.ItemsSource = new Company[0];
        }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            CurrentList = Lists[(string) ListView.SelectedItem];
            CompanyTable.ItemsSource = CurrentList; 
            CompanyTable.Items.Refresh();
        }

        private void ButtonDataUpdate_Click(object s, RoutedEventArgs e)
        {
            CurrentList.Add(new CompanyData(Inn.Text));
            CompanyTable.Items.Refresh();
        }

        private void ButtonAddList_Click(object s, RoutedEventArgs e)
        {
            CurrentList = new List<CompanyData>();
            Lists[ListName.Text] = CurrentList;
            ListView.Items.Refresh();
            CompanyTable.ItemsSource = CurrentList; 
            CompanyTable.Items.Refresh();
        }
        
        private void ButtonMarkersUpdate_Click(object s, RoutedEventArgs e)
        {
            //dataMarkersSource.Add(new CompanyData(Inn2.Text));
            MarkersTable.ItemsSource = Company.CreateINN(Inn2.Text).GetMarkers().Select(MarkerSubData.Create);
            MarkersTable.Items.Refresh(); 
        }
//
//        private void CompanyRow_Selected(object s, MouseButtonEventArgs e)
//        {
//            var row = (DataGridRow) s;
//            row.
//        }

        private static string ColourCode(MarkerColour colour)
        {
            switch (colour)
            {
                case MarkerColour.Green: return "Ok";
                case MarkerColour.Red: return "X";
                case MarkerColour.Yellow: return "*";
                default: throw new AggregateException();
            }
        }
    }
}