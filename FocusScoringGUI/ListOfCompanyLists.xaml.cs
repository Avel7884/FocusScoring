using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class ListOfCompanyLists : UserControl
    {

        public ListOfCompanyLists()
        {
            InitializeComponent();
        }

        private void Init(object o, EventArgs args)
        {/*
            var mainWindow = (MainWindow) ((Grid) Parent).Parent;
            manager = mainWindow.FocusManager;//TODO need to try to come up to better initialization 
            companyList = mainWindow.Companies;
            companiesCache = mainWindow.CompaniesCache;*/
        }
        //TODO clear
        public ListOfCompanyLists(ListsCache<string> cache, FocusKeyManager manager, CompanyList companyList)
        {
            InitializeComponent();
            this.Manager = manager;
            this.CompanyList = companyList;
            CompaniesCache = cache;
            ListNames = CompaniesCache.GetNames();

            if (ListNames.Count != 0) return;
            CompaniesCache.UpdateList("NewList", new List<string>());
            companyList.ShowNewList("NewList");
        }

        private List<string> ListNames;

        private ListsCache<string> companiesCache;
        public ListsCache<string> CompaniesCache
        {
            get => companiesCache;
            set
            {
                companiesCache = value;
                ListNames = companiesCache.GetNames();

                if (CompaniesCache ==null)
                    throw new TypeLoadException("CompanyList should be initialized before CompanyListsCache.");
                
                if (ListNames.Count == 0)
                    companiesCache.UpdateList("NewList", new List<string>());
                ListView.ItemsSource = ListNames;
                CompanyList.ShowNewList(ListNames.First());
            }
        }

        public FocusKeyManager Manager{ get; set; } //TODO check if key used more in namecheck

        //private CompanyList companyList;
        public CompanyList CompanyList { get; set; }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null || 
                ListView.SelectedItem.Equals(CompanyList.CurrentListName))
                return;
            CompanyList.ShowNewList((string)ListView.SelectedItem);
        }

        private string CheckAndAddList(string name, List<string> list)
        {
            if (CompaniesCache.GetNames().Contains(name))
                return "Лист с данным названием уже существует";

            if (name == "")
                return "Название не может быть пустым";

            list = list.Where(inn =>
                    ((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                .ToList();
            
            /*

            foreach (var inn in list)
                if (!((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                    return inn + " --некорректный инн";*/

            var usagesNeeded = CountFocusKeyUsages(list);
            if (usagesNeeded == 0)
            {
                AddList(name,list);
                return null;
            }
            
            var mb = MessageBox.Show($"Ключ будет использован {usagesNeeded} раз.");
            if (mb != MessageBoxResult.OK) 
                return "";
            
            AddList(name,list);
            return null;
        }

        private int CountFocusKeyUsages(List<string> companies)
        {
            return companies.Sum(x => Manager.IsCompanyUsed(x) ? 1 : 0);
        }

        private void AddList(string name, List<string> list)
        {
            CompaniesCache.UpdateList(name, list);
            ListNames.Add(name);
            ListView.Items.Refresh();
            ListView.SelectedItem = ListNames.Last();
            CompanyList.ShowNewList(name);
            FocusKeyUsed.Invoke(this,null);
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            CompaniesCache.DeleteList(name);
            //TODO Delete list settings as well!
            ListNames.Remove(name);
            ListView.Items.Refresh();
        }
        
        public event Action<object, EventArgs> FocusKeyUsed;

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog(CheckAndAddList,CompaniesCache).Show();
        }
    }

}
