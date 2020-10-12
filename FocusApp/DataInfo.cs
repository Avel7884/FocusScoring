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
        IReadOnlyList<SubjectParameter> MemoryOrderOfParameters { get; }
        bool TryRecallOrCreateColumn(int index, SubjectParameter newParameter);
        void ForgetColumn(int index);
    }

    public class DataInfo : IDataInfo
    {
        private int? length;
        private readonly IList<SubjectParameter> memoryOrderOfParameters;
        private readonly IList<SubjectParameter> parameters;
        //private Dictionary<SubjectParameter, int> dictionaryOfParameters;

        public DataInfo(string name, IList<SubjectParameter> parameters = null,IList<SubjectParameter> memoryOrder =null)
        {
            Name = name;
            this.parameters = parameters ?? new []{SubjectParameter.Address,SubjectParameter.Score};
            /*dictionaryOfParameters = this.parameters
                .Zip(Enumerable.Range(0, this.parameters.Count), ValueTuple.Create)
                .ToDictionary(x => x.Item1, x => x.Item2);*/
            memoryOrderOfParameters = memoryOrder ?? this.parameters; //Enumerable.Range(0, this.parameters.Count).ToList();
        }

        public string Name { get; set; }
        public string Description  { get; set; }

        public int Length
        {
            get => length ?? throw new ArgumentException();
            set => length = value;
        }

        public IReadOnlyList<SubjectParameter> Parameters => parameters as IReadOnlyList<SubjectParameter>; //TODO check data integrity 

        public IReadOnlyList<SubjectParameter> MemoryOrderOfParameters => memoryOrderOfParameters as IReadOnlyList<SubjectParameter>;
        public bool TryRecallOrCreateColumn(int index, SubjectParameter newParameter)
        {
            parameters.Insert(index,newParameter);
            /*if(!dictionaryOfParameters.TryGetValue(newParameter,out var memoryIndex))
                memoryIndex = dictionaryOfParameters.Count;*/
            var hasRecall = MemoryOrderOfParameters.Contains(newParameter);
            if(!hasRecall) memoryOrderOfParameters.Add(newParameter);
            return hasRecall;
            /*
            var memoryIndex = MemoryOrderOfParameters.Index()
                .Where(x => x.Value == newParameter)
                .Select(x => x.Key)
                .DefaultIfEmpty(-1)
                .First();*/

        }

        public void ForgetColumn(int index)
        {
            parameters.RemoveAt(index);
            memoryOrderOfParameters.RemoveAt(index);
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}