using System;

namespace FocusAccess
{
    public class QUrlArg : Query //TODO Rename
    {
        public QUrlArg(string query) : base(query)
        {}

        public override string[] Keys { get; } = {"q"};
    }
}