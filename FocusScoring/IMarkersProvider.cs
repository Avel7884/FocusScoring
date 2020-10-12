using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FocusAccess;

namespace FocusScoring
{
    public interface IMarkersProvider<TTarget>
    {
        Marker<TTarget>[] Markers { get; }
    }

    class MarkersDeserializer<TTarget> : IMarkersProviderController<TTarget>
    {
        //private readonly IChecksProvider<TTarget> checksProvider;
        private readonly string markersPath;
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Marker<TTarget>),
            new XmlRootAttribute() {ElementName = "items"});

        private Marker<TTarget>[] markers;

        public MarkersDeserializer(IChecksProvider<TTarget> checksProvider, string path = null)
        { //TODO ensure uniques by name of markers created
            markersPath = path ?? Settings.CachePath + Settings.MarkersFolder;
            //this.checksProvider = checksProvider;
            markers = GetMarkers().ToArray();
        }

        public MarkersDeserializer(string path = null)
        { //TODO ensure uniques by name of markers created
            markersPath = path ?? Settings.CachePath + Settings.MarkersFolder;
            //this.checksProvider = checksProvider;
            markers = GetMarkers().ToArray();
        }

        private IEnumerable<Marker<TTarget>> GetMarkers()
        {
            foreach (var fileName in Directory.EnumerateFiles(markersPath))
                using (var file = File.Open( fileName, FileMode.OpenOrCreate))
                {
                    var marker = (Marker<TTarget>)serializer.Deserialize(file);
                    if (marker != null) //&&
                        //                marker.CheckArguments.TryGetValue(checksProvider.MarkerArgName, out var value))
                        yield return marker; //ProvideCheck(marker);
                }
        }/*

        private Marker<TTarget> ProvideCheck(Marker<TTarget> marker)
        {
            marker.SetCheck(checksProvider.Provide(marker));
            return marker;
        }*/

        public void Include(Marker<TTarget> marker)
        {
            using (var file = CreateMarkerFile(marker))
                serializer.Serialize(file,marker);
            markers = markers.Concat(markers).ToArray();
        }

        private FileStream CreateMarkerFile(Marker<TTarget> marker)
        {
            if (!Directory.Exists(markersPath))
                Directory.CreateDirectory(markersPath);
            return File.Open(markersPath + "/" + marker.GetCodeClassName(), FileMode.OpenOrCreate);
        }

        public void Exclude(Marker<TTarget> marker)
        {
            var path = markersPath + "/" + marker.GetCodeClassName();
            if (!File.Exists(path)) return;
            File.Delete(path);
            markers = markers.Where(x => x != marker).ToArray();
        }

        public Marker<TTarget>[] Markers => markers;
    }
    
    public interface IMarkersProviderController<TTarget> : IMarkersProvider<TTarget>
    {
        void Include(Marker<TTarget> marker);
        void Exclude(Marker<TTarget> marker);
    }

    class MarkersProviderController<TTarget> : IMarkersProviderController<TTarget>
    {
        private readonly IMarkersProvider<TTarget> source;
        private readonly HashSet<Marker<TTarget>> excluded = new HashSet<Marker<TTarget>>(); 

        public MarkersProviderController(IMarkersProvider<TTarget> source)
        {
            this.source = source;
        }

        public void Include(Marker<TTarget> marker)
        {
            excluded.Remove(marker);
        }

        public void Exclude(Marker<TTarget> marker)
        {
            excluded.Add(marker);
        }

        public Marker<TTarget>[] Markers =>
            source.Markers.Where(x=>!excluded.Contains(x)).ToArray();
    }
}