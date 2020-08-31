using System;
using System.Collections.Generic;
using System.Linq;
using FocusAccess;

namespace FocusApp
{
    public interface IDataInfo
    {
        string Name { get; set; }
        string Description { get; set; }
        int Length { get; set; }
        IReadOnlyList<SubjectParameter> Parameters { get; }
        IReadOnlyList<int> MemoryOrderOfParameters { get; }
        void InsertColumn(int index, SubjectParameter newParameter);
        void RemoveColumn(int index);
    }

    public class DataInfo : IDataInfo
    {
        private int? length;
        private readonly IList<int> memoryOrderOfParameters;
        private readonly IList<SubjectParameter> parameters;
        private Dictionary<SubjectParameter, int> dictionaryOfParameters;

        public DataInfo(string name, IList<SubjectParameter> parameters = null)
        {
            Name = name;
            this.parameters = parameters ?? new []{SubjectParameter.Address,SubjectParameter.Score};
            dictionaryOfParameters = this.parameters.Zip(Enumerable.Range(0, this.parameters.Count), ValueTuple.Create)
                .ToDictionary(x => x.Item1, x => x.Item2);
            memoryOrderOfParameters = Enumerable.Range(0, this.parameters.Count).ToList();
        }

        public string Name { get; set; }
        public string Description  { get; set; }

            public int Length
            {
                get => length ?? throw new ArgumentException();
                set => length = value;
            }

        public IReadOnlyList<SubjectParameter> Parameters => parameters as IReadOnlyList<SubjectParameter>; //TODO check data integrity 

        public IReadOnlyList<int> MemoryOrderOfParameters => memoryOrderOfParameters as IReadOnlyList<int>;
        public void InsertColumn(int index, SubjectParameter newParameter)
        {
            parameters.Insert(index,newParameter);
            if(!dictionaryOfParameters.TryGetValue(newParameter,out var memoryIndex))
                memoryIndex = dictionaryOfParameters.Count;
            memoryOrderOfParameters.Insert(index,memoryIndex);
        }

        public void RemoveColumn(int index)
        {
            parameters.RemoveAt(index);
            memoryOrderOfParameters.RemoveAt(index);
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}