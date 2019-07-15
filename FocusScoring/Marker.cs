using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace FocusScoring
{
    public class Marker
    {
        private const string codeCore = @"
        using System;
            
        namespace UserFunctions
        {                
            public class BinaryFunction
            {                
                public static double Function(Company company)
                {
                    return func_xy;
                }
            }
        }
    ";

        public static CSharpCodeProvider Provider = new CSharpCodeProvider();
        
        private readonly Func<Company,bool> check;
        //TODO manage access
        public int Score { get; set; }

        public Marker(string Name, MarkerColour colour, string description, int Score, Func<Company,bool> check)
        { 
            this.check = check;
            this.Name = Name;
            Colour = colour;
            Description = description;
            this.Score = Score;
        }

//        public Marker(string Name, MarkerColour colour, string description, int Score, string code)
//        {                        //Make No replace
//            var finalCode = codeCore.Replace("func_xy",code);
//            Code = code;
//            var method = Provider.CompileAssemblyFromSource(new CompilerParameters(), finalCode).CompiledAssembly
//                .GetType("UserFunctions.BinaryFunction").GetMethod("Function");
//            this.check = (Func<Company, bool>) Delegate.CreateDelegate(typeof(Func<Company, bool>), method);
//        }
        
        public string Name { get; set; }
        public MarkerColour Colour { get; set; }
        public string Description { get; set; }
            //TODO Update check
        public string Code { get; set; }
        
        public bool Check(Company company) => check(company);
    }

    public enum MarkerColour
    {
        Green,Yellow,Red,RedAffiliates,GreenAffiliates,YellowAffiliates
    }
}