using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusApiAccess;

namespace FocusScoring
{
    class Scorer<T> : IScorer<T>
    {
        private readonly IMarkersProvider<T> provider;

        public Scorer(IMarkersProvider<T> provider)
        {
            this.provider = provider;
        }

        public IScoringResult<T> Score(T inn)
        {
            var markers = provider.Markers.Select(marker => marker.Check(inn)).Where(x => x).ToArray();
            return new ScoringResult<T>(markers, CountScore(markers.Select(x => x.Marker).ToArray()),inn);
        }

        /*public async Task<IScoringResult<T>> ScoreAsync(T inn)
        {        
            var markers = await Task.WhenAll(Markers.Select(m=>m.CheckAsync(inn)));
            return new ScoringResult<T>(markers,CountScore(markers.Where(r=>r).Select(r=>r.Marker).ToArray()),inn);
            
            //var results = new MarkerResult<INN>[markersDict.Count];
            //markers = markersDict.Values.Select(marker => marker.Check(inn)).Where(x => x).ToArray();
            //Task.WaitAll(Enumerable.Range(0,markersDict.Count).Select(i=>new Task()));
        }

        public async Task<IScoringResult<T>[]> ScoreAsync(T[] inns)
        {
            var results = new ScoringResult<T>[inns.Length];
            await Task.WhenAll(Enumerable.Range(0, inns.Length)
                .Select(i => ScoreAsync(inns[i])).ToArray());
            return results; //TODO error thing!
        }
*/

        private static int CountScore(Marker<T>[] markers)
        {
            const int yellowBound = 69;
            const int redBound = 39;
            
            if (markers.Any(m =>
                m.Score == 5 && (m.Colour == MarkerColour.Red || m.Colour == MarkerColour.RedAffiliates)))
                return 0;
            var colourScore = markers.Where(m => m.Colour == MarkerColour.Red || m.Colour == MarkerColour.RedAffiliates)
                .Select(m => m.Score).Sum();
            
            if (colourScore > 0)
                return Math.Max(0, redBound - colourScore);
            
            colourScore = markers.Where(m => m.Colour == MarkerColour.Yellow || m.Colour == MarkerColour.YellowAffiliates)
                .Select(m => m.Score).Sum();
            if (colourScore > 0)
                return Math.Max(0, yellowBound - colourScore);
            
            colourScore = markers.Where(m => m.Colour == MarkerColour.Green || m.Colour == MarkerColour.GreenAffiliates)
                .Select(m => m.Score).Sum();
            return colourScore > 0 ? Math.Min(100, yellowBound + 1 + colourScore) : 0;
        }
    }
}