using FocusAccess;

namespace FocusScoring
{
    public class MarkerResult<TTarget>
    {
        private readonly bool check;
        public Marker<TTarget> Marker { get; }
        public string Verbose { get; }
        public TTarget Target { get; }

        public MarkerResult(bool check, Marker<TTarget> Marker, string verbose, TTarget target)
        {
            this.check = check;
            this.Marker = Marker;
            Verbose = verbose;
            Target = target;
        }

        public static implicit operator bool(MarkerResult<TTarget> result) => result.check;
    }
}