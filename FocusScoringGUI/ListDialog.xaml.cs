using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FocusScoring;

namespace FocusScoringGUI
{
    /// <summary>
    /// Логика взаимодействия для ListDialog.xaml
    /// </summary>
    /// 
    public partial class ListDialog : Window
    {
        ListView ListView = null;
        ListView CompanyList = null;
        private List<MainWindow.CompanyData> CurrentList;
        public ListDialog(ref ListView listView, ref ListView companyList, ref List<MainWindow.CompanyData> currentList)
        {
            ListView = listView;
            CompanyList = companyList;
            CurrentList = currentList;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ListName.Text != "")
            {
            CurrentList = new List<MainWindow.CompanyData>();
           // Lists[ListName.Text] = CurrentList;
            ListView.Items.Refresh();
            CompanyList.ItemsSource = CurrentList;
            CompanyList.Items.Refresh();
            }
            MessageBox.Show("Название не может быть пустым");
        }
    }
}
