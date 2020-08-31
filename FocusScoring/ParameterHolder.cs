using FocusAccess;
using FocusAccess.ResponseClasses;

namespace FocusScoring
{
    public class APIMethodProvider
    {
        private readonly MethodValueHolder[] values;

        //public ApiMethodEnum[] ApiMethodEnum { get; }
        public APIMethodProvider(MethodValueHolder[] values)
        {
            this.values = values;
        }
        
    }
}