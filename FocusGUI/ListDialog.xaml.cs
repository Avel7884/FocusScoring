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
using FocusApiAccess;
using FocusApp;

namespace FocusGUI
{
    /// <summary>
    /// Логика взаимодействия для ListDialog.xaml
    /// </summary>
    /// 
    public partial class ListDialog : Window
    {
        private readonly Func<DataInfo, string[],string> addList;
        //private ListsCache<CompanyData> CompanyCache;

        public ListDialog(Func<DataInfo,string[],string> addList)//,ListsCache<CompanyData> companyCache)
        {
            //CompanyCache = companyCache;
            InitializeComponent();
            this.addList = addList;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var inns = Inns.Text.Split(@"
 ,.    ,".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            var error = addList.Invoke(new DataInfo(ListName.Text), inns);
            if (error != null)
            {
                if(error != "") 
                    MessageBox.Show(error);
            }
            else Close();
        }
    }
}
