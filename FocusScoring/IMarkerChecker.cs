using System;
using System.Linq.Expressions;
using FocusAccess;
using FocusAccess.ResponseClasses;

namespace FocusScoring
{
    public interface IMarkerChecker<TTarget>
    {
        ApiMethodEnum[] Methods { get; }
        MarkerResult<TTarget> Check(TTarget target, IParameterValue[] values);
    }

    public class MarkerChecker<TTarget> : IMarkerChecker<TTarget>
    {
        protected Marker<TTarget> marker;
        protected Func<IParameterValue[], CheckResult> check;

        public MarkerChecker(Marker<TTarget> marker,Func<IParameterValue[], CheckResult> check)
        {
            this.marker = marker;
            this.check = check;
            Methods = marker.Methods;
        }
        
        public MarkerResult<TTarget> Check(TTarget target, IParameterValue[] values)
        {
            var checkResult = check.Invoke(values);
            return new MarkerResult<TTarget>(checkResult.Result, marker, checkResult.Verbose, target);
        }

        public ApiMethodEnum[] Methods { get; protected set; }
    }

    public class INNChecker : MarkerChecker<INN>
    {
        public static INNChecker OnAnalytics(string code)
        {
            var prop = typeof(Analytics).GetProperty(code);
            if(prop == null) throw new Exception("No such analytic value.");
            var expr = Expression.Property(Expression.Parameter(typeof(AnalyticsValue)),prop);
            return null;// new INNChecker();
        }

        public INNChecker(Marker<INN> marker, Func<object[], CheckResult> check) : base(marker, check)
        {
            this.marker = marker;
            this.check = check;
            Methods = marker.Methods;
        }
    }
}