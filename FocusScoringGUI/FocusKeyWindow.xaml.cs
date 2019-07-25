using FocusScoring;
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
    /// Логика взаимодействия для FocusKeyWindow.xaml
    /// </summary>
    public partial class FocusKeyWindow : Window
    {
        public FocusKeyWindow()
        {
            InitializeComponent();
        }
        public FocusKeyWindow(string key)
        {
            InitializeComponent();
            KeyBox.Password = key;
            TextBlock.Text = "..." + key.Substring(key.Length - 5, 5);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (KeyBox.Password.Count() < 40)
                MessageBox.Show("Введен некорректный ключ. Введите ключ длинной 40 знаков", "Конутр.Фокус");
            else
            {
                Coder.Encode(KeyBox.Password);
                if (this.Owner == null)
                    new MainWindow().Show();
                this.Close();
            }
        }
    }
}
