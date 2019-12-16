
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;

namespace FocusScoringGUI
{
    public class ListData
    {
        private readonly ListsCache<CompanyData> dataCache;
        private readonly ListsCache<string> settingsCache;
        private readonly ListsCache<string> propertiesCache;
        public event Action<object, EventArgs> PropertyChanged;

        private ListData(){}

        public ListData(string name,ListsCache<CompanyData> dataCache, ListsCache<string> settingsCache, ListsCache<string> propertiesCache)
        {
            Name = name;
            this.dataCache = dataCache;
            this.settingsCache = settingsCache;
            this.propertiesCache = propertiesCache;
        }

        private bool holdCacheUpdates;

        public bool HoldCacheUpdates
        {
            get => holdCacheUpdates;
            set
            {
                holdCacheUpdates = value;
                data.EnsuringOnHold = holdCacheUpdates;
                settings.EnsuringOnHold = holdCacheUpdates;
            }
        }

        
        public void Rename(string newName)
        {
            dataCache.Rename(Name,newName);
            settingsCache.Rename(Name,newName);
            propertiesCache.Rename(Name,newName);
            Name = newName;
        } 

        public string Name { get; private set; }

        private int count = -1;
        public int Count
        {
            get
            {
                if(count<0)
                    return count = int.Parse(propertiesCache.GetList(Name).First());
                return count;
            }
            private set => count = value;
        }

        private ListResourceConsistencyWrapper<string> settings;
        public IList<string> Settings
        {
            get
            {
                if(settings == null)
                    InitSettingsList(settingsCache.GetList(Name));
                return settings;
            }
            set
            {
                InitSettingsList(value);
                settingsCache.UpdateList(Name, value);
            }
        }

        private void InitSettingsList(IList<string> list) =>
            settings = new ListResourceConsistencyWrapper<string>(list,
                l => settingsCache.UpdateList(Name, l));

        private ListResourceConsistencyWrapper<CompanyData> data;

        public IList<CompanyData> Data
        {
            get
            {
                if(data == null)
                    InitDataList(dataCache.GetList(Name));
                return data;
            }
            set
            {
                InitDataList(value);
                Count = value.Count;
                propertiesCache.UpdateList(Name,new List<string>{Count.ToString()});
                dataCache.UpdateList(Name, value);
            }
        }
        
        private void InitDataList(IList<CompanyData> list) =>
            data = new ListResourceConsistencyWrapper<CompanyData>(list,
                l => dataCache.UpdateList(Name, l));
    }
}