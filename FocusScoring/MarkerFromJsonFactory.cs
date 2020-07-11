/*using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using FocusApiAccess;

namespace FocusScoring
{
    class MarkerFromJsonFactory<TTarget> //: IMarkerFromJSONFactory<TTarget>
    { //TODO ensure uniques by name of markers created
        //private readonly IChecksProvider<TTarget> checksProvider;
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Marker<TTarget>),
            new XmlRootAttribute() {ElementName = "items"});
        
        private readonly string markersPath = Settings.CachePath+Settings.MarkersFolder;
        
        public MarkerFromJsonFactory(IChecksProvider<TTarget> checksProvider)
        {
            //this.checksProvider = checksProvider;
        }
        
        public Marker<TTarget> CreateNew(string Name, MarkerColour colour, string description, int Score)
        {
            
        }

        public Marker<TTarget> GetMarkerByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Marker<TTarget> marker)
        {   
            var filePath = Path.Combine(markersPath,marker.GetCodeClassName());
            if (File.Exists(filePath)) 
                File.Delete(filePath);
        }

        public IEnumerable<Marker<TTarget>> DeserializeAll()
        {//TODO make some way to include/exclude markers 
            foreach (var fileName in Directory.EnumerateFiles(markersPath))
                using (var file = File.Open(markersPath + "/" + fileName, FileMode.OpenOrCreate))
                {
                    var marker = (Marker<TTarget>)serializer.Deserialize(file);
                    if (marker != null &&
                        marker.CheckArguments.TryGetValue(checksProvider.MarkerArgName, out var value))
                        yield return marker;
                }
        }

        private void SaveMarker(Marker<TTarget> marker)
        {
            
        }
    }
}*/