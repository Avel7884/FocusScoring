namespace FocusScoring
{
    public class CompanyParameter
    {
        public CompanyParameter(string name, string description, ApiMethod method, string path)
        {
            Description = description;
            Path = path;
            Method = method;
            Name = name;
        }

        public string Name { get; }
        public ApiMethod Method { get; }
        internal string Path { get; }
        public string Description { get; }
    }
}