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
            Loaded += Init;
            InitializeComponent();
        }

        private void Init(object o, EventArgs args)
        {
            var mainWindow = (MainWindow) ((Grid) Parent).Parent;
            manager = mainWindow.FocusManager;//TODO need to try to come up to better initialization 
            companyList = mainWindow.Companies;
            companiesCache = mainWindow.CompaniesCache;
            ListNames = companiesCache.GetNames();

            if (ListNames.Count == 0)
            {
                companiesCache.UpdateList("NewList", new List<CompanyData>());
                companyList.ShowNewList("NewList");
            }
            else
            {
                ListView.ItemsSource = ListNames;
            }
        }

        public ListOfCompanyLists(CompanyListsCache cache, FocusScoringManager manager, CompanyList companyList)
        {
            InitializeComponent();
            this.manager = manager;
            this.companyList = companyList;
            companiesCache = cache;
            ListNames = companiesCache.GetNames();

            if (ListNames.Count != 0) return;
            companiesCache.UpdateList("NewList", new List<CompanyData>());
            companyList.ShowNewList("NewList");
        }
        
        private List<string> ListNames;
        private CompanyListsCache companiesCache;
        private FocusScoringManager manager; //TODO check if key used more in namecheck
        private CompanyList companyList;

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null)
                return;
            companyList.ShowNewList((string)ListView.SelectedItem);
        }

        private string ButtonAddList(string name, List<string> list)
        {
            
            if (companiesCache.GetNames().Contains(name))
                return "Лист с данным названием уже существует";

            if (name == "")
                return "Название не может быть пустым";
            
            foreach (var inn in list)
                if (!((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                    return inn + " --некорректный инн";
                    
            
            companiesCache.UpdateList(name, list.Select(x=>new CompanyData(x,manager)));
            ListNames.Add(name);
            ListView.Items.Refresh();
            ListView.SelectedItem = ListNames.Last();
            companyList.ShowNewList(name);

            return null;
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            companiesCache.DeleteList(name);
            ListNames.Remove(name);
            ListView.Items.Refresh();
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog(ButtonAddList,companiesCache).Show();
        }
    }

}
