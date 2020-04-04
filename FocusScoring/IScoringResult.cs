using FocusScoring;

namespace FocusApiAccess
{
    public interface IScoringResult<out T>
    {
        MarkerResult[] Markers { get; }
        int Score { get; }
        T Target { get; }    
    }
}