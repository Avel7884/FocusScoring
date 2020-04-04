using System;
using System.Collections.Generic;
using System.IO;
using FocusApiAccess;

namespace FocusScoring
{
    public static class FIOCache
    {

        public static bool HasChanged(string inn, string FIO)
        {
            var path = Settings.CachePath + "FIODict";
            if (File.Exists(path))
            {
                using (var file = File.Open(path, FileMode.OpenOrCreate))
                {
                    var dict = FIODictionarySerializer.Deserialize(file);

                    if (dict.TryGetValue(inn, out var tup))
                        return FIO != tup.Item1;

                    dict[inn] = (FIO, DateTime.Now);

                    file.Position = 0;
                    FIODictionarySerializer.Serialize(dict, file);

                    return false;
                }
            }
            else
            {
                using (var file = File.Open(path, FileMode.OpenOrCreate))
                {
                    var dict = new Dictionary<string, (string, DateTime)>();

                    dict[inn] = (FIO, DateTime.Now);

                    file.Position = 0;
                    FIODictionarySerializer.Serialize(dict, file);

                    return false;
                }
            }
        }
    }
}