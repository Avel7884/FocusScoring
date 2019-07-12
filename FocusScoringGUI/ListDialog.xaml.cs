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

namespace FocusScoringGUI
{
    /// <summary>
    /// Логика взаимодействия для ListDialog.xaml
    /// </summary>
    public partial class ListDialog : Window
    { 
        public ListDialog()
        {
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
         //   List
        }
    }
}
