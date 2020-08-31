using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FocusAccess;
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
            SaveInfo();
        }

        private void SaveInfo()
        {
            var path = Settings.CachePath + Settings.AppCachesIndexFileName;
            File.WriteAllText(path,JsonConvert.SerializeObject(infos));
        }

        public IFocusDataBase<INN> OpenNew(DataInfo info)
        {
            if(currentBase != null && currentBase.DataChanged) SaveCurrent();
            factory.Parameters = info.Parameters;
            var path = Settings.CachePath + Settings.AppCacheFolder + "\\" + info.Name;
            var unnamedBase = new DataBase<INN>(JsonConvert.DeserializeObject<DataEntry<INN>[]>(File.ReadAllText(path)));
            return currentBase = new FocusDataBase<INN>(unnamedBase, info, factory);            
        }    

        public IFocusDataBase<INN> CreateNew(DataInfo info, INN[] inns)
        {
            if(currentBase != null && currentBase.DataChanged) SaveCurrent();
            factory.Parameters = info.Parameters;
            if(Infos.Any(i=>i.Name == info.Name))
                throw new ArgumentException();//TODO make exception
            infos.Add(info);
            
            //var dataBase = new DataBase<INN>(inns.Select(factory.CreateEntry).ToArray()); TODO bury the dead
            currentBase = new FocusDataBase<INN>(info, factory);
            currentBase.Write(inns);
            SaveCurrent();
            return currentBase;
        }

        public void Delete(DataInfo info)
        {
            var path = Settings.CachePath + Settings.AppCacheFolder + "\\" + info.Name;
            File.Delete(path);
            infos.Remove(info);
            SaveInfo();
        }
    }
}