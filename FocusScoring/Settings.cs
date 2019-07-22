using System.IO;

namespace FocusScoring
{
    public static class Settings
    {
        public static string FocusKey { get; set; }
        public static bool OgrnEnabled { get; set; }

        public static string CachePath { get; set; } = "./";
                //Might use dictionays in case mulitiple keys 
        public static int UsagesLeft { get; internal set; } 
    }
}