using System.Net.NetworkInformation;

namespace FocusGUI
{
    public class ListCreationResult
    {
        public static ListCreationResult FromError(string error) => 
            new ListCreationResult(true, error, false);
        
        public static ListCreationResult Succsess() => 
            new ListCreationResult(false, null, true);
        
        public static ListCreationResult Pending() => 
            new ListCreationResult(false, null, false);

        public static ListCreationResult FailWithError(string error) => 
            new ListCreationResult(true, error, false);


        public ListCreationResult(bool hasErrors, string errorMessage, bool success)
        {
            HasErrors = hasErrors;
            ErrorMessage = errorMessage;
            Success = success;
        }

        public bool HasErrors { get; set; }
        public string ErrorMessage { get; set; }
        public bool Success { get; set; }
    }
}