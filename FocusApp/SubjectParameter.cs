using System;
using System.Dynamic;
using System.Net.Sockets;
using FocusApiAccess;

namespace FocusApp
{
    /*public class SubjectParameter<TSubject>
    {
        public ApiMethodEnum method { get; }
        
        public string ExtractParameter(TSubject subject,Api3 api)
        {
            data.TryDeleteMember()
            throw new NotImplementedException();
        }
    }*/

    public enum SubjectParameter
    {
        Address,
        Name,
        Inn,
        Score
    }
}