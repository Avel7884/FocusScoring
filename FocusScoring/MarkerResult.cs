using FocusApiAccess;

namespace FocusScoring
{
    public class MarkerResult<T>
    {
        private readonly bool check;
        public Marker<T> Marker { get; }
        public string Verbose { get; }

        public MarkerResult(bool check, Marker<T> Marker, string verbose)
        {
            this.check = check;
            this.Marker = Marker;
            Verbose = verbose;
        }

        public static implicit operator bool(MarkerResult<T> result) => result.check;
    }
}