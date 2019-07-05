using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;

namespace FocusScoring
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            Settings.FocusKey = "3c71a03f93608c782f3099113c97e28f22ad7f45";

            Console.WriteLine(Company.CreateINN("6167110026").GetMarker("func4"));
            
//            
//            var inns = Console.ReadLine().Split();
//            WebResponse response;
//            byte[] buf;
//
//            cache = new XmlCache();
//            xmlDownload = new XmlDownload(focuskey);
//
//            foreach (var inn in inns)
//            {
//                foreach (var (method,node) in companyParams)
//                    Console.WriteLine(GetParam(method,inn,node));
//            }
        }
    }
}