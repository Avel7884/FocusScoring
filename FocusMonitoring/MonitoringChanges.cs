using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace FocusMonitoring
{
    public class MonitoringChanges<TResultValue>
    {
        public MonitoringChange[] Changes { get; }

        public string[] GetChanges()
        {
            throw new NotImplementedException();
        }
    }
    
    public class MonitoringChange
    {
        
        public MonitoringChange(string line )
        {
            var (date, diff, _) = line.Split(' ').ToArray();
            Date = DateTime.Parse(date);
            Diff = diff;
        }
        
        public MonitoringChange(DateTime date, string diff )
        {
            Date = date;
            Diff = diff;
        }
        
        public DateTime Date { get; }
        public string Diff { get; }


        public string[] ParseAsParameterValue<T>(Expression<Func<T,string>> expression)
        {/*
            var path = new Stack<string>();
            var member = expression.Body as MemberExpression;
            while (member != null)
            {   TODO finish
                path.Push(member.Type.ToString());
                member.
            }
            path.Add();
            expression.*/
            return new string[0];
        }

        public string[] ParseAsParameterValue(string path)
        {
            var token = (JToken) Diff;
            return (token.SelectToken(path) as JArray)?.Select(x=>x.ToString()).ToArray() 
                   ?? throw new ArgumentException("Path was not pointing to final node.");
        }
    }
}