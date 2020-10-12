﻿using System;
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
using FocusAccess;
using FocusApp;

namespace FocusGUI
{
    /// <summary>
    /// Логика взаимодействия для ListDialog.xaml
    /// </summary>
    /// 
    public partial class ListDialog : Window
    {
        private readonly Func<DataInfo, string[],ListCreationResult> addList;

        private readonly ParameterRowData[] paramsData;

        //private readonly string[] parameters;
        //private ListsCache<CompanyData> CompanyCache;

        public ListDialog(Func<DataInfo,string[],ListCreationResult> addList)//,ListsCache<CompanyData> companyCache)
        {
            //CompanyCache = companyCache;
            InitializeComponent();
            //ParametersGrid.DataContext = Tuple.Create(SubjectParameter.Inn, true);
            this.addList = addList;
            var parameters = Enum.GetNames(typeof(SubjectParameter));
            var checkboxes = Enumerable.Repeat(false, parameters.Length).ToArray();
            checkboxes[0] = checkboxes[1] = true;
            paramsData = parameters.Zip(checkboxes, ParameterRowData.Create).ToArray();
            /*
            "Включен"
            ParametersGrid.Columns.Add("Параметер",typeof(string));*/
            ParametersGrid.Columns.Add(new DataGridTextColumn{Binding = new Binding("Parameter")});
            ParametersGrid.Columns.Add(new DataGridCheckBoxColumn{Binding = new Binding("Enabled")});

            ParametersGrid.ItemsSource = paramsData;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var inns = Inns.Text.Split("\n\r\t,.    ,".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            //var param = (ParametersGrid.Columns[0] as DataGridCheckBoxColumn).

            var selectedParams = paramsData
                .Zip(Enum.GetValues(typeof(SubjectParameter)).Cast<SubjectParameter>(),ValueTuple.Create)
                .Where(t=>t.Item1.Enabled)//&& !t.Item2.IsGenerated())
                .Select(t=>t.Item2)
                .ToList();
            
            var result = addList.Invoke(new DataInfo(ListName.Text,selectedParams), inns);
            if (result.Success)
                Close();
            if (result.HasErrors)
                MessageBox.Show(result.ErrorMessage);
        }
        
        private class ParameterRowData
        {
            public string Parameter { get; set; }
            public bool Enabled { get; set; }

            public static ParameterRowData Create(string parameter, bool enabled) =>
                new ParameterRowData(parameter, enabled);

            private ParameterRowData(string parameter, bool enabled)
            {
                Parameter = parameter;
                Enabled = enabled;
            }
        }
    }
}