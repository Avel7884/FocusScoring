using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace FocusScoring
{
    public class MarkerRTCompiler
    {
        private static readonly CSharpCodeProvider Provider = new CSharpCodeProvider();
        private const string codeHead = @"
        using FocusScoring;
        using System.Collections.Generic;
        using System.Linq;
            
        namespace MarkersCheckers
        {";

        private const string classCore = @"
            public static class __Name
            {

                private static bool DoubleTryParse(string param, out double result)
                {
                    return double.TryParse(param.Replace('.', ','), out result);
                }            

                public static string verbose = string.Empty;
                public static bool Function(Company company)
                {
                    __Code
                }
            }";
        private Dictionary<Marker,ResultHolder> holders = new Dictionary<Marker, ResultHolder>();
        private bool IsCompiled;
        
        public Func<Company, MarkerResult> PostponededCompile(Marker marker)
        {
            IsCompiled = false;
                 
            var ccl =  new ResultHolder(marker.Name.Replace(' ','_').Replace('.','_'),marker.Code);
            holders[marker] = ccl;                
            
            return c =>
            {
                if (!IsCompiled)
                    Compile();
                return new MarkerResult(ccl.Check(c), marker, (string) ccl.Verbose.GetValue(null));
            };
        }

        private void Compile()
        {
            IsCompiled = true;
            var sb = new StringBuilder(codeHead);
            foreach (var code in holders.Values.Select(x=>x.Code))
                sb.Append(code);
            sb.Append('}');
            
            //var ass = typeof(Company).Assembly.CodeBase;
            var param = new CompilerParameters();
            param.ReferencedAssemblies.Add("./FocusScoring.dll");
            param.ReferencedAssemblies.Add("./System.dll");
            param.ReferencedAssemblies.Add("./System.Core.dll");
            //param.IncludeDebugInformation = true;
            var result = Provider.CompileAssemblyFromSource(param, sb.ToString());

            foreach (var holder in holders.Values)
                holder.TakeResult(result);
        }
        //TODO rename
        private class ResultHolder
        {
            public string Name { get; }

            public ResultHolder(string Name, string code)
            {
                this.Name = Name;
                            //TODO rewrite with @$ or something
                Code = classCore.Replace("__Name", Name).Replace("__Code", code);
            }
            
            public string Code { get; }
            
            public void TakeResult(CompilerResults result)
            {
                var method = result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetMethod("Function");
                Verbose= result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetField("verbose");
                Check = (Func<Company, bool>) Delegate.CreateDelegate(typeof(Func<Company, bool>), method);
            } 
            
            public Func<Company,bool> Check { get; set; }
            public FieldInfo Verbose { get; set; }
        }
    }
}