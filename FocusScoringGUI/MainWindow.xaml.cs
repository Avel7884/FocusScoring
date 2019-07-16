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
        private string currentListName;
        private Dictionary<string, List<CompanyData>> Lists;
        private CompanyListsCache companiesCache;
        public Scorer scorer;

        //public string Inn { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //var binding = new Binding {Source = Inn};
            
            Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";
            companiesCache = CompanyListsCache.Create();
            Lists = companiesCache.GetLists();
            scorer = new Scorer();

            if (Lists.Count == 0)
            {
                var list = new List<CompanyData>();
                Lists["NewList"] = list;

                companiesCache.UpdateList("NewList", list);
            }

            ListView.ItemsSource = Lists.Keys;
            CurrentList = Lists.First().Value;
            currentListName = Lists.First().Key;
            CompanyList.ItemsSource = CurrentList;

            //MarkersTable.ItemsSource = new Company[0];
        }

        private void MarkerSelected_Click(object s, RoutedEventArgs e)
        {
            if(MarkersList.SelectedItem == null)
                return; 
            var marker = ((MarkerSubData) MarkersList.SelectedItem).Marker;
            var dialog = new MarkerDialog(marker);
            dialog.ShowDialog();
            dialog.Closed += (ev, ob) => MarkersList.Items.Refresh();
        }
        
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if(CompanyList.SelectedItem == null)
                return; //TODO Message boxes here and everywhere else
            var companyData = ((CompanyData) CompanyList.SelectedItem);
            TextBlockName.Text = companyData.Name;
            MarkersList.ItemsSource = scorer.CheckMarkers(companyData.Company ?? 
                                                         (companyData.Company = Company.CreateINN(companyData.Inn)))
                                            .Select(MarkerSubData.Create);
            MarkersList.Items.Refresh();
        }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if(ListView.SelectedItem == null)
                return;
            var SelectedList = (string)ListView.SelectedItem;
            CurrentList = Lists[SelectedList];
            TextBlockList.Text = SelectedList;
            CompanyList.ItemsSource = CurrentList; 
            CompanyList.Items.Refresh();
            currentListName = SelectedList;
        }

        private readonly int[] k = {3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8};
        private bool InnCheckSum(string inn)
        {
            var numbers = inn.Select(x=>new string(new []{x})).Select(int.Parse).ToArray();
            if (numbers.All(x => x == 0))
                return false;
            
            switch (numbers.Length)
            {
                case 10:
                    return numbers.Take(9).Zip(k.Skip(2), (x, y) => x * y).Sum() %11 %10 == numbers[9];
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
            companiesCache.UpdateList(currentListName,new[]{data});
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
            currentListName = name;
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

    }
}