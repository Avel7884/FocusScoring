using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FocusScoring
{
    public class MarkerCheckers<T> : IEnumerable<IMarkerChecker<T>>
    {
        private readonly IMarkersProvider<T> markers;
        private readonly IParameterizedChecksProvider<T> checks;
        private readonly IMarkerParameters parameters;

        public MarkerCheckers(IMarkersProvider<T> markers, IParameterizedChecksProvider<T> checks)
        {
            this.markers = markers;
            this.checks = checks;
        }
        
        public IEnumerator<IMarkerChecker<T>> GetEnumerator()
        {
            return (from marker in markers.Markers 
                let markerKey = marker.CheckArguments[checks.MarkerArgName] 
                select new MarkerChecker<T>(marker,
                checks.ProvideCheck(markerKey),
                checks.ProvideParameters(markerKey))).Cast<IMarkerChecker<T>>().GetEnumerator();

            /*markers.Markers.
            
            markersProvider.Markers
                .Select(x=>new MarkerChecker<T>(x,checksProvider.Provide(x)))
                .Cast<IMarkerChecker<T>>().ToList();
            throw new System.NotImplementedException();*/
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}