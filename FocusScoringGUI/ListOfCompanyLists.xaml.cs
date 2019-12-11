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

        private List<string> ListNames;

        private ListsCache<CompanyData> companiesCache;
        public ListsCache<CompanyData> CompaniesCache
        {
            get => companiesCache;
            set
            {
                companiesCache = value;
                ListNames = companiesCache.GetNames();

                if (CompaniesCache ==null)
                    throw new TypeLoadException("CompanyList should be initialized before CompanyListsCache.");

                if (ListNames.Count == 0)
                {
                    companiesCache.UpdateList("NewList", new List<CompanyData>());
                    ListNames.Add("NewList");                    
                }
                ListView.ItemsSource = ListNames;
                CompanyList.ShowNewList(ListNames.First());
            }
        }

        public FocusKeyManager Manager{ get; set; } 
        public IExporter Excel { get; set; }
        public CompanyList CompanyList { get; set; }

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if(!CompanyListReady())
                return;            
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


            var companyList = list.Where(inn =>
                        ((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                .ToHashSet()
                .ToList();
            
            if(!Manager.AbleToUseMore(1))
                return "Ключ требует продления!";

            /*

            foreach (var inn in list)
                if (!((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                    return inn + " --некорректный инн";*/

            var usagesNeeded = Manager.IsCompanyUsed(list);//TODO use ienumerable instead
            if (usagesNeeded == 0)
            {
                AddList(name,companyList);
                return null;
            } 
            
            if (!Manager.AbleToUseMore(usagesNeeded))
                return "Непроверенных компаний больше чем осталось использовний ключа. Уменьшите список или продлите ключ.";
            
            var mb = MessageBox.Show($"Ключ будет использован {usagesNeeded} раз.",
                "Внимание", MessageBoxButton.YesNo);
            if (mb != MessageBoxResult.Yes) 
                return "";
            
            AddList(name,companyList);
            return null;
        }

        private void AddList(string name, List<string> list)
        {
            if(!CompanyListReady())
                return;            
            ListNames.Add(name);
            ListView.Items.Refresh();
            ListView.SelectedItem = ListNames.Last();
            CompanyList.CreateNewList(name,list);
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {            
            if(!CompanyListReady())
                return;            
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            CompaniesCache.DeleteList(name);
            //TODO Delete list settings as well!
            ListNames.Remove(name);
            ListView.Items.Refresh();
        }

        public bool CompanyListReady()
        {
            var b = CompanyList.IsWorkDone();
            if (!b) MessageBox.Show("Дождитесь обработки списка.");
            return b;
        }
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog(CheckAndAddList,CompaniesCache).Show();
        }

        private void UnloadExcel_Click(object sender, RoutedEventArgs e)
        {
            if(!CompanyListReady())
                return;            
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            if(Manager.IsBaseMode())
                Excel.BaseExport(name);
            else
                Excel.Export(name);
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            var renameBox = new ListRenameWindow();
            renameBox.Closed += (o, ev) =>
            {
                var name = (string)ListView.SelectedItem;
                companiesCache.UpdateList(renameBox.NewName,CompaniesCache.GetList(name));
                companiesCache.DeleteList(name);
                ListNames.Remove(name);
                ListNames.Add(renameBox.NewName);
            };
        }
    }

}
