using System;
using System.Collections.Generic;
using System.Linq;

namespace FocusAccess
{
    public class INN : IQueryable//: IUrlQueryArg
    {
        private string value;
        private bool isOGRN;


        public bool IsFL() => value.Length == 12;
        
        public static bool TryParse(string str,out INN req)
        {
            req= new INN();
            if (!InnCheckSum(str)) return false;
            req.isOGRN = str.Length > 12;
            req.value = str;
            return true;
        }

        public static INN CreateUnsafe(string str)
        {
            var req= new INN();
            req.isOGRN = str.Length > 12;
            req.value = str;
            return req;
        }

        private static readonly int[] k = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };

        private static bool InnCheckSum(string inn)    
        {
            var numbers = inn.Select(x => new string(new[] { x })).Select(int.Parse).ToArray();
            if (numbers.All(x => x == 0))
                return false;

            switch (numbers.Length)
            {
                case 10:
                    return numbers.Take(9).Zip(k.Skip(2), (x, y) => x * y).Sum() % 11 % 10 == numbers[9];
                case 12:
                    return numbers.Take(10).Zip(k.Skip(1), (x, y) => x * y).Sum() % 11 % 10 == numbers[10] &&
                           numbers.Take(11).Zip(k, (x, y) => x * y).Sum() % 11 % 10 == numbers[11];
                case 13:
                    var res = long.Parse(inn.Substring(0, 12)) % 11;
                    return (res == 10 ? 0 : res) == int.Parse(inn.Substring(12, 1));
                default:
                    return false;
            }
        }
        
        public override string ToString()=>value;
/*

        public string ToQueryArg()
        {
            return (isOGRN ? "ogrn" : "inn") + "=" + value;
        }*/

        public static implicit operator INN(string val)
        {
            if(!TryParse(val,out var inn))
                throw new FormatException("Некорректный инн.");
            return inn;
        }
    }
}