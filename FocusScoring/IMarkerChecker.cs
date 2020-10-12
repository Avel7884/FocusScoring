using System;
using System.Diagnostics;
using System.Linq.Expressions;
using FocusAccess;
using FocusAccess.ResponseClasses;

namespace FocusScoring
{
    public interface IMarkerChecker<TTarget>
    {
        IMarkerParameters Parameters { get; }
        MarkerResult<TTarget> Check(TTarget target, object[] values);
    }

    public class MarkerChecker<TTarget> : IMarkerChecker<TTarget>
    {
        readonly Marker<TTarget> parametrizedMarker;
        readonly Func<object[], CheckResult> check;

        public MarkerChecker(ParametrizedMarker<TTarget> parametrizedMarker,Func<object[], CheckResult> check)
        {
            this.parametrizedMarker = parametrizedMarker;
            this.check = check;
            Parameters = parametrizedMarker.Parameters;
        }
        public MarkerChecker(Marker<TTarget> parametrizedMarker,Func<object[], CheckResult> check, IMarkerParameters parameters)
        {
            this.parametrizedMarker = parametrizedMarker;
            this.check = check;

            Parameters = parameters;

        }
        
        public MarkerResult<TTarget> Check(TTarget target, object[] values)
        {
            var checkResult = check.Invoke(values);
            return new MarkerResult<TTarget>(checkResult.Result, parametrizedMarker, checkResult.Verbose, target);
        }

        public IMarkerParameters Parameters { get; protected set; }
    }
/*

    public class INNChecker : MarkerChecker<INN>
    {
        public static INNChecker OnAnalytics(string code)
        {
            var prop = typeof(Analytics).GetProperty(code);
            if(prop == null) throw new Exception("No such analytic value.");
            var expr = Expression.Property(Expression.Parameter(typeof(AnalyticsValue)),prop);
            return null;// new INNChecker();
        }

        public INNChecker(Marker<INN> parametrizedMarker, Func<object[], CheckResult> check) : base(parametrizedMarker, check)
        {
            this.parametrizedMarker = parametrizedMarker;
            this.check = check;
            Parameters = parametrizedMarker.Methods;
        }
    }*/
}