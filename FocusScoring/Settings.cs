using System.IO;

namespace FocusScoring
{
    public static class Settings
    {
        private static string cachePath = "./";
        public static string FocusKey { get; set; }
        public static bool OgrnEnabled { get; set; }

        public static string CachePath
        {
            get => cachePath;
            set => cachePath = value;
        }
    }
}