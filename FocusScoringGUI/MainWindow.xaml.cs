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
            CompanyList.ItemsSource = CurrentList;

            //MarkersTable.ItemsSource = new Company[0];
        }

        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return; 
            var marker = ((MarkerSubData) MarkersList.SelectedItem).Marker;
            new MarkerDialog(marker).ShowDialog();
        }

        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if(CompanyList.SelectedItem == null)
                return; //TODO Message boxes here and everywhere else
            var inn = ((CompanyData) CompanyList.SelectedItem).Inn;
            MarkersList.ItemsSource = Company.CreateINN(inn).Markers.Select(MarkerSubData.Create);
            MarkersList.Items.Refresh();
        }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if(ListView.SelectedItem == null)
                return;
            CurrentList = Lists[(string) ListView.SelectedItem];
            CompanyList.ItemsSource = CurrentList; 
            CompanyList.Items.Refresh();
        }

        private void ButtonDataUpdate_Click(object s, RoutedEventArgs e)
        {
            if (Inn.Text == "")//TODO Checks with regex 
                return;
            CurrentList.Add(new CompanyData(Inn.Text));
            CompanyList.Items.Refresh();
        }

        //private void ButtonAddList_Click(object s, RoutedEventArgs e)
        //{
        //    if(ListName.Text == "")
        //        return;
        //    CurrentList = new List<CompanyData>();
        //    Lists[ListName.Text] = CurrentList;
        //    ListView.Items.Refresh();
        //    CompanyList.ItemsSource = CurrentList; 
        //    CompanyList.Items.Refresh();
        //}

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            if(ListView.SelectedItem == null)
                return;
            Lists.Remove((string) ListView.SelectedItem);
            ListView.Items.Refresh();
        }
        
        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            new MarkerListWindow().Show();
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}