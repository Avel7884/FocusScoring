using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using FocusAccess;

namespace FocusScoring
{
    public class XmlMarkerProvider : IMarkersProvider<INN>
    {
        private readonly string xmlPath;
        private Marker<INN>[] markers ;

        public XmlMarkerProvider(string xmlPath = ".\\markers.xml")
        {
            this.xmlPath = xmlPath;
        }

        public Marker<INN>[] Markers => markers ??= GetMarkers().ToArray();

        private IEnumerable<Marker<INN>> GetMarkers()
        {
            var doc = new XmlDocument();
            var nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("a", "urn:schemas-microsoft-com:office:spreadsheet");
            doc.Load(xmlPath);
            var rows = doc
                .ChildNodes[2]
                .ChildNodes[4]
                .ChildNodes[0]
                .ChildNodes;
            for (int i = 0; i < rows.Count; i++)
            {
                Marker<INN> a;
                try
                {
                      a =ParseMarker(rows[i], i);
                }
                catch (Exception e)
                {
                    throw;
                }

                yield return a;
            }
        }

        private Marker<INN> ParseMarker(XmlNode row, int index) =>
            new Marker<INN>
            {
                Name = CellData(row,2),
                Description = CellData(row,4),
                Colour = ParseColour(CellData(row,1),CellData(row,6)),
                Score = int.Parse(CellData(row,7)),
                CheckArguments = new Dictionary<string, string>
                {
                    {"LibraryCheckMethodName","Marker"+(index+1)}
                }
            };

        private MarkerColour ParseColour(string colour, string affiliative) =>
            (colour, affiliative) switch
            {
                ("Красный", "Нет") => MarkerColour.Red,
                ("Желтый", "Нет") => MarkerColour.Yellow,
                ("Зеленый", "Нет") => MarkerColour.Green,
                ("Красный", "Да") => MarkerColour.RedAffiliates,
                ("Желтый", "Да") => MarkerColour.YellowAffiliates,
                ("Зеленый", "Да") => MarkerColour.GreenAffiliates,
                _ => throw new ArgumentException("Incorrect value of colour of affiliativeness")
            };

        private string CellData(XmlNode row, int index) => 
            row.ChildNodes[index].FirstChild.InnerText;
    }
}