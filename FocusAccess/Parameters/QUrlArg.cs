using System;

namespace FocusAccess
{
    public class QUrlArg : QueryComponents //TODO Rename
    {
        public QUrlArg(string query) : base(query)
        {}

        public override string[] Keys { get; } = {"q"};
    }
}