using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using FocusScoring;
using Microsoft.Win32;

namespace FocusScoringGUI
{ //TODO Refactor
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistryKey key;
        public FocusScoringManager FocusManager { get; set; }
        public ListsCache<string> CompaniesCache { get; }
        /*public MarkersList Markers { get; set; }
        public CompanyList Companies { get; set; }*/

        public MainWindow(FocusScoringManager manager)
        {
            FocusManager = manager;   
            CompaniesCache = new ListsCache<string>("CompanyLists");
            
            InitializeComponent();
            
            MarkersControl.Manager = FocusManager;

            CompanyControl.Manager = FocusManager;
            CompanyControl.markersList = MarkersControl;
            CompanyControl.CompaniesCache = CompaniesCache;
            
            CompanyListsControl.Manager = FocusManager;
            CompanyListsControl.CompanyList = CompanyControl;
            CompanyListsControl.CompaniesCache = CompaniesCache;
        }
        
        private void FocusWindowShow(object sender, RoutedEventArgs e)
        {
            FocusKeyWindow fkw;
            using (key = Registry.CurrentUser.OpenSubKey(@"Software\FocusScoring"))
                fkw = new FocusKeyWindow(Coder.Decode(key.GetValue("fkey").ToString()));
            fkw.Owner = this;
            fkw.Show();

            fkw.KeyAccepted += (o, a) =>
                this.FocusManager = fkw.Manager;
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(FocusManager);
            a.Owner = this;
            a.Show();
        }
    }
}
