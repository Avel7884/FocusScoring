using System;
using FocusApiAccess;
using FocusApiAccess.ResponseClasses;

namespace FocusScoring
{
    public struct MethodValueHolder
    {
        public MethodValueHolder(ApiMethodEnum method, IParameterValue value)
        {
            if(Api3.GetType(method).IsInstanceOfType(value))
                throw new ArgumentException("");//TODO make exception string
            Value = value;
            Method = method;
        }

        public ApiMethodEnum Method { get; }
        public IParameterValue Value { get; }
    }
}