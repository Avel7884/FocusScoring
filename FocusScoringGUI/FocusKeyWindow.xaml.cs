using FocusScoring;
using Microsoft.Win32;
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
        private readonly bool mode;
        public FocusKeyManager Manager { get; set; }
        private RegistryKey key;

        public FocusKeyWindow()
        {
            InitializeComponent();
            using (key = Registry.CurrentUser.CreateSubKey(@"Software\FocusScoring"))
            {
                if (key != null)
                {
                    if (key.GetValue("fkey") != null)
                    {
                        this.Hide();
                        Manager = FocusKeyManager.StartAccess(Coder.Decode(key.GetValue("fkey").ToString()));
                        KeyAccepted?.Invoke(this,null);
                        var mW = new MainWindow(Manager);
                        mW.Owner = null;
                        mW.Show();
                        this.Close();
                    }
                }
            }
        }
        
        public FocusKeyWindow(string key, bool mode)
        {
            this.mode = mode;
            InitializeComponent();
            KeyBox.Password = key;
            TextBlock.Text = "..." + key.Substring(key.Length - 5, 5);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event Action<object, EventArgs> KeyAccepted;

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (KeyBox.Password.Count() < 40)
                MessageBox.Show("Введен некорректный ключ. Введите ключ длинной 40 знаков", "Конутр.Фокус");
            else
            {
                Manager = FocusKeyManager.StartAccess(KeyBox.Password);
                if (Manager.Usages.StartsWith("Ошибка"))//TODO shitty check
                {
                    MessageBox.Show("Проверьте правильность ключа");
                    return;
                }
                //if (Manager.IsBaseMode() != mode) TODO fix!
                    MessageBox.Show("Новый ключ имеет отличный набор методов. Рекомендуется перезапуск приложения.");

                KeyAccepted?.Invoke(this,null);
                Coder.Encode(KeyBox.Password);
                if (this.Owner == null)
                    new MainWindow(Manager).Show();
                this.Close();
            }
        }

        private void CheckKey_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
