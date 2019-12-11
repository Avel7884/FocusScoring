using System.Windows;

namespace FocusScoringGUI
{
    public partial class ListRenameWindow : Window
    {
        public ListRenameWindow()
        {
            InitializeComponent();
        }

        private void CanselButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        public string NewName { get; private set; }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text == "")
            {
                MessageBox.Show("Название не может быть пустым.");
                return;
            }
            NewName = NameBox.Text;
            Close();
        }
    }
}