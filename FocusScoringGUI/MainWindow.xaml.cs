using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;
using Microsoft.Win32;

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

        private FocusScoringManager manager;
        //        public Scorer scorer;
        private ListMonitorer monitorer;
        RegistryKey key;
        private HashSet<string> monitoredInns;
        //public string Inn { get; set; }

        public MainWindow(FocusScoringManager manager)
        {
            var a = this.Owner;
            InitializeComponent();
            this.manager = manager;
            companiesCache = CompanyListsCache.Create();
            ListNames = companiesCache.GetNames();

            monitorer = null;// manager.StartMonitor();
            monitoredInns =
                new HashSet<string>(companiesCache.GetAllCompanies().Where(x => x.Autoupdate).Select(x => x.Inn));

            //monitorer.DataUpdate += MonitorerOnDataUpdate;

            if (ListNames.Count == 0)
            {
                var list = new List<CompanyData>();
                CurrentList = list;
                companiesCache.UpdateList("NewList", list);
            }

            currentListName = ListNames.First();

            ListView.SelectedItem = currentListName;
            ListView.ItemsSource = ListNames;
            CurrentList = companiesCache.GetList(currentListName);
            TextBlockList.Text = currentListName;
            CompanyList.ItemsSource = CurrentList;

            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();

            KeyCounter.Text = "Ключ: использовано " + manager.Usages;

            //MarkersTable.ItemsSource = new Company[0];
        }

        private void MonitorerOnDataUpdate(object o, ListMonitorer.MonitorEventArgs args)
        {
            foreach (var inn in args.ChangedCompanies)
                manager.CreateFromInn(inn).MakeScore();
            //TODO bug here, need to use force download update
        }

        //private void Inn_KeyDown(object sender, KeyEventArgs e)
        //{
        //    e.Handled = !(Char.IsDigit(e.Key.ToString(), 0));
        //}

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
        //private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var cmbbx = (ComboBox)sender;
        //    var selectedI = (ComboBoxItem)cmbbx.SelectedItem;
        //    if (selectedI.Content.ToString() == "ИНН Компании")
        //    {
        //        //discriptionBlock.Text = "Введите ИНН контрагента для формирования отчета";
        //        return;
        //    }
        //    if (selectedI.Content.ToString() == "Банкротство физлица")
        //    {
        //        discriptionBlock.Text = "Введите ФИО/ИНН/СНИЛС физлица";
        //        return;
        //    }
        //    if (selectedI.Content.ToString() == "Паспорт")
        //    {
        //        discriptionBlock.Text = "Введите серию и номер паспорта(ов) через запятую. Например 1111 111111, 222222222";
        //        return;
        //    }
        //}

        private void FocusWindowShow(object sender, RoutedEventArgs e)
        {
            FocusKeyWindow fkw;
            using (key = Registry.CurrentUser.OpenSubKey(@"Software\FocusScoring"))
                fkw = new FocusKeyWindow(Coder.Decode(key.GetValue("fkey").ToString()));
            fkw.Owner = this;
            fkw.Show();

            fkw.KeyAccepted += (o, a) =>
            this.manager = fkw.Manager;
        }
    }
}
