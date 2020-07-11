using System.Collections.Generic;

namespace FocusApp
{
    public interface IDataBase<TSubject>
    {
        IEnumerable<DataEntry<TSubject>> Data { get; }
        void Write(params DataEntry<TSubject>[] writeData);
        void AddColumns(string[][] columns);
        void Delete(TSubject subject);
        void RemoveColumn(int column);
    }
}