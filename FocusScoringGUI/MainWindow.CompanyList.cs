using System;
using System.Linq;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class MainWindow
    {
        private void CompanySelected_Click(object s, RoutedEventArgs e)
        {
            if(CompanyList.SelectedItem == null)
                return;
            var companyData = ((CompanyData) CompanyList.SelectedItem);
            TextBlockName.Text = companyData.Name;
            if (companyData.IsChecked)
            {
                companyData.Check(manager);
                MarkersList.ItemsSource = companyData.Company.Markers.Select(MarkerSubData.Create);
            }
            CompanyList.Items.Refresh();
            MarkersList.Items.Refresh();
        }

        //TODO make remove button
        
        
        
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
            foreach (var data in CurrentList)
                data.Check(manager);
            KeyCounter.Text = "Ключ: " + manager.Usages;
            companiesCache.UpdateList(currentListName,CurrentList);
            CompanyList.Items.Refresh();
        }

        private void ButtonDataUpdate_Click(object s, RoutedEventArgs e)
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
        }     
        
        private void DeleteCompany_Click(object s, RoutedEventArgs e)
        {
            CurrentList.Remove((CompanyData) CompanyList.SelectedItem);
            CompanyList.Items.Refresh();            
        }
    }
}