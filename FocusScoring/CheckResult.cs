namespace FocusScoring
{
    public class CheckResult
    {
        public bool Result { get; set; }
        public string Verbose { get; set; }

        public static CheckResult Failed() => 
            new CheckResult {Result = false};
        public static CheckResult Checked(string verbose = "") => 
            new CheckResult {Result = true,Verbose = verbose};
    }
}