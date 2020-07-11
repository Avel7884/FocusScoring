using System;
using System.Collections;
using System.Collections.Specialized;
using FocusApiAccess;

namespace FocusApp
{
    public class ListsView : IDynamicInteractiveView<(DataInfo,INN[]),DataInfo>
    {
        private readonly IDataManager manager;
        private readonly IProvidableInteractiveView<IFocusDataBase<INN>> subjectsView;

        public ListsView(IDataManager manager, IProvidableInteractiveView<IFocusDataBase<INN>> subjectsView)
        {
            this.manager = manager;
            this.subjectsView = subjectsView;
        }
        
        public IEnumerator GetEnumerator() => 
            manager.Infos.GetEnumerator();

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void Selected(int index)
        {
            var dataInfo = manager.Infos[index];
            subjectsView.Provide(manager.OpenNew(dataInfo));
        }

        public void Add((DataInfo,INN[]) obj)
        {
            var (info, inns) = obj;
            var db = manager.CreateNew(info,inns);
            subjectsView.Provide(db);

            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,info,0));
        }

        public void Delete(DataInfo obj)
        {
            manager.Delete(obj);
        }

    }
}