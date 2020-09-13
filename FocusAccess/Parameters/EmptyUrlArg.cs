using System;
using System.Collections.Generic;

namespace FocusAccess
{
    internal class EmptyUrlArg: IQuery
    {
        public string AssembleQuery()
        {
            return "";
        }

        public string[] Keys => new string[] {};
        public string[] Values => new string[] {};

        public string MakeAlias()
        {
            throw new InvalidOperationException();
        }
    }
}