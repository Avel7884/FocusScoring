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
using System.Windows.Media;
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
        public ListsCache<CompanyData> CompaniesCache{ get; set; }
        public ICompanyFactory CompanyFactory { get; set; }
        public MarkersList markersList;
        private List<CompanyData> currentList;
        public string CurrentListName { get; private set; }
        private BackgroundWorker Worker;//TODO consider use threadpull
        private ListsCache<string> cache;
        private Window SettingsWindow;
        private CompanyToParameterConverter converter { get; set; }

        public void ShowNewList(string listName)
        {
            Worker?.CancelAsync();
            
            ListSetted = true;
            CurrentListName = listName;
            TextBlockList.Text = CurrentListName;
            
            currentList = CompaniesCache.GetList(CurrentListName);
            RepopulateColumns();

            //currentList = new List<CompanyData>(); //CompaniesCache.GetList(listName).Select(Manager.CreateFromInn).ToList();
            
            CompanyListView.ItemsSource = currentList;
            CompanyListView.Items.Refresh();
            
            //ProcessCompanies(listName);

            
            
        }

        public bool IsWorkDone()
        {
            return !(Worker?.IsBusy ?? false);
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
                    //var company = CompanyFactory.CreateFromInn(inn);
                    if(bv.CancellationPending) return;
                    //bv.ReportProgress(currentList.Count * 100 / l.Count, company);
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (currentList)
                lock (CompanyListView)
                {     
                    //currentList.Add(e.UserState as Company);
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerAsync(100000);
        }
        
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if (CompanyListView.SelectedItem == null)
                return;
            var data = CompanyListView.SelectedItem as CompanyData;
            markersList.ShowNewMarkers(data.Source ?? data.MakeSource(CompanyFactory));
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

        private void RepopulateColumns()//bool resetCompanies = false)
        {/*
            var gridView = new GridView();
            var originalView = (GridView)CompanyListView.View;
            gridView.Columns.Add(originalView.Columns[0]);
            CompanyListView.View = gridView;*/
            EnsureCache();
            var settings = cache.GetList(CurrentListName);

            /*if (resetCompanies)
            {
                currentList = currentList
                    .Select(x => x.Source ?? CompanyFactory.CreateFromInn(x.Inn))
                    .Select(x => new CompanyData(x,settings)).ToList();
                CompanyListView.ItemsSource = currentList;
                CompaniesCache.UpdateList(CurrentListName,currentList);
            }*/

            var gridView = (GridView)CompanyListView.View;
            for(int i = gridView.Columns.Count-1;i>0;i--)
                gridView.Columns.RemoveAt(i);
            
            //gridView.Columns.Add(new GridViewColumn{CellTemplate = new DataTemplate(new Image{Source = new Binding()})});
            converter = new CompanyToParameterConverter();
            for (var i = 0; i < settings.Count; i++)
            {
                gridView.Columns.Add(new GridViewColumn
                {
                    Header = settings[i],
                    DisplayMemberBinding = new Binding
                    {
                        Converter = converter,
                        ConverterParameter = i
                    }
                });
            }
            
            CompanyListView.Items.Refresh();
        }

        private void EnsureCache()
        {
            if(!cache.GetNames().Contains(CurrentListName))
                cache.UpdateList(CurrentListName,new []{"Имя","Инн"});//TODO use something stat
        }
        
        private void ButtonCompaniesSettings_Click(object s, RoutedEventArgs e)
        {
            if (SettingsWindow != null && SettingsWindow.IsLoaded)
            {
                SettingsWindow.Focus();
                return;                
            }
            SettingsWindow= new CompanySettings(cache, CurrentListName, 
                CompanyData.GetAvailableParameters(Manager));
            cache.DeleteList(CurrentListName);
            SettingsWindow.Show();
            SettingsWindow.Closed += (o, a) =>
            {
                if(!(o as CompanySettings).OkClicked)
                    return;
                var settings = cache.GetList(CurrentListName);
                currentList = currentList
                    .Select(x => x.Source ?? CompanyFactory.CreateFromInn(x.Inn))
                    .Select(x => new CompanyData(x,settings)).ToList();
                CompanyListView.ItemsSource = currentList;
                CompaniesCache.UpdateList(CurrentListName,currentList);
                RepopulateColumns();
            };
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

            var mb = MessageBox.Show("Будет отправлен запрос.");
            if(mb != MessageBoxResult.OK)
                return;

            var company = CompanyFactory.CreateFromInn(Inn.Text);
            var companyData = new CompanyData(company,cache.GetList(CurrentListName));
            currentList.Add(companyData);
            CompaniesCache.UpdateList(CurrentListName,currentList);
            CompanyListView.Items.Refresh();
            FocusKeyUsed.Invoke(this,null);
        }

        private void DeleteCompany_Context(object s, RoutedEventArgs e)
        {
            currentList.Remove((CompanyData)CompanyListView.SelectedItem);
            CompaniesCache.UpdateList(CurrentListName,currentList);
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

        public event Action<object, EventArgs> FocusKeyUsed;

        public void CheckCurrentList()
        {
            var settings = cache.GetList(CurrentListName);

            /*var stub = new string[settings.Count];
            for (int j = 0; j < settings.Count; j++)
                stub[j] = "";

            foreach (var inn in currentList)
            {
                var data = new CompanyData {CLight = Light.Red, Inn = inn};
                data.InitParameters(settings);//TODO Make better
                currentList.Add(data);
            }    */
            
            CompanyListView.ItemsSource = currentList;
            CompanyListView.Items.Refresh();
            
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            
            foreach (var (i,data) in Enumerable.Range(0,currentList.Count).Zip(currentList,ValueTuple.Create))
                Worker.DoWork += (o, e) =>
                {
                    var bv = o as BackgroundWorker;
                    if(bv.CancellationPending) return;
                    if (data.Source == null)
                        data.Source = CompanyFactory.CreateFromInn(data.Inn);
                    data.Recheck(settings);
                    if(bv.CancellationPending) return;
                    bv.ReportProgress((i/currentList.Count), data);
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (currentList)
                lock (CompanyListView)
                {     
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerCompleted += (o,e) =>
                CompaniesCache.UpdateList(CurrentListName,currentList);
            
            Worker.RunWorkerAsync(100000);
        }

        public void CreateNewList(string name, IList<string> listInn)
        {
            ListSetted = true;
            CurrentListName = name;
            TextBlockList.Text = CurrentListName;
            RepopulateColumns();
            
            /*currentList = listInn.Select(CompanyFactory.CreateFromInn)
                .Select(c => new CompanyData(c, new List<string>() {"Имя", "Инн"})).ToList();*/
            
            FillList(name, listInn);
        }

        private void FillList(string name, IList<string> listInn)
        {
            var settings = cache.GetList(CurrentListName);
            
            var pastList = currentList;
            currentList = new List<CompanyData>();

            /*var stub = new string[settings.Count];
            for (int j = 0; j < settings.Count; j++)
                stub[j] = "";*/

            foreach (var inn in listInn)
            {
                var data = new CompanyData {CLight = Light.Red, Inn = inn};
                data.InitParameters(settings);//TODO Make better
                currentList.Add(data);
            }    
            
            CompanyListView.ItemsSource = currentList;
            CompanyListView.Items.Refresh();
            
            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            
            foreach (var (i,inn) in Enumerable.Range(0,listInn.Count).Zip(listInn,ValueTuple.Create))
                Worker.DoWork += (o, e) =>
                {
                    var bv = o as BackgroundWorker;
                    if(bv.CancellationPending) return;
                    var data = new CompanyData(CompanyFactory.CreateFromInn(inn),settings);
                    data.InitLight(data.Source.Score);//TODO Refactor here!
                    if(bv.CancellationPending) return;
                    bv.ReportProgress(currentList.Count * 100 / pastList.Count, (data,i));
                };
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (currentList)
                lock (CompanyListView)
                {     
                    var (data, index) = ((CompanyData, int)) e.UserState;
                    currentList[index] = data;
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerCompleted += (o,e) =>
                CompaniesCache.UpdateList(name,currentList);
            
            Worker.RunWorkerAsync(100000);
        }

        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var element = (UIElement) e.Source;
            var grid = (GridView)CompanyListView.View;
            
            //Clipboard.SetText((CompanyData)CompanyListView.SelectedItem);            
        }
    }
}
