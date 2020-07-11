/*using System;
using System.Linq;
using FocusApiAccess;
using FocusScoring;

namespace FocusApp
{
    //TODO reintroduce in case of optimization 
    public class ApiAdapter<TSubject>
    {
        private readonly Api3 api;

        public ApiAdapter(IScorer<TSubject> scorer,Api3 api)
        {
            this.api = api;
        }

        public string[] GetParameter(TSubject subject, SubjectParameter[] parameters)
        {
            return parameters.Select(p => GetParameter(subject, p)).ToArray();
        }

        public string GetParameter(TSubject subject,SubjectParameter parameter)
        {
            switch (parameter)
            {
                case SubjectParameter.Address:
                    api.Req.MakeRequest(subject)
                    break;
                case SubjectParameter.Name:
                    
                    break;
                case SubjectParameter.Inn:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }
    }
}*/