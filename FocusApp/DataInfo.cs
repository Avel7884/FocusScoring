using System;
using System.Collections.Generic;
using FocusApiAccess;

namespace FocusApp
{
    public class DataInfo
    {
        private int? length;

        public DataInfo(string name, IList<SubjectParameter> parameters = null)
        {
            Name = name;
            Parameters = parameters ?? new []{SubjectParameter.Address,SubjectParameter.Score};
            //Length = Parameters.Count;
        }

        public string Name { get; }
        public string Description  { get; internal set; }

        public int Length
        {
            get => length ?? throw new ArgumentException();
            internal set => length = value;
        }

        public IList<SubjectParameter> Parameters { get; }
            
        public override int GetHashCode() => Name.GetHashCode();
    }
}