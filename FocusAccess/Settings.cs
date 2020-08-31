using System;
using System.IO;

namespace FocusAccess
{
    public static class Settings
    {
        //public static bool OgrnEnabled { get; set; } = false;
        public static string CachePath { get; set; } = "./";
        public static string JSONCacheFolder { get; set; } = "JSONCache";
        public static string MarkersFolder { get; set; } = "Markers";//TODO remove dis
        public static string AppCacheFolder { get; set; } = "AppCache";
        public static string AppCachesIndexFileName { get; set; } = "AppCache.json";

        /*private static FocusKeyManager defaultManager;
        public static FocusKeyManager DefaultManager //TODO avoid singleton
        {
            get
            {
                if(defaultManager == null) //TODO find proper exception
                    throw new Exception("No FocusScoringManager was initialized");
                return defaultManager;
            }
            set
            {
                defaultManager = value;
            }
        }*/

        public static string ApiUrl { get; set; } = "https://focus-api.kontur.ru/api3";
    }
}