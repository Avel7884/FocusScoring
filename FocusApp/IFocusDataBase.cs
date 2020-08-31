using System.Collections.Generic;

namespace FocusApp
{
    public interface IFocusDataBase<TSubject>: IReadOnlyList<DataEntry<TSubject>>
    {
        IDataInfo Info { get; }
        IDataBase<TSubject> Base {get;}
        bool DataChanged { get; }
        void Write(params TSubject[] subjects);
        void AddColumns(SubjectParameter[] newParameters);
        void Delete(TSubject subject);
        void RemoveColumn(int column);
        void ReorderColumns(int targetIndex, int newIndex);
    }
}