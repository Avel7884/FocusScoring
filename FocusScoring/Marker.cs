using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FocusApiAccess;

namespace FocusScoring
{
    public class Marker
    {
        private Func<INN, MarkerResult> check;
        private static readonly MarkerRTCompiler compiler = new MarkerRTCompiler();
        
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(Marker),
            new XmlRootAttribute() {ElementName = "items"});

        public static CompilerErrorCollection CheckCodeErrors()
        {
            if(!compiler.IsCompiled)
                compiler.Compile();
            return compiler.Errors;
        }
        
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
            Code = "return true;";
        }
        
        public Marker(string Name, MarkerColour colour, string description, int Score, Func<INN, string> check)
        {
            this.check = x =>
            {
                var res = check.Invoke(x);
                return new MarkerResult(res != null, this, res);
            };
            
            this.Name = Name;
            Colour = colour;
            Description = description;
            code = "Unavalable";
            this.Score = Score;
        }

        public Marker(string Name, MarkerColour colour, string description, int Score, Func<INN, bool> check, string verbose = "")
        {
            this.check = x => new MarkerResult(check.Invoke(x), this, verbose);
            this.Name = Name;
            Colour = colour;
            Description = description;
            code = "Unavalable";
            this.Score = Score;
        }

        public Marker(string Name, MarkerColour colour, string description, int Score, ApiMethodEnum[] methods, string code)
        {   
            this.Name = Name;
            Methods = methods.ToHashSet(); //TODO value check and error
            Colour = colour;
            Description = description;
            this.Score = Score;
            Code = code;
        }
        
        public HashSet<ApiMethodEnum> Methods {get; set;}   
        public string Name { get; set; }
        public MarkerColour Colour { get; set; }
        public string Description { get; set; }

        private string code;

        public string Code
        {
            get => code;
            set
            {
                code = value;   
                check = compiler.PostponededCompile(this);
            }
        }

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

        public MarkerResult Check(INN inn) => check(inn);
        public async Task<MarkerResult> CheckAsync(INN inn) => check(inn);
    }

    public enum MarkerColour
    {
        Green, Yellow, Red, GreenAffiliates, YellowAffiliates, RedAffiliates
    }
}