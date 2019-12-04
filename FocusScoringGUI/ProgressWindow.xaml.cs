using System.Windows;

namespace FocusScoringGUI
{
    public partial class ProgressWindow : Window
    {
        public ProgressWindow(string windowName)
        {
            InitializeComponent();
            Name.Text = windowName;
        }
    }
}