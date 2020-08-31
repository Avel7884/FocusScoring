using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FocusAccess;

namespace FocusScoringGUI
{
    public partial class CompanySettings
    {
        private readonly ListData list;
        public bool OkClicked { get; private set; }

        public CompanySettings(ListData list, IEnumerable<string> allowedParameters)
        {
            this.list = list;
            InitializeComponent();
            
            ListView.ItemsSource = allowedParameters
                .Select(x => new CompanySetting
                    {Check = list.Settings.Contains(x), Name = x})
                .ToArray();
        }

        private void Ok_Click(object o, RoutedEventArgs e)
        {
            list.Settings =
                ListView.ItemsSource
                    .Cast<CompanySetting>()
                    .Where(x => x.Check)
                    .Select(x => x.Name)
                    .ToList();
            OkClicked = true;
            Close();
        }
    }
}
