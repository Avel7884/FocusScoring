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

    internal class MarkerRTCompiler<TTarget> : MarkerCompiler<TTarget>
    {                                        //TODO Get interface back
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
        private Dictionary<Marker<TTarget>,ResultHolder> holders = new Dictionary<Marker<TTarget>, ResultHolder>();
        private bool isCompiled;
        private CompilerErrorCollection errors;

        public override bool IsCompiled => isCompiled;

        public override CompilerErrorCollection Errors => errors;

        public override Func<TTarget, MarkerResult<TTarget>> AddToCompilation(Marker<TTarget> marker)
        {
            isCompiled = false;
                 
            var ccl =  new ResultHolder(marker);
            holders[marker] = ccl;                
            
            return c =>
            {
                if (!IsCompiled)
                    Compile();
                if (!ccl.IsCompiled)
                    return new MarkerResult<TTarget>(false,marker,""); 
                return new MarkerResult<TTarget>(ccl.Check(c), marker, (string) ccl.Verbose.GetValue(null));
            };
        }

        public override void RemoveFromCompilation(Marker<TTarget> marker) => holders.Remove(marker);

        public override void Compile() //TODO save assembly (or bring it to agile)
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

            errors = result.Errors;
            if (Errors.HasErrors)
                return;
            
            isCompiled = true;
            foreach (var holder in holders.Values)
                holder.TakeResult(result);
        }
        
        //TODO rename
        private class ResultHolder
        {
            private string Name { get; }
            public string Code { get; }
            public bool IsCompiled { get; private set; }
            public Func<TTarget,bool> Check { get; private set; }
            public FieldInfo Verbose { get; private set; }

            public ResultHolder(Marker<TTarget> marker)
            {
                this.Name = 
                            //TODO rewrite with @$ or something
                Code = classCore.Replace("__Name", Name)
                                .Replace("__Code", marker.Code)//CheckArguments["C#Code"])
                                .Replace("__Parameters",
                                    string.Join(",",marker.Methods.Select(x => Api3.GetType(x) + " " + x)));
                IsCompiled = false;
            }

            public void TakeResult(CompilerResults result)
            {
                var method = result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetMethod("Function");
                Verbose= result.CompiledAssembly.GetType("MarkersCheckers."+Name).GetField("verbose");
                Check = (Func<TTarget, bool>) Delegate.CreateDelegate(typeof(Func<INN, bool>), method);
                IsCompiled = true;
            }
        }
    }
}