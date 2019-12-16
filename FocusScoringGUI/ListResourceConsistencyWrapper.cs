using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FocusScoringGUI
{
    public class ListResourceConsistencyWrapper<T> : IList<T>
    {
        private readonly IList<T> baseList;
        private readonly Action<IList<T>> ensureConsistency;
        
        private bool ensuringOnHold;
        public bool EnsuringOnHold
        {
            get => ensuringOnHold;
            set
            {
                if(!value && value!=ensuringOnHold) ensureConsistency.Invoke(baseList);
                ensuringOnHold = value;
            }
        }

        public ListResourceConsistencyWrapper(IList<T> baseList, Action<IList<T>> ensureConsistency)
        {
            this.baseList = baseList;
            this.ensureConsistency = ensureConsistency;
        }

        public void EnsureConsistency()
        {
            if(EnsuringOnHold) return;
            ensureConsistency.Invoke(baseList);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return baseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            baseList.Add(item);
            EnsureConsistency();
        }

        public void Clear()
        { 
            baseList.Clear();
            EnsureConsistency();
        }

        public bool Contains(T item)
        {
            return baseList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            baseList.CopyTo(array,arrayIndex);
        }

        public bool Remove(T item)
        {
            var result = baseList.Remove(item);
            EnsureConsistency();
            return result;
        }

        public int Count => baseList.Count;
        public bool IsReadOnly => false; //TODO What?
        public int IndexOf(T item)
        {
            return baseList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            baseList.Insert(index, item);
            EnsureConsistency();
        }

        public void RemoveAt(int index)
        {
            baseList.RemoveAt(index);
            EnsureConsistency();
        }

        public T this[int index]
        {
            get => baseList[index];
            set
            {
                baseList[index] = value;
                EnsureConsistency();
            }
        }
    }
}