using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shapes;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class CompanyList : UserControl
    {
        public CompanyList()
        {
            //Loaded +=  Init;
            //TODO create it in the constructor
            cache = new ListsCache<string>("SettingsLists");
            InitializeComponent();
        }

        private TextBlock Key;
        private bool ListSetted;
        public FocusKeyManager Manager { get; set; } //TODO Consider refactor to not have it here 
        public ListsCache<string> CompaniesCache{ get; set; }
        public ICompanyFactory CompanyFactory { get; set; }
        public MarkersList markersList;
        private List<Company> currentList;
        public string CurrentListName { get; private set; }
        private BackgroundWorker Worker;//TODO consider use threadpull

        public void ShowNewList(string listName)
        {
            Worker?.CancelAsync();
            
            ListSetted = true;
            CurrentListName = listName;
            TextBlockList.Text = CurrentListName;
            RepopulateColumns();

            currentList = new List<Company>(); //CompaniesCache.GetList(listName).Select(Manager.CreateFromInn).ToList();
            CompanyListView.ItemsSource = currentList;

            ProcessCompanies(listName);

        }

        private void ProcessCompanies(string listName)
        {
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;

            var l = CompaniesCache.GetList(listName);
            foreach (var inn in l)
                Worker.DoWork += (o, e) =>
                {
                    
                    var bv = o as BackgroundWorker;
                    if(bv.CancellationPending) return;
                    var company = CompanyFactory.CreateFromInn(inn);
                    if(bv.CancellationPending) return;
                    bv.ReportProgress(currentList.Count * 100 / l.Count, company);
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (currentList)
                lock (CompanyListView)
                {     
                    currentList.Add(e.UserState as Company);
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerAsync(100000);
        }
        
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if (CompanyListView.SelectedItem == null)
                return;
            markersList.ShowNewMarkers((Company)CompanyListView.SelectedItem);
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

        private void RepopulateColumns()
        {/*
            var gridView = new GridView();
            var originalView = (GridView)CompanyListView.View;
            gridView.Columns.Add(originalView.Columns[0]);
            CompanyListView.View = gridView;*/
         
            var gridView = (GridView)CompanyListView.View;
            for(int i = gridView.Columns.Count-1;i>0;i--)
                gridView.Columns.RemoveAt(i);
            
            EnsureCache();
            //gridView.Columns.Add(new GridViewColumn{CellTemplate = new DataTemplate(new Image{Source = new Binding()})});
            converter = new CompanyToParameterConverter();
            foreach (var setting in cache.GetList(CurrentListName))
            {
                gridView.Columns.Add(new GridViewColumn
                {
                    Header = setting, 
                    DisplayMemberBinding = new Binding
                    {
                        Converter = converter,
                        ConverterParameter = setting
                    }
                });
            }
        }

        private void EnsureCache()
        {
            if(!cache.GetNames().Contains(CurrentListName))
                cache.UpdateList(CurrentListName,new []{"Имя","Инн"});
        }
        
        private void ButtonCompaniesSettings_Click(object s, RoutedEventArgs e)
        {            
            var settingsWindow = new CompanySettings(cache, CurrentListName, 
                CompanyToParameterConverter.GetAvailableParameters(Manager));
            settingsWindow.Show();
            settingsWindow.Closed += (o, a) => RepopulateColumns();
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

            var company = CompanyFactory.CreateFromInn(Inn.Text);
            currentList.Add(company);
            CompaniesCache.UpdateList(CurrentListName,currentList.Select(x=>x.Inn));
            CompanyListView.Items.Refresh();
            FocusKeyUsed.Invoke(this,null);
        }

        private void DeleteCompany_Context(object s, RoutedEventArgs e)
        {
            currentList.Remove((Company)CompanyListView.SelectedItem);
            CompaniesCache.UpdateList(CurrentListName,currentList.Select(x=>x.Inn));
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
        private ListsCache<string> cache;
        private CompanyToParameterConverter converter { get; set; }

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

        public event Action<object, EventArgs> FocusKeyUsed;

        public void CheckCurrentList()
        {
            CompanyListView.ItemsSource = currentList;
            CompanyListView.Items.Refresh();
            
            //TODO DRYish
            var pastList = currentList;
            currentList = new List<Company>();
            CompanyListView.ItemsSource = currentList;
            CompanyListView.Items.Refresh();
            
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;

            foreach (var company in pastList)
                Worker.DoWork += (o, e) =>
                {
                    var bv = o as BackgroundWorker;
                    if(bv.CancellationPending) return;
                    company.ForcedMakeScore(); 
                    if(bv.CancellationPending) return;
                    bv.ReportProgress(currentList.Count * 100 / pastList.Count, company);
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (currentList)
                lock (CompanyListView)
                {     
                    currentList.Add(e.UserState as Company);
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerAsync(100000);
        }
    }
}
