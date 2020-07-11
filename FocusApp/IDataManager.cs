using System.Collections.Generic;
using System.Data;
using FocusApiAccess;

namespace FocusApp
{
    public interface IDataManager
    {
        void SaveCurrent();
        IReadOnlyList<DataInfo> Infos { get; }
        IFocusDataBase<INN> OpenNew(DataInfo info);

        IFocusDataBase<INN> CreateNew(DataInfo info, INN[] inns);
        void Delete(DataInfo dataInfo);
    }
}