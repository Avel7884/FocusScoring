using System;
using System.Collections.Generic;
using System.Linq;

namespace FocusScoringGUI
{
    public class ListFactory
    {
        private readonly ListsCache<CompanyData> dataCache;
        private readonly ListsCache<string> settingsCache;
        private readonly ListsCache<string> propertiesCache;

        public ListFactory(ListsCache<CompanyData> dataCache, 
            ListsCache<string> settingsCache,
            ListsCache<string> propertiesCache)
        {
            this.dataCache = dataCache;
            this.settingsCache = settingsCache;
            this.propertiesCache = propertiesCache;
        }

        public void DeleteList(string name)
        {
            dataCache.DeleteList(name);
            settingsCache.DeleteList(name);
            propertiesCache.DeleteList(name);
        } 

        public List<ListData> GetCachedLists()
        {
            var names = dataCache.GetNames();

            var inconsistency1 = names.ToHashSet();
            inconsistency1.ExceptWith(settingsCache.GetNames());
            var inconsistency2 = names.ToHashSet();
            inconsistency2.ExceptWith(propertiesCache.GetNames());
            
            if(inconsistency1.Count != 0 || inconsistency2.Count != 0)
                throw new ApplicationException(
                    "Cache inconsistency in lists: " + string.Join("    ", inconsistency1.Concat(inconsistency2)));

            return names.Select(n => new ListData(n, dataCache, settingsCache, propertiesCache)).ToList();
        }

        public ListData Create(string name)=>
            new ListData(name, dataCache, settingsCache, propertiesCache)
            {
                Data = new List<CompanyData>(), 
                Settings = new List<string> {"Имя", "Инн"}
            };
    }
}