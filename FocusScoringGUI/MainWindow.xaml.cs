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
        private bool isMonAvailable;
        public FocusKeyManager FocusManager { get; set; }
        private ListsCache<CompanyData> CompaniesCache { get; }
        
        
        /*public MarkersList Markers { get; set; }
        public CompanyList Companies { get; set; }*/

        public MainWindow(FocusKeyManager manager)
        {
            FocusManager = manager;   
            CompaniesCache = new ListsCache<CompanyData>("CompanyLists");
            
            InitializeComponent();
            CheckFocusKey(manager);

            isMonAvailable = FocusManager.GetAvailableMethods().Contains(ApiMethod.mon);

            CheckButton.Content = "Проверить список";//= isMonAvailable ? "Включить автопроверку." : "Проверить список";
            
            //MarkersControl.Manager = FocusManager;

            CompanyControl.CompanyFactory = FocusManager.CreateCompanyFactory();
            CompanyControl.Manager = FocusManager;//TODO Attepmpt to remove it
            CompanyControl.markersList = MarkersControl;
            CompanyControl.CompaniesCache = CompaniesCache;
            CompanyControl.FocusKeyUsed += (o, a) => CheckFocusKey(manager);
            
            CompanyListsControl.Manager = FocusManager;
            CompanyListsControl.CompanyList = CompanyControl;
            CompanyListsControl.CompaniesCache = CompaniesCache;
            //CompanyListsControl.FocusKeyUsed += (o, a) => CheckFocusKey(manager);
        }

        private void CheckFocusKey(FocusKeyManager manager)
        {
            KeyCounter.Text = "Ключ: использовано " + manager.Usages;
            var worker = new BackgroundWorker();
            worker.DoWork += (o,e)=>Thread.Sleep(10000);
            worker.RunWorkerCompleted += (o, e) =>
                KeyCounter.Text = "Ключ: использовано " + manager.Usages;
            worker.RunWorkerAsync(20000);
        }
        
        private void FocusWindowShow(object sender, RoutedEventArgs e)
        {
            FocusKeyWindow fkw;
            using (key = Registry.CurrentUser.OpenSubKey(@"Software\FocusScoring"))
                fkw = new FocusKeyWindow(
                    Coder.Decode(key.GetValue("fkey").ToString()),
                    FocusManager.IsBaseMode());
            fkw.Owner = this;
            fkw.Show();

            fkw.KeyAccepted += (o, a) =>
            {
                FocusManager = fkw.Manager; 
                
                //MarkersControl.Manager = FocusManager;
                CompanyControl.CompanyFactory = FocusManager.CreateCompanyFactory();
                CompanyControl.Manager = FocusManager;
                CompanyListsControl.Manager = FocusManager;
                
                CheckFocusKey(FocusManager);
            };
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            var a = new MarkerListWindow(FocusManager);
            a.Owner = this;
            a.Show();
        }

        private void ListCheck_OnClick(object sender, RoutedEventArgs e)
        {
            if (isMonAvailable)
            {
                CompanyControl.CheckCurrentList();
            }
            else CompanyControl.CheckCurrentList();
        }
    }
}
