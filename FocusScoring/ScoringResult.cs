using FocusScoring;

namespace FocusApiAccess
{
    public class ScoringResult<T> : IScoringResult<T>
    {

        public MarkerResult[] Markers { get; }
        public int Score { get; }
        public T Target { get; }
        
        public ScoringResult(MarkerResult[] markers, int score, T target)
        {
            Markers = markers;
            Score = score;
            Target = target;
        }
    }
}