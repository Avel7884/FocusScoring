using System;
using FocusAccess.ResponseClasses;

namespace FocusScoring
{
    public interface IChecksProvider<TTarget>
    {
         string MarkerArgName { get; }
         Func<IParameterValue[], CheckResult> Provide(Marker<TTarget> Marker);
    }

    class ChecksProviderFromLibrary<TTarget> : IChecksProvider<TTarget>
    {
        public string MarkerArgName { get; }
        public Func<IParameterValue[], CheckResult> Provide(Marker<TTarget> Marker)
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

        public Func<IParameterValue[], CheckResult> Provide(Marker<TTarget> Marker) =>
            compiler.AddToCompilation(Marker.CheckArguments[MarkerArgName]);
    }
}