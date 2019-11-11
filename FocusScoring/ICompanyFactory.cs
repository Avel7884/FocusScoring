namespace FocusScoring
{
    public interface ICompanyFactory
    {
        Company CreateFromInn(string inn);
    }
}