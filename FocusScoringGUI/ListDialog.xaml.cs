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
        private readonly Action<string, List<MainWindow.CompanyData>> addList;
        ListView ListView = null;
        ListView CompanyList = null;
        private List<MainWindow.CompanyData> CurrentList;
        private CompanyListsCache CompanyCache;

        public ListDialog(Action<string,List<MainWindow.CompanyData>> addList,CompanyListsCache companyCache)
        {
            this.CompanyCache = companyCache;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            this.addList = addList;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyCache.GetNames().Contains(ListName.Text))
            {
                MessageBox.Show("Лист с данным названием уже существует");
                return;
            }
            if (ListName.Text != "")
            {
                addList.Invoke(ListName.Text,new List<MainWindow.CompanyData>());
                this.Close();
            //CurrentList = new List<MainWindow.CompanyData>();
            //ListView.Items.Refresh();
            //CompanyList.ItemsSource = CurrentList;
            //CompanyList.Items.Refresh();
            }
            else
            MessageBox.Show("Название не может быть пустым");
        }
    }
}
