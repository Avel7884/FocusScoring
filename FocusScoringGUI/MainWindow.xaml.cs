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
        public Scorer scorer;

        //public string Inn { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //var binding = new Binding {Source = Inn};

            Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            companiesCache = CompanyListsCache.Create();
            ListNames = companiesCache.GetNames();
            scorer = new Scorer();

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

        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if (MarkersList.SelectedItem == null)
                return;
            var markerData = ((MarkerSubData)MarkersList.SelectedItem);
            var dialog = new MarkerDialog(markerData.Marker);
            dialog.Show();
            dialog.Closed += (ev, ob) =>
            {
                markerData.Update();
                MarkersList.Items.Refresh();
            };
        }

        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if(CompanyList.SelectedItem == null)
                return;
            var companyData = ((CompanyData) CompanyList.SelectedItem);
            TextBlockName.Text = companyData.Name;
            MarkersList.ItemsSource = scorer.CheckMarkers(companyData.Company ??
                                                         (companyData.Company = Company.CreateINN(companyData.Inn)))
                                            .Select(MarkerSubData.Create);
            MarkersList.Items.Refresh();
        }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null)
                return;
            currentListName = (string)ListView.SelectedItem;
            CurrentList = companiesCache.GetList(currentListName);
            TextBlockList.Text = currentListName;
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
        }

        private readonly int[] k = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        private bool InnCheckSum(string inn)
        {
            var numbers = inn.Select(x => new string(new[] { x })).Select(int.Parse).ToArray();
            if (numbers.All(x => x == 0))
                return false;

            switch (numbers.Length)
            {
                case 10:
                    return numbers.Take(9).Zip(k.Skip(2), (x, y) => x * y).Sum() % 11 % 10 == numbers[9];
                case 12:
                    return numbers.Take(10).Zip(k.Skip(1), (x, y) => x * y).Sum() % 11 % 10 == numbers[10] &&
                           numbers.Take(11).Zip(k, (x, y) => x * y).Sum() % 11 % 10 == numbers[11];
                default:
                    return false;
            }
        }

        private void ButtonDataUpdate_Click(object s, RoutedEventArgs e)
        {
            if (CurrentList.Select(x => x.Inn).Contains(Inn.Text))
            {
                MessageBox.Show("Company already in list");
                return;
            }

            if (!InnCheckSum(Inn.Text))
            {
                MessageBox.Show("Invalid inn");
                return;
            }

            var data = new CompanyData(Inn.Text);
            CurrentList.Add(data);
            companiesCache.UpdateList(currentListName, new[] { data });
            CompanyList.Items.Refresh();
        }

        private void ButtonAddList(string name, List<CompanyData> list)
        {
            CurrentList = list;
            currentListName = name;
            companiesCache.UpdateList(currentListName, list);
            ListNames.Add(name);
            ListView.Items.Refresh();
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            companiesCache.DeleteList(name);
            currentListName = ListNames.First();
            CurrentList = companiesCache.GetList(currentListName);
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
            TextBlockList.Text = currentListName;
            ListNames.Remove(name);
            ListView.Items.Refresh();
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(scorer.GetAllMarkers);
            a.Owner = this;
            a.Show();
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

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}