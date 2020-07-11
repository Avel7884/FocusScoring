using FocusApiAccess;

namespace FocusScoring
{
    public static class ScorerFactory
    {
        public static IScorer<INN> CreateDefaultINNScorer()
        {
            return new Scorer<INN>(new MarkersProviderController<INN>(new MarkersDeserializer<INN>(new MarkerRTCompiler<INN>())));
        }
    }
}