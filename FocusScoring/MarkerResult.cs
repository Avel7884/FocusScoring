using System;

namespace FocusScoring
{
    public class MarkerResult
    {
        private readonly bool check;
        public Marker Marker { get; }
        public string Verbose { get; }

        public MarkerResult(bool check, Marker Marker, string verbose)
        {
            this.check = check;
            this.Marker = Marker;
            Verbose = verbose;
        }

        public static implicit operator bool(MarkerResult result) => result.check;
    }
}