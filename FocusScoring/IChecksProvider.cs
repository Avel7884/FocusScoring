using System;

namespace FocusScoring
{
    public interface IChecksProvider<TTarget>
    {
         string MarkerArgName { get; }
         Func<TTarget, MarkerResult<TTarget>> Provide(Marker<TTarget> Marker);
    }
}