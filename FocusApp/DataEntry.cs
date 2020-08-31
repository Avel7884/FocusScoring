using System;
using System.Collections.Generic;
using FocusScoring;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FocusApp
{
    [JsonObject]
    public class DataEntry<TSubject>
    {
        [JsonProperty]
        private readonly IList<string> data;
        
        [JsonProperty]
        private readonly IList<MarkerResult<TSubject>> markers;
        
        [JsonProperty]
        public TSubject Subject { get; }
        
        [JsonProperty]
        public int Score { get; }
        
        [JsonProperty]
        public Light Light { get; }
        
        [JsonIgnore]
        public IReadOnlyList<string> Data => data as IReadOnlyList<string>;
        
        [JsonIgnore]
        public IReadOnlyList<MarkerResult<TSubject>> Markers => markers as IReadOnlyList<MarkerResult<TSubject>>;

        internal DataEntry(){}
        
        public DataEntry(DataEntry<TSubject> oldEntry, string[] newData)
        {
            Subject = oldEntry.Subject;
            Score = oldEntry.Score;
            Light = oldEntry.Light;
            data = newData;
            markers = oldEntry.markers;
        } 
        
        public DataEntry(TSubject subject,int score,Light light, List<string> data, IList<MarkerResult<TSubject>> markers)
        {
            Subject = subject;
            Score = score;
            Light = light;
            this.data = data;
            this.markers = markers;
        } 

        public void Insert(int index,string subject) => 
            data.Insert(index,subject);

        public void RemoveAt(int index) => 
            data.RemoveAt(index);
        
        [JsonProperty]
        public Uri ShieldCode
        {
            get
            {
                switch (Light)
                {
                    case Light.Green: return new Uri("pack://application:,,,/src/green-shield.png");
                    case Light.Red: return new Uri("pack://application:,,,/src/red-shield.png");
                    case Light.Yellow: return new Uri("pack://application:,,,/src/yellow-shield.png");
                    case Light.Loading: return new Uri("pack://application:,,,/src/loading.png");
                    default: throw new AggregateException();
                }
            }
        }
    }
}