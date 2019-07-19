using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace FocusScoring
{
    public class Marker
    {

        private readonly Func<Company,MarkerResult> check;
        private static readonly MarkerRTCompiler compiler = new MarkerRTCompiler();
        
        private int score;
        public int Score
        {
            get => score;
            set
            {
                if (value>5 || value<0)
                    throw new ArgumentException();
                score = value;
            }
        }
        
        
        //public Marker(string Name, MarkerColour colour, string description, int Score, Func<Company,MarkerResult>)

        public Marker(string Name, MarkerColour colour, string description, int Score, Func<Company, string> check)
        {
            this.check = x =>
            {
                var res = check.Invoke(x);
                return new MarkerResult(res != null, this, res);
            };
            this.Name = Name;
            Colour = colour;
            Description = description;
            Code = "Unavalable";
            this.Score = Score;
        }

        public Marker(string Name, MarkerColour colour, string description, int Score, Func<Company, bool> check, string verbose = "")
        {
            this.check = x => new MarkerResult(check.Invoke(x), this, verbose);
            this.Name = Name;
            Colour = colour;
            Description = description;
            Code = "Unavalable";
            this.Score = Score;
        }

        public Marker(string Name, MarkerColour colour, string description, int Score, string code)
        {   
            this.Name = Name;
            Colour = colour;
            Description = description;
            this.Score = Score;
            Code = code;
            check = compiler.PostponededCompile(this);
        }
        
        public string Name { get; set; }
        public MarkerColour Colour { get; set; }
        public string Description { get; set; }
        //TODO Update check
        public string Code { get; set; }

        public MarkerResult Check(Company company) => check(company);
    }

    public enum MarkerColour
    {
        Green, Yellow, Red, RedAffiliates, GreenAffiliates, YellowAffiliates
    }
}