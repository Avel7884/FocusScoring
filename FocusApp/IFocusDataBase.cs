using System.Collections.Generic;

namespace FocusApp
{
    public interface IFocusDataBase<TSubject>: IReadOnlyList<DataEntry<TSubject>>
    {
        DataInfo Info { get; }
        IDataBase<TSubject> Base {get;}
        void Write(params TSubject[] subjects);
        void AddColumns(SubjectParameter[] newParameters);
        void Delete(TSubject subject);
        void RemoveColumn(int column);
    }
}