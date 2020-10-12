using System;
using System.Dynamic;
using System.Net.Sockets;
using FocusAccess;

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
        Inn,
        Name,
        Address,
        Score,
        FIO,
        Phone,
        Site,
        Shield
    }

    public static class SubjectParameterExtensions
    {
        public static bool IsGenerated(this SubjectParameter parameter) =>
            parameter == SubjectParameter.Shield;

        public static bool IsEssential(this SubjectParameter parameter) =>
            parameter switch
            {
                SubjectParameter.Inn => true,
                SubjectParameter.Score => true,
                _ => false
            };
    }
}