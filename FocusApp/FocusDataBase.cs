using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FocusApiAccess;

namespace FocusApp
{
    class FocusDataBase<TSubject> : IFocusDataBase<TSubject>
    {
        private readonly IList<SubjectParameter> Parameter;
        private readonly IEntryFactory<TSubject> factory;
        public IDataBase<TSubject> Base { get; }
        
        public FocusDataBase(IDataBase<TSubject> db, DataInfo info, IEntryFactory<TSubject> factory)
        {
            Base = db;
            this.factory = factory;
            Parameter = info.Parameters;
            UpdateLength();
        }

        public FocusDataBase(DataInfo info, TSubject[] inns, IEntryFactory<TSubject> factory)
        {
            this.factory = factory;
            Parameter = info.Parameters;
            Info = info;
            Base = new DataBase<TSubject>(inns.Select(factory.CreateEntry).ToArray());
            UpdateLength();
        }
        
        public DataInfo Info { get; }
        

        public int Count => Info.Length;

        public DataEntry<TSubject> this[int index] => Base.Data.ToList()[index]; //TODO remove interface or something

        private void UpdateLength() => Info.Length = Base.Data.Count();

        public void Write(params TSubject[] subjects)
        {
            Base.Write(subjects.Select(factory.CreateEntry).ToArray());
            UpdateLength();
        }

        public void AddColumns(SubjectParameter[] newParameters)
        {
            foreach (var entry in Base.Data)
                factory.UpdateEntry(entry,newParameters);
        }

        public void Delete(TSubject subject)
        {
            throw new NotImplementedException();
        }

        public void RemoveColumn(int column)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<DataEntry<TSubject>> GetEnumerator() =>
            Base.Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}