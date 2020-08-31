using FocusAccess;
using FocusMarkers;

namespace FocusScoring
{
    public static class ScorerFactory
    {
        /*
        public static IScorer<INN> CreateDefaultINNScorer()
        {
            return new Scorer<INN>(new MarkersProviderController<INN>(new MarkersDeserializer<INN>(new MarkerRTCompiler<INN>())));
        }*/

        public static IScorer<INN> CreateEmptyINNScorer() => 
            new EmptyScorer<INN>();
        
        public static IScorer<INN> CreateLibraryINNScorer(Api3 api) =>
            new Scorer<INN>(api, new CodeMarkerProvider(), new FocusChecksProvider());
    }

    public class EmptyScorer<TTarget> : IScorer<TTarget>
    {
        public IScoringResult<TTarget> Score(TTarget target) => 
            new ScoringResult<TTarget>(new MarkerResult<TTarget>[0],0,target);
    }
}