using System;
using System.Collections.Generic;
using System.Linq;
using FocusApiAccess;
using FocusScoring;

namespace FocusApp
{
    public interface IEntryFactory<TSubject>
    {
        IList<SubjectParameter> Parameters { get; }
        DataEntry<TSubject> CreateEntry(TSubject subject);
        void UpdateEntry(DataEntry<TSubject> entry, SubjectParameter[] parameters);
    }

    public interface ISettableEntryFactory<TSubject> : IEntryFactory<TSubject>
    {
        new IList<SubjectParameter> Parameters { get; set; }
    } 

    public class EntryFactory : ISettableEntryFactory<INN>
    {
        private readonly Api3 api;
        private readonly IScorer<INN> scorer;
        private IList<SubjectParameter> parameters;

        public IList<SubjectParameter> Parameters //TODO from constructor
        {
            get => parameters;
            set => parameters = value;
        }

        public EntryFactory(Api3 api, IScorer<INN> scorer)
        {
            this.api = api;
            this.scorer = scorer;
        }

        public DataEntry<INN> CreateEntry(INN subject)
        {
            var result = scorer.Score(subject);
            return new DataEntry<INN>
            {
                Subject = subject,
                Score = result.Score,
                Data = parameters.Select(p=>ExtractParameter(p,subject,result.Score)).ToList(),
                Light = GetLight(result.Score),
                Markers = result.Markers
            };
        }

        public void UpdateEntry(DataEntry<INN> entry, SubjectParameter[] parameters)
        {
            foreach (var parameter in parameters)
                entry.Data.Add(ExtractParameter(parameter,entry.Subject,entry.Score));
        }

        private Light GetLight(int score)
        {
            if (score > 69) return Light.Green;
            if (score > 39) return Light.Yellow;
            return Light.Red;
        }

        private string ExtractParameter(SubjectParameter parameter,INN subject, int score)
        {                    //TODO consider better option
            switch (parameter)
            {
                case SubjectParameter.Address:
                    return api.Req.MakeRequest(subject).Ul.LegalAddress.ParsedAddressRf.House.TopoFullName;
                case SubjectParameter.Name:
                    return api.Req.MakeRequest(subject).Ul.LegalName.Short;
                case SubjectParameter.Inn:
                    return subject.ToString();
                case SubjectParameter.Score:
                    return score.ToString();
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }
    }
}