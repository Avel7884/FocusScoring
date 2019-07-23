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
            CheckList.IsEnabled = CurrentList.Any(x => !x.IsChecked);
        }

        private void ButtonAddList(string name, List<CompanyData> list)
        { 
            CurrentList = list;
            currentListName = name;
            companiesCache.UpdateList(currentListName, list);
            ListNames.Add(name);
            ListView.Items.Refresh();
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
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