using FocusScoring;

namespace FocusApiAccess
{
    internal class ScoringResult<T> : IScoringResult<T>
    {

        public MarkerResult<T>[] Markers { get; }
        public int Score { get; }
        public T Target { get; }
        
        public ScoringResult(MarkerResult<T>[] markers, int score, T target)
        {
            Markers = markers;
            Score = score;
            Target = target;
        }
    }
}