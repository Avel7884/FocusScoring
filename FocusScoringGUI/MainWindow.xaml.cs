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
    public partial class MainWindow:Window
    {
        private MarkerSubData[] dataMarkersSource;
        private List<CompanyData> CurrentList;
        private Dictionary<string, List<CompanyData>> Lists;
        private CompanyListsCache companiesCache;

        //public string Inn { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //var binding = new Binding {Source = Inn};
            
            Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            companiesCache = CompanyListsCache.Create();
            Lists = companiesCache.GetLists();

            if (Lists.Count == 0)
            {
                var list = new List<CompanyData>();
                Lists["NewList"] = list;
                companiesCache.UpdateList("NewList",list);
            }

            ListView.ItemsSource = Lists.Keys;
            CurrentList = Lists.First().Value;
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

        private void ButtonAddList(string name, List<CompanyData> list)
        {
            CurrentList = list;
            Lists[name] = CurrentList;
            companiesCache.UpdateList(name, list);
            ListView.Items.Refresh();
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
        }

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
            new ListDialog(ButtonAddList).Show();
        }

        //private void Inn_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = !(Char.IsDigit(e.Key.ToString(), 0));
        //}

        private void Inn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }
    }
}