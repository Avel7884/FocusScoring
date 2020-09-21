using System;

namespace FocusAccess
{
    public static class QueryFactory
    {
        public static IQuery CreateForm(IQueryable obj)
        {
            switch (obj)
            {
                case INN inn: return (InnUrlArg) inn;
                default: throw new NotImplementedException();
            }
        }
    }
}