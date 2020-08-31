using Microsoft.Win32;

namespace FocusAccess
{
    public class Coder
    {
        public static void Encode(string dkey)
        {
            string ekey = "";
            foreach (var c in dkey)
            {
                if (c >= 65 & c <= 90)
                    ekey += (char)(c + 127);
                if (c >= 97 & c <= 122)
                    ekey += (char)(c + 121);
                if (c >= 48 & c <= 57)
                    ekey += (char)(c + 196);
                if (c == 32)
                    ekey += c;
            }
            using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\FocusScoring"))
                key.SetValue("fkey", ekey);

        }

        public static string Decode(string ekey)
        {
            string dkey = "";
            foreach (var c in ekey)
            {
                if (c >= 192 & c <= 217)
                    dkey += (char)(c - 127);
                if (c >= 218 & c <= 243)
                    dkey += (char)(c - 121);
                if (c >= 244 & c <= 253)
                    dkey += (char)(c - 196);
                if (c == 32)
                    dkey += c;
            }
            return dkey;
        }
        //TODO rework
    }
}
