using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class CompanyList : UserControl
    {
        public CompanyList()
        {
            Loaded +=  Init;
            InitializeComponent();
        }
        
        private void Init(object o, EventArgs args)
        {
            var mainWindow = (MainWindow) ((Grid) Parent).Parent;
            companiesCache = mainWindow.CompaniesCache;
            manager = mainWindow.FocusManager;
            markersList = mainWindow.Markers;
            Key = mainWindow.KeyCounter;//TODO make dedicated object for this instead
            ListSetted = false;
            currentList = new List<CompanyData>();
            CompanyListView.ItemsSource = currentList;
            currentListName = "";
            mainWindow.Companies = this;//TODO maybe remove this line 
        }
        
        public CompanyList(CompanyListsCache cache, FocusScoringManager manager, MarkersList markersList)
        {
            companiesCache = cache;
            this.manager = manager;
            this.markersList = markersList;
            currentList = new List<CompanyData>();
            CompanyListView.ItemsSource = currentList;
            currentListName = "";
            InitializeComponent();
        }

        private TextBlock Key;
        private bool ListSetted;
        private CompanyListsCache companiesCache;
        private FocusScoringManager manager;
        private MarkersList markersList;
        private List<CompanyData> currentList;
        private string currentListName;

        public void ShowNewList(string listName)
        {
            ListSetted = true;
            currentList = companiesCache.GetList(listName);
            CompanyListView.ItemsSource = currentList;
            currentListName = listName;
            TextBlockList.Text = currentListName;
        }
        
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if (CompanyListView.SelectedItem == null)
                return;
            markersList.ShowNewMarkers((CompanyData)CompanyListView.SelectedItem);
        }

        private static readonly int[] k = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        public static bool InnCheckSum(string inn)//TODO remove terreble naming it makes
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

        private void ButtonCheckList_Click(object s, RoutedEventArgs e)
        {
            var dialogResult =
                MessageBox.Show(
                    "Ключ будет использован не более " + currentList.Count(x => !x.IsChecked) + " раз",
                    "Предупреждение", MessageBoxButton.OKCancel);
            if (dialogResult == MessageBoxResult.Cancel)
                return;
            //var force = CurrentList.Any(x => !x.IsChecked);

            foreach (var data in currentList)
                data.Check(manager);
            Key.Text = "Ключ: использовано " + manager.Usages;
            companiesCache.UpdateList(currentListName, currentList);
            CompanyListView.Items.Refresh();
        }

        private void Check_Context(object s, RoutedEventArgs e)
        {
            if (CompanyListView.SelectedItem == null)
                return;
            var data = (CompanyData)CompanyListView.SelectedItem;
            data.IsChecked = true;

            data.Check(manager);
            
            Key.Text = "Ключ: использовано " + manager.Usages;
            companiesCache.UpdateList(currentListName, currentList);
            CompanyListView.Items.Refresh();
        }

        private void ButtonAddCompany_Click(object s, RoutedEventArgs e)
        {
            if (!ListSetted)
            {
                MessageBox.Show("Необходимо выбрать или создать список!");
                return;
            }
            
            if (currentList.Select(x => x.Inn).Contains(Inn.Text))
            {
                MessageBox.Show("Компания уже имеется в списке");
                return;
            }

            if (!InnCheckSum(Inn.Text) || string.IsNullOrWhiteSpace(Inn.Text))
            {
                MessageBox.Show("Не корректный ИНН");
                return;
            }

            var data = new CompanyData(Inn.Text, manager);
            currentList.Add(data);
            companiesCache.UpdateList(currentListName, new[] { data });
            CompanyListView.Items.Refresh();
        }

        private void DeleteCompany_Context(object s, RoutedEventArgs e)
        {
            currentList.Remove((CompanyData)CompanyListView.SelectedItem);
            companiesCache.DeleteList(currentListName);
            companiesCache.UpdateList(currentListName,currentList);
            CompanyListView.Items.Refresh();
        }
        
        //TODO rewrite everything following
        private void Inn_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try { e.Handled = !(Char.IsDigit(e.Text, 0)); }
            catch { }    //
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
        
        
        //Some stackoverflow shit. Not my.
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction, sender);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction,object sender)
        {
            var seender = sender as ListView;
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(seender.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            try { dataView.SortDescriptions.Add(sd); }
            catch
            {
                return;
            }
            dataView.Refresh();
        }
    }
}
