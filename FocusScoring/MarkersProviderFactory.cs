using FocusApiAccess;

namespace FocusScoring
{
    public class MarkersProviderFactory
    {
        public IMarkersProviderController<T> Create<T>()
        {
            return new MarkersProviderController<T>(new MarkersDeserializer<T>(new MarkerRTCompiler<T>()));
        }
    }
}