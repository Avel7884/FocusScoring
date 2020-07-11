using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FocusApiAccess;
using Newtonsoft;
using Newtonsoft.Json;

namespace FocusApp
{
    public class DataManager : IDataManager
    {
        private IFocusDataBase<INN> currentBase;
        private IList<DataInfo> infos;
        private readonly ISettableEntryFactory<INN> factory;

        public IReadOnlyList<DataInfo> Infos {
            get
            {
                if (infos != null) return infos.ToArray();
                var path = Settings.CachePath + Settings.AppCachesIndexFileName;
                if (!File.Exists(path))
                {
                    infos = new DataInfo[0];
                    File.WriteAllText(path,JsonConvert.SerializeObject(infos));
                }
                    
                var json = File.ReadAllText(path);
                return (infos = JsonConvert.DeserializeObject<DataInfo[]>(json).ToList()).ToArray();
            }
        }

        public DataManager(ISettableEntryFactory<INN> factory)
        {
            this.factory = factory;
        }
        
        public void SaveCurrent()
        {
            if(currentBase == null) throw new Exception();//TODO make exception
            if(infos.Count == 0) return;
            var path = Settings.CachePath + Settings.AppCacheFolder + "\\" + currentBase.Info.Name;
            File.WriteAllText(path,JsonConvert.SerializeObject(currentBase.Base));
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            var path = Settings.CachePath + Settings.AppCachesIndexFileName;
            File.WriteAllText(path,JsonConvert.SerializeObject(infos));
        }

        public IFocusDataBase<INN> OpenNew(DataInfo info)
        {
            if(currentBase != null) SaveCurrent();
            factory.Parameters = info.Parameters;
            var path = Settings.CachePath + Settings.AppCacheFolder + "\\" + info.Name;
            var unnamedBase = JsonConvert.DeserializeObject<DataBase<INN>>(File.ReadAllText(path));
            return currentBase = new FocusDataBase<INN>(unnamedBase, info, factory);            
        }

        public IFocusDataBase<INN> CreateNew(DataInfo info, INN[] inns)
        {
            if(currentBase != null) SaveCurrent();
            factory.Parameters = info.Parameters;
            if(infos.Any(i=>i.Name == info.Name))
                throw new ArgumentException();//TODO make exception
            //info.Length = inns.Length;
            infos.Add(info);
            return currentBase = new FocusDataBase<INN>(info, inns, factory);
        }

        public void Delete(DataInfo info)
        {
            throw new NotImplementedException();
        }
    }
}