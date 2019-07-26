using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {

        private void ListSelected_Click(object s, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null)
                return;
            currentListName = (string)ListView.SelectedItem;
            CurrentList = companiesCache.GetList(currentListName);
            TextBlockList.Text = currentListName;
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
            //CheckList.IsEnabled = CurrentList.Any(x => !x.IsChecked);
            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();
        }

        private string ButtonAddList(string name, List<string> list)
        {
            foreach (var inn in list)
                if (!((inn.Length == 10 || inn.Length == 12) && inn.All(char.IsDigit) && InnCheckSum(inn)))
                    return inn + " --некорректный инн";
                    
            
            CurrentList = list.Select(x=>new CompanyData(x,manager)).ToList();
            currentListName = name;
            companiesCache.UpdateList(currentListName, CurrentList);
            ListNames.Add(name);
            ListView.Items.Refresh();
            TextBlockList.Text = name;
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
            MarkersList.ItemsSource = null;
            ListView.SelectedItem = ListNames.Last();
            
            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();

            return null;
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (ListView.SelectedItem == null || ListNames.Count <= 1)
                return;
            var name = (string)ListView.SelectedItem;
            companiesCache.DeleteList(name);
            ListNames.Remove(name);
            currentListName = ListNames.First();
            CurrentList = companiesCache.GetList(currentListName);
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
            TextBlockList.Text = currentListName;
            ListView.Items.Refresh();
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog(ButtonAddList,companiesCache).Show();
        }
    }
}