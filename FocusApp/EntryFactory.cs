using System;
using System.Collections.Generic;
using System.Linq;
using FocusAccess;
using FocusScoring;

namespace FocusApp
{
    public interface IEntryFactory<TSubject>
    {
        IReadOnlyList<SubjectParameter> Parameters { get; }
        DataEntry<TSubject> CreateEntry(TSubject subject);
        void UpdateEntry(DataEntry<TSubject> entry, SubjectParameter[] parameters);
    }

    public interface ISettableEntryFactory<TSubject> : IEntryFactory<TSubject>
    {
        new IReadOnlyList<SubjectParameter> Parameters { get; set; }
    } 

    public class EntryFactory : ISettableEntryFactory<INN>
    {
        private readonly IApi3 api;
        private readonly IScorer<InnUrlArg> scorer;
        private IReadOnlyList<SubjectParameter> parameters;

        public IReadOnlyList<SubjectParameter> Parameters //TODO from constructor
        {
            get => parameters;
            set => parameters = value;
        }

        public EntryFactory(IApi3 api, IScorer<InnUrlArg> scorer)
        {
            this.api = api;
            this.scorer = scorer;
        }

        public DataEntry<INN> CreateEntry(INN subject)
        {
            var result = scorer.Score(subject);
            return new DataEntry<INN>(subject,result.Score
                ,GetLight(result.Score),
                parameters.Select(p=>ExtractParameter(p,subject,result.Score)).ToList(),
                result.Markers);
        }

        public void UpdateEntry(DataEntry<INN> entry, SubjectParameter[] parameters)
        {
            foreach (var parameter in parameters)
                entry.Insert(entry.Data.Count,ExtractParameter(parameter,entry.Subject,entry.Score));
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
                    return subject.IsFL
                        ? "У ИП отсутствует адресс."
                        : api.Req(subject).Address;
                case SubjectParameter.Name:
                    return subject.IsFL 
                        ? api.Req(subject).Ip.Fio 
                        : api.Req(subject).Ul.LegalName.Short;
                case SubjectParameter.Inn:
                    return subject.ToString();
                case SubjectParameter.Score:
                    return score.ToString();
                case SubjectParameter.FIO:
                    return subject.IsFL
                        ? api.Req(subject).Ip.Fio
                        : api.Req(subject).Ul.Heads[0].Fio;
                case SubjectParameter.Site:
                    return api.Sites(subject).Sites[0];
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }
    }
}