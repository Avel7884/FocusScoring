using System;
using System.IO;
using FocusAccess.ResponseClasses;

namespace FocusAccess
{
    internal class JsonFileSystemCache : IJsonCache
    {
        private readonly bool forceless;
        private readonly string cacheFolder;

        public JsonFileSystemCache(bool forceless = true, string cacheFolder = null)
        {
            this.forceless = forceless;
            this.cacheFolder = cacheFolder ?? Settings.JSONCacheFolder;
            
            if (!Directory.Exists(this.cacheFolder))
                Directory.CreateDirectory(this.cacheFolder);
        }

        public bool TryGetJson<TQuery>(ApiMethodEnum method, TQuery args, out string json)
            where TQuery : IQueryComponents
        {
            json = default;
            if (!method.DiscCache())
                return false;
            var path = $"{Settings.CachePath}{cacheFolder}\\{args.MakeAlias()}.{method.Alias()}";
            if (!File.Exists(path) || forceless && DateTime.Now - File.GetLastWriteTime(path) > TimeSpan.FromDays(1))
                return false;
            json = File.ReadAllText(path);
            return true;
        }

        public void Update<TQuery>(ApiMethodEnum method, TQuery args, string json)
            where TQuery : IQueryComponents
        {
            if (!method.DiscCache()) return;
            var path = $"{Settings.CachePath}{cacheFolder}/{args.MakeAlias()}.{method.Alias()}";
            using var file = File.CreateText(path);
            file.Write(json);
        }

        public void Clear<TQuery>(ApiMethodEnum method, TQuery json)
            where TQuery : IQueryComponents
        {
            if (!method.DiscCache()) return;
            File.Delete($"{Settings.CachePath}{cacheFolder}/{json.MakeAlias()}.{method}");
        }
    }
}