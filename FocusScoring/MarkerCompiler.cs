using System;
using System.CodeDom.Compiler;
using FocusApiAccess;

namespace FocusScoring
{
    public abstract class MarkerCompiler<TTarget> : IChecksProvider<TTarget>
    {
        public abstract bool IsCompiled { get; }
        public abstract CompilerErrorCollection Errors { get; }
        public abstract Func<TTarget, MarkerResult<TTarget>> AddToCompilation(Marker<TTarget> marker);
        public abstract void RemoveFromCompilation(Marker<TTarget> marker);
        public abstract void Compile();
        public string MarkerArgName { get; }
        public Func<TTarget, MarkerResult<TTarget>> Provide(Marker<TTarget> Marker) => 
            AddToCompilation(Marker);
    }
}