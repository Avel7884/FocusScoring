using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FocusScoringGUI
{
    public partial class CompanyList : UserControl
    {
        public CompanyList()
        {
            //Loaded +=  Init;
            //TODO create it in the constructor
            InitializeComponent();
        }

        private TextBlock Key;
        private bool ListSetted;
        public FocusKeyManager Manager { get; set; } //TODO Consider refactor to not have it here 
        public ListsCache<CompanyData> CompaniesCache{ get; set; }
        public ICompanyFactory CompanyFactory { get; set; }
        public MarkersList markersList;
        public List<CompanyData> CurrentList { get; private set; }
        public string CurrentListName { get; private set; }
        private ICollectionWorker Worker;//TODO consider use threadpull
        public ListsCache<string> SettingsCache { get; set; }
        private CompanySettings SettingsWindow;
        private CompanyToParameterConverter converter { get; set; }

        public void ShowNewList(string listName)
        {
            Worker?.StopWork();
            
            ListSetted = true;
            CurrentListName = listName;
            TextBlockList.Text = CurrentListName;
            
            CurrentList = CompaniesCache.GetList(CurrentListName);
            RepopulateColumns();

            //currentList = new List<CompanyData>(); //CompaniesCache.GetList(listName).Select(Manager.CreateFromInn).ToList();
            
            CompanyListView.ItemsSource = CurrentList;
            CompanyListView.Items.Refresh();
            
            //ProcessCompanies(listName);

            
            
        }

        public bool IsWorkDone()
        {
            return !(Worker?.IsBusy() ?? false);
        }
        
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if (CompanyListView.SelectedItem == null)
                return;
            var data = CompanyListView.SelectedItem as CompanyData;
            if(data.CLight == Light.Loading)
                return;
            //if(!Manager.IsBaseMode())
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
        {
            EnsureCache();
            var settings = SettingsCache.GetList(CurrentListName);

            var gridView = (GridView)CompanyListView.View;
            var lastColumn = 1;//Manager.IsBaseMode() ? 0 : 1;
            for(int i = gridView.Columns.Count-1;i >= lastColumn;i--)
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
            if(!SettingsCache.GetNames().Contains(CurrentListName))
                SettingsCache.UpdateList(CurrentListName,new []{"Имя","Инн"});//TODO use something stat
        }
        
        private void ButtonCompaniesSettings_Click(object s, RoutedEventArgs e)
        {
            if(Worker!= null && Worker.IsBusy())
            {
                MessageBox.Show("Дождитесь окончания обработки списка.");
                return;
            }
            
            if (SettingsWindow != null && SettingsWindow.IsLoaded)
            {
                SettingsWindow.Focus();
                return;                
            }
            SettingsWindow = new CompanySettings(SettingsCache, CurrentListName, 
                CompanyData.GetAvailableParameters(Manager));
            SettingsCache.DeleteList(CurrentListName);
            SettingsWindow.Show();
            SettingsWindow.Closed += (o, a) =>
            {
                if(!SettingsWindow.OkClicked)
                    return;
                RepopulateColumns();
                
                if(!(o as CompanySettings).OkClicked)
                    return;
                var settings = SettingsCache.GetList(CurrentListName);
                var errors = new List<string>();
                Worker = new CollectionWorker("Прогресс настроек");

                Worker.ProgressChanged += (o2, e2) =>
                {
                    var (index, data) = ((int, CompanyData)) e2.UserState;
                    lock (CurrentList)
                    lock (CompanyListView)
                    {     
                        CurrentList[index] = data;
                        CompanyListView.Items.Refresh();
                    }
                };

                Worker.RunWorkerCompleted += (o2, e2) =>
                {
                    if (errors.Count != 0) MessageBox.Show("Произошли следующие ошибки: " + string.Join(" ", errors));
                    CompaniesCache.UpdateList(CurrentListName, CurrentList);};
                
                Worker.WorkOn(CurrentList, (i, data) =>
                {
                    lock (CompanyFactory)
                    {
                        var company = data.Source ?? CompanyFactory.CreateFromInn(data.Inn);
                        if(CompanyFactory.Exception != null) 
                            errors.Add(CompanyFactory.Exception.Message);
                        return (i,new CompanyData(company, settings, Manager.IsBaseMode()));
                    } 
                });
                /*

                CurrentList = CurrentList
                    .Select(x => x.Source ?? CompanyFactory.CreateFromInn(x.Inn))
                    .Select(x => new CompanyData(x,settings)).ToList();
                CompanyListView.ItemsSource = CurrentList;
                CompaniesCache.UpdateList(CurrentListName,CurrentList);*/
            };
        }

        private void ButtonAddCompany_Click(object s, RoutedEventArgs e)
        {
            if(Worker!= null && Worker.IsBusy())
            {
                MessageBox.Show("Дождитесь окончания обработки списка.");
                return;
            }
            
            
            if (!ListSetted)
            {
                MessageBox.Show("Необходимо выбрать или создать список!");
                return;
            }
            
            if (CurrentList.Select(x => x.Inn).Contains(Inn.Text))
            {
                MessageBox.Show("Компания уже имеется в списке");
                return;
            }

            if (!InnCheckSum(Inn.Text) || string.IsNullOrWhiteSpace(Inn.Text))
            {
                MessageBox.Show("Не корректный ИНН");
                return;
            }

            if (!Manager.AbleToUseMore(1))
            {
                MessageBox.Show("Ключ требует продления!");
                return;
            }


            var mb = MessageBox.Show("Будет отправлен запрос.");
            if(mb != MessageBoxResult.OK)
                return;

            var company = CompanyFactory.
                CreateFromInn(Inn.Text);
            if (CompanyFactory.Exception != null)
                MessageBox.Show("Ошибка при обработке:" + CompanyFactory.Exception.Message);
                
            var companyData = new CompanyData(company,SettingsCache.GetList(CurrentListName),Manager.IsBaseMode());
            CurrentList.Add(companyData);
            CompaniesCache.UpdateList(CurrentListName,CurrentList);
            CompanyListView.Items.Refresh();
            FocusKeyUsed.Invoke(this,null);
        }

        private void DeleteCompany_Context(object s, RoutedEventArgs e)
        {
            CurrentList.Remove((CompanyData)CompanyListView.SelectedItem);
            CompaniesCache.UpdateList(CurrentListName,CurrentList);
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
        public void CheckCurrentList()
        {
            if(Worker!= null && Worker.IsBusy())
                return;
            
            var errors = new List<string>();
            var settings = SettingsCache.GetList(CurrentListName);
            CompanyListView.ItemsSource = CurrentList;
            CompanyListView.Items.Refresh();
            
            Worker = new CollectionWorker();
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (CurrentList)
                lock (CompanyListView)
                {
                    //var data = (CompanyData) e.UserState;
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerCompleted += (o,e) =>
            {
                if (errors.Count != 0) MessageBox.Show("Произошли следующие ошибки: " + string.Join(" ", errors));

                CompaniesCache.UpdateList(CurrentListName, CurrentList);
                MessageBox.Show($"Проверка листа завершена загружено {CurrentList.Count-errors.Count} компаний");
            };
            
            Worker.WorkOn(CurrentList,((i, data) =>
            {
                if (data.Source == null)
                    data.Source = CompanyFactory.CreateFromInn(data.Inn);
                if (CompanyFactory.Exception != null)
                    errors.Add(data.Inn +": "+ CompanyFactory.Exception.Message+"\t\n");
                data.Recheck(settings,Manager.IsBaseMode());
                return data;
            }));
        }
        public event Action<object, EventArgs> FocusKeyUsed;

        public void CreateNewList(string name, IList<string> listInn)
        {
            ListSetted = true;
            CurrentListName = name;
            TextBlockList.Text = CurrentListName;
            
            RepopulateColumns();
            FillList(name, listInn);
        }

        private void FillList(string name, IList<string> listInn)
        {
            var settings = SettingsCache.GetList(CurrentListName);
            var errors = new List<string>();
            CurrentList = new List<CompanyData>();
            foreach (var inn in listInn)
            {
                var data = new CompanyData {CLight = Light.Loading, Inn = inn};
                data.InitParameters(settings);//TODO Make better
                CurrentList.Add(data);
            }    
            
            CompanyListView.ItemsSource = CurrentList;
            CompanyListView.Items.Refresh();
            Worker = new CollectionWorker();
            
            Worker.ProgressChanged += (o,e) =>
            {
                lock (CurrentList)
                lock (CompanyListView)
                {     
                    var (data, index) = ((CompanyData, int)) e.UserState;
                    CurrentList[index] = data;
                    CompanyListView.Items.Refresh();
                }
            };
            
            Worker.RunWorkerCompleted += (o,e) =>
            {
                if (errors.Count != 0) MessageBox.Show("Произошли следующие ошибки: " + string.Join(" ", errors));
                CompaniesCache.UpdateList(name, CurrentList);
                FocusKeyUsed?.Invoke(this,new EventArgs());
                MessageBox.Show($"Загрузка листа завершена загружено {CurrentList.Count-errors.Count} компаний");
            };
            
            Worker.WorkOn(listInn, (i, inn) =>
            {
                var data = new CompanyData(CompanyFactory.CreateFromInn(inn),settings,Manager.IsBaseMode());
                if (CompanyFactory.Exception != null)
                    errors.Add(data.Inn + ": " + CompanyFactory.Exception.Message);//MessageBox.Show("Ошибка при обработке:" + CompanyFactory.Exception.Message);
                //data.InitLight(data.Source.Score);//TODO Refactor here!
                return (data, i);
            });
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
        private void CopyExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            var element = (UIElement) e.Source;
            var grid = (GridView)CompanyListView.View;
            
            //Clipboard.SetText((CompanyData)CompanyListView.SelectedItem);            
        }
    }
}
