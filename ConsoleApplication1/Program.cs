using System;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var left = "{  \"author\": {    \"first\": \"s\",    \"last\": \"bish\"  }}";
            var right = "{  \"author\": {    \"first\": \"w\",    \"last\": \"bish\"  }}";
            
            var jdp = new JsonDiffPatch();
            JToken diff = jdp.Diff(left, right);    
            Console.WriteLine(diff);
            Console.ReadKey();
        }
    }
}