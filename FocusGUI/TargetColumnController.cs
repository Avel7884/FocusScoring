using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using FocusApp;

namespace FocusGUI
{
    public class TargetColumnController<TTarget>
    {
        private readonly DataGrid grid;
        private IFocusDataBase<TTarget> dataBase;
        private int reorderingColumnIndex;

        public TargetColumnController(DataGrid grid)
        {
            this.grid = grid;
            grid.ColumnDisplayIndexChanged += ReorderedHandler;
            grid.ColumnReordering += ReorderingHandler;
        }

        private void ReorderingHandler(object sender, DataGridColumnReorderingEventArgs e)
        {
            reorderingColumnIndex = e.Column.DisplayIndex;
        }

        private void ReorderedHandler(object sender,  DataGridColumnEventArgs e)
        {
            //return;
            //TODO continue here with mass 
            
            if(dataBase == null || e.Column.DisplayIndex == reorderingColumnIndex) return;
            dataBase.ReorderColumns(reorderingColumnIndex,e.Column.DisplayIndex);
        }

        public void SetNewData(IFocusDataBase<TTarget> dataBase)
        {
            this.dataBase = dataBase;
            grid.ItemsSource = dataBase;
            InitializeColumns();
            grid.Items.Refresh();
        }

        private void InitializeColumns()
        {
            for (var i = grid.Columns.Count - 1; i >= 0; i--)
                grid.Columns.RemoveAt(i);
            for (var i = 0; i < dataBase.Info.Parameters.Count; i++)
                if (dataBase.Info.Parameters[i] == SubjectParameter.Shield)
                {
                    var image = new FrameworkElementFactory(typeof(Image));
                    image.SetBinding(
                        Image.SourceProperty,
                        new Binding {Converter = new EntryToUrlConverter()});
                    grid.Columns.Add(new DataGridTemplateColumn
                    {
                        CellTemplate = new DataTemplate
                        {
                            DataType = typeof(DataEntry<TTarget>),
                            VisualTree = image
                        }
                    });
                }
                else
                    grid.Columns.Add(new DataGridTextColumn
                    {
                        Header = dataBase.Info.Parameters[i].ToString(),
                        Binding = new Binding($"Data[{i}]")
                    });
        }     
    }
}