using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FocusAccess;

namespace FocusApp
{
    internal class FocusDataBase<TSubject> : IFocusDataBase<TSubject>
    {
        private readonly IReadOnlyList<SubjectParameter> Parameter;
        private readonly IEntryFactory<TSubject> factory;
        public IDataBase<TSubject> Base { get; }
        
        public FocusDataBase(IDataBase<TSubject> db, IDataInfo info, IEntryFactory<TSubject> factory)
        {
            this.factory = factory;
            Parameter = info.Parameters;
            Info = info;
            Base = db;
            UpdateLength();
        }
        
        public FocusDataBase(IDataInfo info, IEntryFactory<TSubject> factory)
        {
            this.factory = factory;
            Parameter = info.Parameters;
            Info = info;
            Base = new DataBase<TSubject>();
            UpdateLength();
        }
        
        public IDataInfo Info { get; }
        public bool DataChanged { get; private set; }

        public int Count => Info.Length;

        public DataEntry<TSubject> this[int index] => Base[index]; 

        private void UpdateLength() => Info.Length = Base.Count();

        public void Write(params TSubject[] subjects)
        {
            Base.Write(subjects.Select(factory.CreateEntry).ToArray());
            UpdateLength();
            DataChanged = true;
        }

        public void AddColumns(SubjectParameter[] newParameters)
        {
            foreach (var entry in Base)
                factory.UpdateEntry(entry,newParameters);
            foreach (var parameter in newParameters) 
                Info.InsertColumn(Info.Length, parameter);
            DataChanged = true;
        }

        public void Delete(TSubject subject)
        {
            Base.Delete(subject);
            UpdateLength();
            DataChanged = true;
        }

        public void RemoveColumn(int column)
        {
            Info.RemoveColumn(column);
        }

        public void ReorderColumns(int targetIndex, int newIndex)
        {
            var param = Info.Parameters[targetIndex];
            Info.RemoveColumn(targetIndex);
            Info.InsertColumn(newIndex,param);
        }

        public IEnumerator<DataEntry<TSubject>> GetEnumerator() =>
            Base.Select(x=>new DataEntry<TSubject>(x,Reorder(x.Data,Info.MemoryOrderOfParameters))).GetEnumerator();

        private static T[] Reorder<T>(IReadOnlyList<T> source,IReadOnlyList<int> newIndexes)
        {
            if(newIndexes.Count != source.Count)
                throw new ArgumentException(); //TODO make exception
            var result = new T[source.Count];
            foreach (var (s, i) in source.Zip(newIndexes, ValueTuple.Create))
                result[i] = s;
            return result;
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}