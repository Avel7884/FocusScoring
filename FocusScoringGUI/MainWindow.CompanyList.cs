using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if (CompanyList.SelectedItem == null)
                return;
            var companyData = ((CompanyData)CompanyList.SelectedItem);
            TextBlockName.Text = companyData.Name;
            if (companyData.IsChecked)
            {
                companyData.Check(manager);
                MarkersList.ItemsSource = companyData.Company.Markers.Select(MarkerSubData.Create);
            }
            else MarkersList.ItemsSource = null;

            CompanyList.Items.Refresh();
            MarkersList.Items.Refresh();
        }

        private readonly int[] k = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        private bool InnCheckSum(string inn)
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

        private void ButtonCheckList_Click(object s, RoutedEventArgs e)
        {
            var force = CurrentList.Any(x => !x.IsChecked);

            foreach (var data in CurrentList)
                data.Check(manager);
            KeyCounter.Text = "Ключ: использовано " + manager.Usages;
            companiesCache.UpdateList(currentListName, CurrentList);
            CompanyList.Items.Refresh();
            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();
        }

        private async void CheckBoxAutoUpdate_Click(object o, EventArgs e)
        {
            if (CurrentList.Any(x => !x.Autoupdate))
                CheckBoxAutoUpdate_Checked();
            else
                CheckBoxAutoUpdate_Unchecked();

        }

        private void CheckBoxAutoUpdate_Checked()
        {

            monitorer.Update(CurrentList.Select(x => x.Inn));
            monitoredInns.UnionWith(CurrentList.Select(x => x.Inn));
            foreach (var data in CurrentList)
                data.Autoupdate = true;
            companiesCache.UpdateList(currentListName, CurrentList);

        }

        private void CheckBoxAutoUpdate_Unchecked()
        {
            monitoredInns.ExceptWith(CurrentList.Select(y => y.Inn));
            monitorer.Update(monitoredInns, false);
            foreach (var data in CurrentList)
                data.Autoupdate = false;
            companiesCache.UpdateList(currentListName, CurrentList);
        }

        private void SwitchAutoUpdate_Context(object s, RoutedEventArgs e)
        {
            //TODO Warning here!

            var data = (CompanyData)CompanyList.SelectedItem;
            data.Autoupdate = !data.Autoupdate;
            companiesCache.UpdateList(currentListName, new[] { data });
            RefreshCheckBoxAutoUpdate();

            if (data.Autoupdate)
            {
                monitoredInns.Add(data.Inn);
                monitorer.Update(new[] { data.Inn });
            }
            else
            {
                monitoredInns.Remove(data.Inn);
                monitorer.Update(monitoredInns, false);
            }
        }

        private void Check_Context(object s, RoutedEventArgs e)
        {
            var data = (CompanyData)CompanyList.SelectedItem;
            if (data.IsChecked)
                data.IsChecked = true;
            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();
        }

        private void RefreshCheckButton()
        { //TODO remove, maybe
            if (CurrentList.All(x => x.IsChecked))
            {
                //CheckList.IsEnabled = false;
                AutoUpdate.IsEnabled = true;
            }
            else
            {
                //CheckList.IsEnabled = true;
                AutoUpdate.IsEnabled = false;
            }
        }

        private void RefreshCheckBoxAutoUpdate()
        {
            if (CurrentList.All(x => x.Autoupdate))
                AutoUpdate.IsChecked = true;
            else if (CurrentList.All(x => !x.Autoupdate))
                AutoUpdate.IsChecked = false;
            else
                AutoUpdate.IsChecked = null;
        }

        private void ButtonAddCompany_Click(object s, RoutedEventArgs e)
        {
            if (CurrentList.Select(x => x.Inn).Contains(Inn.Text))
            {
                MessageBox.Show("Компания имеется в списке");
                return;
            }

            if (!InnCheckSum(Inn.Text) || string.IsNullOrWhiteSpace(Inn.Text))
            {
                MessageBox.Show("Не корректный ИНН");
                return;
            }

            var data = new CompanyData(Inn.Text, manager);
            CurrentList.Add(data);
            companiesCache.UpdateList(currentListName, new[] { data });
            CompanyList.Items.Refresh();
            //CheckList.IsEnabled = true;    
            RefreshCheckButton();
            RefreshCheckBoxAutoUpdate();
        }

        private void DeleteCompany_Context(object s, RoutedEventArgs e)
        {
            CurrentList.Remove((CompanyData)CompanyList.SelectedItem);

            CompanyList.Items.Refresh();
        }
    }
}