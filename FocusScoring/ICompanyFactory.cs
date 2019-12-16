using System;

namespace FocusScoring
{
    public interface ICompanyFactory
    {
        Company CreateFromInn(INN inn);
        Exception Exception { get; }
    }
}