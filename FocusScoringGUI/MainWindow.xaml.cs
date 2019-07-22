using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;


namespace FocusScoringGUI
{ //TODO Refactor
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private MarkerSubData[] dataMarkersSource;
        private List<CompanyData> CurrentList;
        private string currentListName;
        //private Dictionary<string, List<CompanyData>> Lists;
        private List<string> ListNames;
        private CompanyListsCache companiesCache;
//        public Scorer scorer;

        //public string Inn { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //var binding = new Binding {Source = Inn};

            FocusScoringManager.StartAccess("3c71a03f93608c782f3099113c97e28f22ad7f45");
            companiesCache = CompanyListsCache.Create();
            ListNames = companiesCache.GetNames();

            if (ListNames.Count == 0)
            {
                var list = new List<CompanyData>();
                CurrentList = list;
                companiesCache.UpdateList("NewList", list);
            }

            currentListName = ListNames.First();
            ListView.ItemsSource = ListNames;
            CurrentList = companiesCache.GetList(currentListName);
            TextBlockList.Text = currentListName;
            CompanyList.ItemsSource = CurrentList;

            //MarkersTable.ItemsSource = new Company[0];
        }

        //private void Inn_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = !(Char.IsDigit(e.Key.ToString(), 0));
        //}

        private void Inn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsDigit(e.Text, 0));
        }

        private void Inn_TextChanged(object sender, TextChangedEventArgs e)
        {
            var addedlength = e.Changes.ElementAt(0).AddedLength;
            if (!Inn.Text.All(char.IsDigit))
            {
                Inn.Text = Inn.Text.Remove(Inn.SelectionStart - addedlength, addedlength);
                Inn.SelectionStart = Inn.Text.Length;
            }
        }

        private void GridViewColumn_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
