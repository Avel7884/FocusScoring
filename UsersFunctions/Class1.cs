
using System;
using System.Collections.Generic;
using System.Linq;
using FocusScoring;
            
namespace UserFunctions
{                
    public class BinaryFunction
    {                
        public static bool Function(Company company)
        {
            return company.GetParam("Dissolving") == "true" || company.GetParam("Dissolved") == "true";
        }
    }
}
