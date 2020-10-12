
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FocusAccess;
using OfficeOpenXml;

namespace FocusScoring
{
    public class ExcelMarkerProvider : IMarkersProvider<INN>
    {
        private readonly string excelPath;
        private Marker<INN>[] markers ;

        public ExcelMarkerProvider(string excelPath = ".\\markers.xlsx")
        {
            this.excelPath = excelPath;
        }

        public Marker<INN>[] Markers => markers ??= GetMarkers().ToArray();

        private IEnumerable<Marker<INN>> GetMarkers()
        {
            using ExcelPackage excel = new ExcelPackage();
            excel.Load(File.OpenRead(excelPath));
            var worksheet = excel.Workbook.Worksheets["TDSheet"];
            for (int i = 2; i < 106; i++)
                yield return new Marker<INN>()
                {
                    Name = worksheet.Cells[i, 3].Text,
                    Description = worksheet.Cells[i, 5].Text ?? "",
                    Colour = ParseColour(worksheet.Cells[i, 2].Text, worksheet.Cells[i, 7].Text),
                    Score = int.Parse(worksheet.Cells[i, 8].Text),
                    CheckArguments = new Dictionary<string, string>
                    {
                        {"LibraryCheckMethodName", "Marker" + (i - 1)}
                    }
                };
        }
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
    }
}