using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FocusAccess;
using Newtonsoft.Json;

namespace FocusApp
{
    [JsonArray]
    public class DataBase<TSubject> : IDataBase<TSubject>
    {
        private readonly List<DataEntry<TSubject>> data;

        public IEnumerable<DataEntry<TSubject>> Data => data;


        public DataBase()
        {
            data = new List<DataEntry<TSubject>>();
        }

        public DataBase(DataEntry<TSubject>[] entries)
        {                                             
            data = new List<DataEntry<TSubject>>();
            Write(entries);
        }

        
        public void Write(params DataEntry<TSubject>[] writeData)
        {
            if(data.Count > 0 && writeData.Any(x=>x.Data.Count != data[0].Data.Count))
                throw new ArgumentException("Length of entries should match.");
            data.AddRange(writeData);
        }
        
        public void AddColumns(string[][] columns)
        {
            if(columns.Any(x=>x.Length != Length))
                throw new ArgumentException("Length of entries should match.");
            for (int i = 0; i < Length; i++)
                foreach (var column in columns)
                    data[i].Insert(data[0].Data.Count,column[i]);
        }

        public void Delete(TSubject subject)
        {
            data.RemoveAll(x=>x.Subject.Equals(subject));//TODO possible error
        }

        public void RemoveColumn(int column)
        {
            foreach (var entry in data) 
                entry.RemoveAt(column);
        }
        public int Length => data.Count;
        public IEnumerator<DataEntry<TSubject>> GetEnumerator() =>
            data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int Count => data.Count;

        public DataEntry<TSubject> this[int index] => data[index];
    }
}