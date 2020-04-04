using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FocusApiAccess;
using Microsoft.CSharp;

namespace FocusScoring
{
    internal class MarkerRTCompiler
    {
        private static readonly CSharpCodeProvider Provider = new CSharpCodeProvider();
        private const string codeHead = @"
        using FocusApiAccess;
        using System.Collections.Generic;
        using System.Linq;
        using System;
            
        namespace MarkersCheckers
        {";

        private const string classCore = @"
            public static class __Name
            {         
                public static string verbose = string.Empty;
                public static bool Function(__Parameters)
                {
                    __Code
                }
            }";
        private Dictionary<Marker,ResultHolder> holders = new Dictionary<Marker, ResultHolder>();
        public bool IsCompiled { get; private set; }
        public CompilerErrorCollection Errors { get; private set; }
        
        public Func<INN, MarkerResult> PostponededCompile(Marker marker)
        {
            IsCompiled = false;
                 
            var ccl =  new ResultHolder(marker);
            holders[marker] = ccl;                
            
            return c =>
            {
                if (!IsCompiled)
                    Compile();
                if (!ccl.IsCompiled)
                    return new MarkerResult(false,marker,""); 
                return new MarkerResult(ccl.Check(c), marker, (string) ccl.Verbose.GetValue(null));
            };
        }

        public void RemoveFromCompilation(Marker marker) => holders.Remove(marker);

        public void Compile()
        {
            var sb = new StringBuilder(codeHead);
            foreach (var code in holders.Values.Select(x=>x.Code))
                sb.Append(code);
            sb.Append('}');
            
            //var ass = typeof(Company).Assembly.CodeBase;
            var param = new CompilerParameters();
            param.ReferencedAssemblies.Add("./FocusApiAccess.dll");
            param.ReferencedAssemblies.Add("./System.dll");
            param.ReferencedAssemblies.Add("./System.Core.dll");
            //param.IncludeDebugInformation = true;
            var result = Provider.CompileAssemblyFromSource(param, sb.ToString());

            Errors = result.Errors;
            if (Errors.HasErrors)
                return;
            
            IsCompiled = true;
            foreach (var holder in holders.Values)
                holder.TakeResult(result);
        }
        
        //TODO rename
        private class ResultHolder
        {
            private string Name { get; }
            public string Code { get; }
            public bool IsCompiled { get; private set; }
            
            public Func<INN,bool> Check { get; private set; }
            public FieldInfo Verbose { get; private set; }

            public ResultHolder(Marker marker)
            {
                this.Name = marker.GetCodeClassName();
                            //TODO rewrite with @$ or something
                Code = classCore.Replace("__Name", Name)
                                .Replace("__Code", marker.Code)
                                .Replace("__Parameters",
                                    string.Join(",",marker.Methods.Select(x => Api3.GetType(x) + " " + x)));
                IsCompiled = false;
            }

            public void TakeResult(CompilerResults result)
            {
                var method = result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetMethod("Function");
                Verbose= result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetField("verbose");
                Check = (Func<INN, bool>) Delegate.CreateDelegate(typeof(Func<INN, bool>), method);
                IsCompiled = true;
            }
        }
    }
}