using System;
using FocusAccess;
using FocusAccess.ResponseClasses;

namespace FocusScoring
{
    public interface IChecksProvider<TTarget>
    {
         string MarkerArgName { get; }
         Func<object[], CheckResult> ProvideCheck(string markerKey);
    }

    public interface IParameterizedChecksProvider<TTarget> : IChecksProvider<TTarget>
    {
        IMarkerParameters ProvideParameters(string markerKey);
    }

    public interface IMarkerParameters
    {
        ApiMethodEnum[] MethodsUsed { get; set; }
        ApiMethodEnum[] History { get; set; }
    }

    class MarkerParameters : IMarkerParameters
    {
        public ApiMethodEnum[] MethodsUsed { get; set; }
        public ApiMethodEnum[] History { get; set; }
    }

    /*class ChecksProviderFromLibrary<TTarget> : IChecksProvider<TTarget>
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
    }*/
}