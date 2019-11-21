using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FocusScoring;

namespace FocusScoringGUI
{
    public partial class CompanySettings
    {
        private readonly ListsCache<string> settings;
        private readonly string listName;
        public bool OkClicked { get; private set; }

        public CompanySettings(ListsCache<string> settings, string listName, IEnumerable<string> allowedParameters)
        {
            this.settings = settings;
            this.listName = listName;
            IEnumerable<string> selected;
            if (!settings.GetNames().Contains(listName))
            {
                selected = new[] {"Short","FIO"};
                settings.UpdateList(listName,selected);                
            }
            else
                selected = settings.GetList(listName);

            InitializeComponent();
            
            ListView.ItemsSource = allowedParameters
                .Select(x => new CompanySetting
                    {Check = selected.Contains(x), Name = x})
                .ToArray();
        }

        private void Ok_Click(object o, RoutedEventArgs e)
        {
            settings.UpdateList(listName,
                ListView.ItemsSource
                    .Cast<CompanySetting>()
                    .Where(x => x.Check)
                    .Select(x => x.Name));
            OkClicked = true;
            Close();
        }
    }
}
