using System;
using System.CodeDom.Compiler;

namespace FocusScoring
{
    public interface IMarkerCompiler<TTarget>
    {
        string LanguageName { get; }
        
        bool IsCompiled { get; }
        CompilerErrorCollection Errors { get; }
        Func<object[], CheckResult> AddToCompilation(string code);
        //void RemoveFromCompilation(Marker<TTarget> marker);
        void Compile();
    }
}