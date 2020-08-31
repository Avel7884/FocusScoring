using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FocusAccess;

namespace FocusScoring
{
    public class Marker<T>
    {
        //private Func<T, MarkerResult<T>> check;
        
        private int score;

        public int Score
        {
            get => score;
            set
            {
                if (value > 5 || value < 0)
                    throw new ArgumentException();
                score = value;
            }
        }

        public Marker()
        {
            Name = "NewMarker";
            Description = "";
            Colour = MarkerColour.Yellow;
            Score = 3;
            //Code = "return true;";
        }    

        public ApiMethodEnum[] Methods {get; set;}   //TODO remove setters after serialization tests
        public string Name { get; set; }
        public MarkerColour Colour { get; set; }
        
        
        public string Description { get; set; }
        
        public Marker<T> Parent { get; set; }
        
        public IReadOnlyDictionary<string, string> CheckArguments { get; set; }

        internal string GetCodeClassName()
        {
            return string.Concat(Name.Split(' ','-','_','.',',',')','(','%','&','$','#','\\','/','?','!','\'','\"'));
        }
        /*
        public string Code { get; set; } //TODO Make hidden version for compiled
        internal void SetCheck(Func<T, MarkerResult<T>> check)
        {
            this.check = check;
        }
        public MarkerResult<T> Check(T target) => check(target);*/
    }

    public enum MarkerColour
    {
        Green, Yellow, Red, GreenAffiliates, YellowAffiliates, RedAffiliates
    }
    
    public class Marker_OfThePast<T>
    {
        private Func<T, MarkerResult<T>> check;
        /*private static readonly MarkerRTCompiler<T> compiler = new MarkerRTCompiler<T>();
        
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Marker<T>),
            new XmlRootAttribute() {ElementName = "items"});

        public static CompilerErrorCollection CheckCodeErrors()
        {
            if(!compiler.IsCompiled)
                compiler.Compile();//I am feel deeply ashamed for making this.
            return compiler.Errors;
        }*/
        
        private int score;

        public int Score
        {
            get => score;
            set
            {
                if (value > 5 || value < 0)
                    throw new ArgumentException();
                score = value;
            }
        }
/*

        public Marker()
        {
            Name = "NewMarker";
            Description = "";
            Colour = MarkerColour.Yellow;
            Score = 3;
            //Code = "return true;";
        }*/
        
        /*
        public Marker(string Name, MarkerColour colour, string description, int Score, Func<T, string> check)
        {
            this.check = x =>
            {
                var res = check.Invoke(x);
                return new MarkerResult<T>(res != null, this, res);
            };
            
            this.Name = Name;
            Colour = colour;
            Description = description;
            code = "Unavalable";
            this.Score = Score;
        }

        public Marker(string Name, MarkerColour colour, string description, int Score, Func<T, bool> check, string verbose = "")
        {
            this.check = x => new MarkerResult<T>(check.Invoke(x), this, verbose);
            this.Name = Name;
            Colour = colour;
            Description = description;
            code = "Unavalable";
            this.Score = Score;
        }

        */
        /*public Marker(string Name, MarkerColour colour, string description, int Score, ApiMethodEnum[] methods, string code)
        {   
            this.Name = Name;
            Methods = methods.ToHashSet(); //TODO value check and error
            Colour = colour;
            Description = description;
            this.Score = Score;
            Code = code;
        }*/
        
        public HashSet<ApiMethodEnum> Methods {get; set;}   //TODO remove setters after serialization tests
        public string Name { get; set; }
        public MarkerColour Colour { get; set; }
        public string Description { get; set; }

        //private string code;

        //public string Code { get; set; }
        /*{
            get => code;
            set
            {
                code = value;   
                check = compiler.AddToCompilation(this);
            }
        }*/

        public string Code { get; set; } //TODO Make hidden version for compiled
        //public IReadOnlyDictionary<string, string> CheckArguments { get; }

        internal string GetCodeClassName()
        {
            return string.Concat(Name.Split(' ','-','_','.',',',')','(','%','&','$','#','\\','/','?','!','\'','\"'));
        }
/*

        public void Save()//TODO move it to factory or somethin'
        {
            var markersPath = Settings.CachePath+Settings.MarkersFolder;
            if (!Directory.Exists(markersPath))
                Directory.CreateDirectory(markersPath);

            using (var file = File.Open(markersPath + "/" + GetCodeClassName(), FileMode.OpenOrCreate))
            {
                serializer.Serialize(file,this);
                file.Write(new byte[file.Length-file.Position],0,(int)(file.Length-file.Position));

            } 
        }

        public void Delete()
        {
            compiler.RemoveFromCompilation(this);
            var markerPath = Settings.CachePath + Settings.MarkersFolder + "/" + GetCodeClassName();
            if (File.Exists(markerPath))
                File.Delete(markerPath);
        }
*/
        internal void SetCheck(Func<T, MarkerResult<T>> check)
        {
            this.check = check;
        }
        public MarkerResult<T> Check(T target) => check(target);
        //public async Task<MarkerResult<T>> CheckAsync(T target) => check(target);
    }
}