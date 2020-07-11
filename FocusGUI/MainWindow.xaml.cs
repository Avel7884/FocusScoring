using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FocusApiAccess;
using FocusApp;
using FocusScoring;

namespace FocusGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private FocusKey Key;
        private Api3 Api;
        private ListsView ListsView; //TODO swap names

        public MainWindow()
        {
            InitializeComponent();
            Key = new FocusKey("3208d29d15c507395db770d0e65f3711e40374df");
            Api = Key.StartApiAccess();
            var scorer = ScorerFactory.CreateDefaultINNScorer();
            var markers = new MarkersView(); 
            var subjects = new SubjectsView(Api,scorer,markers);
            ListsView = new ListsView(new DataManager(new EntryFactory(Api,scorer)),subjects);
            Lists.ItemsSource = ListsView;
            Companies.ItemsSource = subjects;
            Markers.ItemsSource = markers;
            /*
            ListsView.Updated += (o, a) => Lists.Items.Refresh();
            subjects.Updated += (o, a) => Companies.Items.Refresh();
            */
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog((n,l)=>
            {
                
                ListsView.Add((n,ParseInns(l).ToArray()));
                return null;
            }).Show();
        }

        private static IEnumerable<INN> ParseInns(IEnumerable<string> inns)
        {
            foreach (var innStr in inns.Where(inn => inn.Length == 10 || inn.Length == 12|| inn.Length == 13))
                if (INN.TryParse(innStr, out var inn))
                    yield return inn;
        }
    }    
}