using System;
using System.IO;
using FocusApiAccess.Methods;
using FocusApiAccess.ResponseClasses;

namespace FocusApiAccess
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

        public bool TryGetJson<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, out string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            json = default;
            if (!method.DiscCache)
                return false;
            var path = $"{Settings.CachePath}{cacheFolder}\\{args.MakeAlias()}.{method.MakeAlias()}";
            if (!File.Exists(path) || forceless && DateTime.Now - File.GetLastWriteTime(path) > TimeSpan.FromDays(1))
                return false;
            json = File.ReadAllText(path);
            return true;
        }

        public void Update<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery args, string json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            if (!method.DiscCache) return;
            var path = $"{Settings.CachePath}{cacheFolder}/{args.MakeAlias()}.{method.MakeAlias()}";
            using var file = File.CreateText(path);
            file.Write(json);
        }

        public void Clear<TData, TQuery>(ApiMethod<TData, TQuery> method, TQuery json)
            where TData : IParameterValue where TQuery : IQueryComponents
        {
            if (!method.DiscCache) return;
            File.Delete($"{Settings.CachePath}{cacheFolder}/{json.MakeAlias()}.{method}");
        }
    }
}