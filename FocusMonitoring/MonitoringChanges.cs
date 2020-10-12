using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FocusAccess.ResponseClasses;
using Newtonsoft.Json.Linq;

namespace FocusMonitoring
{
    public class MonitoringChanges<TResultValue> where TResultValue : IParameterValue
    {
        public MonitoringChanges(MonitoringChange[] changes)
        {
            Changes = changes;
        }
        
        public MonitoringChange[] Changes { get; }

        public string[] ParseAsParameterValue(Expression<Func<TResultValue,object>> expression, DateTime after = default, DateTime before = default)
        {
            if (before == default) before = DateTime.MaxValue;
            /*
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
            var token = (JToken) path;
            return (token.SelectToken(path) as JArray)?.Select(x=>x.ToString()).ToArray() 
                   ?? throw new ArgumentException("Path was not pointing to final node.");
        }
    }
    
    public class MonitoringChange
    {
        public static MonitoringChange Parse(string line)
        {
            var (date, diff, _) = line.Split(' ').ToArray();
            return new MonitoringChange(DateTime.Parse(date),diff);
        }
        
        public MonitoringChange(DateTime date, string diff )
        {
            Date = date;
            Diff = diff;
        }
        
        public DateTime Date { get; }
        public string Diff { get; }


    }
}