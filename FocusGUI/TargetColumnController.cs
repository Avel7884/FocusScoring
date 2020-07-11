using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using FocusApp;

namespace FocusGUI
{
    public class TargetColumnController<TTarget>
    {
        private readonly DataGrid grid;
        private readonly ListsView lists;

        public TargetColumnController(DataGrid grid, IDynamicInteractiveView<(DataInfo,TTarget[]),DataInfo> lists, )
        {
            this.grid = grid;
            this.lists = lists;
            
            InitializeColumns();
            grid.ColumnReordered += ReorderedHandler;
        }

        private void ReorderedHandler(object sender, DataGridColumnEventArgs e)
        {
            e.Column.DisplayIndex
        }

        private void InitializeColumns()
        {
            for (var i = 0; i < (lists as ICollection).Count; i++)
            {
                grid.Columns.Add(new GridViewColumn
                {
                    Header = CurrentList.Settings[i],
                    DisplayMemberBinding = new Binding($"i")
                });
            }

            ;
        } 
    }
}