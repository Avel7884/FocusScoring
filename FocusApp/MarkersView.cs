/*using System;
using System.Collections;
using System.Collections.Specialized;
using FocusApiAccess;
using FocusScoring;

namespace FocusApp
{
    public class MarkersView : IProvidableInteractiveView<MarkerResult<INN>[]>
    {
        private MarkerResult<INN>[] markers = new MarkerResult<INN>[0];

        public IEnumerator GetEnumerator()
        {
            return markers.GetEnumerator();
        }

        public void Selected(int index)
        {
            throw new NotImplementedException();
        }

        public void Provide(MarkerResult<INN>[] data)
        {
            markers = data;
            CollectionChanged?.Invoke(this,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}*/