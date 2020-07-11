using System;
using System.Collections.Generic;
using FocusScoring;

namespace FocusApp
{
    public class DataEntry<TSubject>
    {
        public TSubject Subject { get; internal set; }
        public int Score { get; internal set; }
        public Light Light { get; internal set; }
        public IList<string> Data { get; internal set; }
        public IReadOnlyList<MarkerResult<TSubject>> Markers { get; internal set; }
        

        public void Insert(int index,string subject) => 
            Data.Insert(index,subject);

        public void RemoveAt(int index) => 
            Data.RemoveAt(index);
        
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