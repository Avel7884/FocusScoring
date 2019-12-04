using System;

namespace FocusScoring
{
    public interface ICompanyFactory
    {
        Company CreateFromInn(string inn);
        Exception Exception { get; }
    }
}