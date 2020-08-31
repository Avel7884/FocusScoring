using FocusScoring;

namespace FocusAccess
{
    public interface IScoringResult<T>
    {
        MarkerResult<T>[] Markers { get; }
        int Score { get; }
        T Target { get; }    
        
    }
}