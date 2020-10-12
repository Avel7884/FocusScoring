﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using FocusAccess;
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
        private IApi3 Api;
        private readonly IDataManager dataManager;
        private readonly TargetColumnController<INN> targetColumnController;
        
        private IFocusDataBase<INN> currentDataBase;

        private IFocusDataBase<INN> CurrentDataBase
        {
            get => currentDataBase;
            set => currentDataBase = value;
        }

        public MainWindow()
        {
            InitializeComponent();
            Key = new FocusKey("3208d29d15c507395db770d0e65f3711e40374df");
            Api = new Api(Key);
            var scorer = ScorerFactory.CreateLibraryINNScorer(Api);
            /*var markers = new MarkersView(); 
            var subjects = new SubjectsView(Api,scorer,markers);
            ListsView = new ListsView(new DataManager(new EntryFactory(Api,scorer)),subjects);*/

            dataManager = new DataManager(new EntryFactory(Api, scorer));
            targetColumnController = new TargetColumnController<INN>(Companies);
            UpdateKeyCounter();
            
            Lists.ItemsSource = dataManager.Infos;
            Lists.MouseDoubleClick += (o, a) =>
            {
                if(Lists.SelectedItem == null) return;
                CurrentDataBase = dataManager.OpenNew(Lists.SelectedItem as DataInfo);
                targetColumnController.SetNewData(CurrentDataBase);
            };

            if (dataManager.Infos.Count > 0)
            {
                CurrentDataBase = dataManager.OpenNew(dataManager.Infos[0]);
                targetColumnController.SetNewData(CurrentDataBase);
            }
            
            Companies.MouseDoubleClick += (o, a) =>
            {
                if (!Companies.SelectedCells.Any())
                    return;
                Markers.ItemsSource = (Companies.SelectedCells.First().Item as DataEntry<INN>)?.Markers 
                                      ?? throw new ApplicationException("Selection error.");
                Markers.Items.Refresh();
            };

            Closed += (s, a) =>
            {
                if (currentDataBase != null)
                    dataManager.SaveCurrent();
            };
        }

        private void UpdateKeyCounter()
        {
            Key.CheckUsages();
            KeyCounter.Text = $"Ключ: использовано {Key.Nominator} из {Key.Denominator}";
        }

        private void AddList_Click(object sender, RoutedEventArgs e)
        {
            new ListDialog((info, list) =>
            {
                var result = GetListResult(info, list);
                if (!result.Success) return result;
                SetList(info, list);
                return result; //TODO remove
                try
                {
                    SetList(info, list);
                    return result;
                }
                catch (Exception ex)
                {
                    return ListCreationResult.FailWithError(ex.Message);
                }
                finally
                {
                    UpdateKeyCounter();
                }
            }).Show();
        }

        private ListCreationResult GetListResult(DataInfo info,string[] list)
        {
            if (info.Name == "")
                return ListCreationResult.FromError("Название не может быть пустым");
            
            if (dataManager.Infos.Any(x=>x.Name == info.Name))
                return ListCreationResult.FromError("Лист с данным названием уже существует");
            
            /*if(!Key.AbleToUseMore(1)) TODO return
                return ListCreationResult.FailWithError("Ключ требует продления!");

            var usagesNeeded = Key.QueryUsage(Query.Unify(list.Select(i => (InnUrlArg) (INN) i)));
            if (usagesNeeded == 0)
                return ListCreationResult.Succsess();
            if (!Key.AbleToUseMore(usagesNeeded))
                return ListCreationResult.FailWithError("Непроверенных компаний больше чем осталось использовний ключа. Уменьшите список или продлите ключ.");
            
            */
            var usagesNeeded = 10;
            var mb = MessageBox.Show($"Ключ будет использован {usagesNeeded} раз.",
                "Внимание", MessageBoxButton.YesNo);
            return mb == MessageBoxResult.Yes ? 
                ListCreationResult.Succsess() : 
                ListCreationResult.Pending();
        }

        private void SetList(DataInfo info, string[] list)
        {
            CurrentDataBase = dataManager.CreateNew(info,ParseInns(list).ToArray());
            targetColumnController.SetNewData(CurrentDataBase);
            Lists.ItemsSource = dataManager.Infos;
            Lists.Items.Refresh();
        }

        private static IEnumerable<INN> ParseInns(IEnumerable<string> inns)
        {
            foreach (var innStr in inns.Where(inn => inn.Length == 10 || inn.Length == 12|| inn.Length == 13))
                if (INN.TryParse(innStr, out var inn))
                    yield return inn;
        }

        private void DeleteList_Click(object sender, RoutedEventArgs e) => 
            dataManager.Delete(Lists.SelectedItem as DataInfo);

        private void AddTarget_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataBase == null)
            {
                MessageBox.Show("Сначала выберите список компаний");
                return;
            }
            CurrentDataBase.Write((INN)"702100195003");
        }

        private void Rename_Click(object sender, RoutedEventArgs e) =>
            currentDataBase.Info.Name  = "New_" + currentDataBase.Info.Name;
        
        private void DeleteTarget_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDataBase == null)
            {
                MessageBox.Show("Сначала выберите список компаний");
                return;
            }
            if(currentDataBase.Count == 0)
            {
                MessageBox.Show("Список уже пуст");
                return;
            }
            CurrentDataBase.Delete((Companies.SelectedItem as DataEntry<INN>)?.Subject ?? throw new Exception("Wut?!"));
        }

        private void FocusWindowShow(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AllMarkers_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }    
}