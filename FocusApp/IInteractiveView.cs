using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FocusApp
{
    public interface IInteractiveView : INotifyCollectionChanged, ICollection
    {
        void Selected(int index);
    }
}