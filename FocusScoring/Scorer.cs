using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FocusAccess;
using FocusAccess.ResponseClasses;
using IQueryable = FocusAccess.IQueryable;

namespace FocusScoring
{
    class Scorer<T> : IScorer<T> where T : IQueryable 
    {
        private readonly IApi3 api;
        private readonly IList<IMarkerChecker<T>> markerCheckers;

        public Scorer(IApi3 api, MarkerCheckers<T> checkers)
        {
            this.api = api;
            markerCheckers = checkers.ToList(); 
        }

        public IScoringResult<T> Score(T target)
        {
            var query = QueryFactory.CreateForm(target);
            var markers = markerCheckers
                .Select(c => c.Check(target, GenerateValues(c.Parameters, query)))
                .Where(x => x).ToArray();
            return new ScoringResult<T>(markers, CountScore(markers.Select(x => x.Marker).ToArray()),target);
        }

        private object[] GenerateValues(IMarkerParameters parameters, IQuery query)
        {                                 
            return parameters.MethodsUsed
                .Select(m => m.IsMultiValue() ? api.GetValues(m, query).Cast(m.ValueType().GenericTypeArguments.First()) : api.GetValue(m, query))
                .Concat(parameters.History.Select(m => api.GetValue(m, query)))
                .ToArray();
        }

        /*    public async Task<IScoringResult<T>> ScoreAsync(T inn)
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

    static class IEnumerableExtensions
    {
        //TODO Move to separate place somewere
        private static object Cast<T>(this T[] sourceArray, Type elementType)
        {
            var destinationArray = Array.CreateInstance(elementType, sourceArray.Length);
            Array.Copy(sourceArray,destinationArray,sourceArray.Length);
            return destinationArray;
        }
    }
}