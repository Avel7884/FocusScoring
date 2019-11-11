using System;
using System.IO;

namespace FocusScoring
{
    public static class Settings
    {
        public static bool OgrnEnabled { get; set; } = false;
        public static string CachePath { get; set; } = "./";
        public static string XMLCacheFolder { get; set; } = "XmlCache";
        public static string MarkersFolder { get; set; } = "Markers";

        private static FocusKeyManager defaultManager;
        public static FocusKeyManager DefaultManager
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
        }
    }
}