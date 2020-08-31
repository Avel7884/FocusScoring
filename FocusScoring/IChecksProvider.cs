using System;

namespace FocusScoring
{
    public interface IChecksProvider<TTarget>
    {
         string MarkerArgName { get; }
         Func<object[], CheckResult> Provide(Marker<TTarget> Marker);
    }

    class ChecksProviderFromLibrary<TTarget> : IChecksProvider<TTarget>
    {
        public string MarkerArgName { get; }
        public Func<object[], CheckResult> Provide(Marker<TTarget> Marker)
        {
            throw new NotImplementedException();
        }
    }

    class ChecksProviderFromCompiler<TTarget> : IChecksProvider<TTarget>
    {
        private readonly IMarkerCompiler<TTarget> compiler;
        public string MarkerArgName => compiler.LanguageName;

        public ChecksProviderFromCompiler(IMarkerCompiler<TTarget> compiler)
        {
            this.compiler = compiler;
        }

        public Func<object[], CheckResult> Provide(Marker<TTarget> Marker) =>
            compiler.AddToCompilation(Marker.CheckArguments[MarkerArgName]);
    }
}