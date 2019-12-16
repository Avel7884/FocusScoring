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

        private ListResourceConsistencyWrapper<ListData> ListNames;

        private ListFactory listFactory;
        public ListFactory ListFactory
        {
            get => listFactory;
            set
            {
                listFactory = value;
                ListNames = new ListResourceConsistencyWrapper<ListData>(
                    listFactory.GetCachedLists(),_=>ListView.Items.Refresh());
                ListView.ItemsSource = ListNames;
                
                if (ListFactory ==null)
                    throw new TypeLoadException("CompanyList should be initialized before CompanyListsCache.");

                if (ListNames.Count == 0)
                    ListNames.Add(listFactory.Create("NewList"));
                
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
                ListView.SelectedItem.Equals(CompanyList.CurrentList))
                return;
            CompanyList.ShowNewList((ListData)ListView.SelectedItem);
        }

        private string CheckAndAddList(string name, List<string> list)
        {
            
            if (name == "")
                return "Название не может быть пустым";
            
            if (ListNames.Any(x=>x.Name == name))
                return "Лист с данным названием уже существует";

            if(!Manager.AbleToUseMore(1))
                return "Ключ требует продления!";
            
            var companyList = list.Where(inn =>
                        ((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && CompanyList.InnCheckSum(inn)))
                .ToHashSet()
                .ToList();
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

        private void AddList(string name, List<string> innLsit)
        {
            var data = ListFactory.Create(name);
            ListNames.Add(CompanyList.CreateNewList(data,innLsit));
            ListView.SelectedItem = ListNames.Last();
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {            
            if(!CompanyListReady())
                return;            
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var data = (ListData)ListView.SelectedItem;
            ListFactory.DeleteList(data.Name);
            ListNames.Remove(data);
        }

        public bool CompanyListReady()
        {
            var b = CompanyList.IsWorkDone();
            if (!b) MessageBox.Show("Дождитесь обработки списка.");
            return b;
        }
        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog(CheckAndAddList).Show();
        }

        private void UnloadExcel_Click(object sender, RoutedEventArgs e)
        {
            if(!CompanyListReady())
                return;            
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var data = (ListData)ListView.SelectedItem;
            if(Manager.IsBaseMode())
                Excel.BaseExport(data.Name);
            else
                Excel.Export(data.Name);
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            var renameBox = new ListRenameWindow();
            renameBox.Show();
            renameBox.Closed += (o, ev) =>
            {
                if(renameBox.NewName == "")
                    return;
                var name = (ListData)ListView.SelectedItem;
                ListFactory.DeleteList(name.Name);
                ListNames.Remove(name);
            };
        }
    }
}
