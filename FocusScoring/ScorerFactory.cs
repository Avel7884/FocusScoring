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

        public static IScorer<InnUrlArg> CreateEmptyINNScorer() => 
            new EmptyScorer<InnUrlArg>();
        
        public static IScorer<InnUrlArg> CreateLibraryINNScorer(IApi3 api) =>
            new Scorer<InnUrlArg>(api, new CodeMarkerProvider(), new FocusChecksProvider());
    }

    public class EmptyScorer<TTarget> : IScorer<TTarget> where TTarget : IQuery
    {
        public IScoringResult<TTarget> Score(TTarget target) => 
            new ScoringResult<TTarget>(new MarkerResult<TTarget>[0],0,target);
    }
}