using System.Drawing;
using System.Linq;
using FocusScoring;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FocusScoringGUI
{
    public class ExcelExporter : IExporter
    {
        private readonly ListsCache<string> cache;
        private readonly ListsCache<CompanyData> CompaniesCache;
        private readonly ICompanyFactory CompanyFactory;

        public ExcelExporter(ListsCache<string> settingsCache, ListsCache<CompanyData> companiesCache,
            ICompanyFactory companyFactory)
        {
            cache = settingsCache;
            CompanyFactory = companyFactory;
            CompaniesCache = companiesCache;
        }

        public void Export(string name)
        {
            var fd = new SaveFileDialog();
            fd.Filter = "Excel files (*.xlsx, .xlsm or .xls)|.xlsx;*.xlsm;*.xls;";
            fd.ShowDialog();
            if(fd.FileName == "")
                return;

            using (var file = fd.OpenFile())
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                var settings = cache.GetList(name);
                var companies = CompaniesCache.GetList(name);

                var headerRange = "A"+ 1 + ":" + char.ConvertFromUtf32(settings.Count + 64) + 1;
                worksheet.Cells[headerRange].LoadFromArrays(new []{settings.ToArray()});
                var maxLen = 0;
                for(var i = 0;i<companies.Count;i++)
                {
                    var company = companies[i];
                    if(company.Source==null)
                        company.MakeSource(CompanyFactory);
                    var markers = company.Source.Markers
                        .Where(x => x.Marker.Colour == MarkerColour.Red || x.Marker.Colour == MarkerColour.RedAffiliates )
                        .Concat(company.Source.Markers
                            .Where(x => x.Marker.Colour != MarkerColour.Red && x.Marker.Colour != MarkerColour.RedAffiliates))
                        .ToArray();
                    var companyRow =
                        settings.Select(company.getSetting)
                            .Concat(markers.Select(x=>x.Marker.Description)).ToArray();
                    var companyRange = "A"+ (i+2) + ":" + char.ConvertFromUtf32(companyRow.Length + 64) + (i+2);

                    worksheet.Cells[companyRange].LoadFromArrays(new []{companyRow});
                    //DA Big Clusterfuck
                    worksheet.Cells["A" + (i+2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A" + (i+2)].Style.Fill.BackgroundColor.SetColor(GetExcelColor(company.CLight));
                    worksheet.Cells["A" + (i+2)].AutoFitColumns();
                    var len = company.Source.Markers.Length;
                    
                    
                    for (var j =0;j<len;j++)
                    {
                        var color = GetExcelColor(markers[j].Marker.Colour);
                        var markerPos = char.ConvertFromUtf32(settings.Count + 1 + j + 64) + (i + 2);
                        worksheet.Cells[markerPos].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[markerPos].Style.Fill.BackgroundColor.SetColor(color);
                        worksheet.Cells[markerPos].AutoFitColumns();
                    }

                    if (maxLen < len) maxLen = len;
/*
                    foreach (var VARIABLE in )
                    {
                        
                        worksheet.Cells[markerPos].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }*/
                    //worksheet.Cells["A1:BB"+companies.Count].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                for (var i = 0; i < companies.Count; i++)
                {
                    var markerPos = char.ConvertFromUtf32(65) + (i + 2)+':'+char.ConvertFromUtf32(64+maxLen + settings.Count) + (i + 2);
                    worksheet.Cells[markerPos].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                //var excelFile = new FileInfo(new OpenFileDialog().file);
                excel.SaveAs(file);
            }
        }
            //TODO DRY IT HARD!!
        public void BaseExport(string name)
        {
            var fd = new SaveFileDialog();
            fd.Filter = "Excel files (*.xlsx, .xlsm or .xls)|.xlsx;*.xlsm;*.xls;";
            fd.ShowDialog();
            if(fd.FileName == "")
                return;

            using (var file = fd.OpenFile())
            using (ExcelPackage excel = new ExcelPackage())
            {
                excel.Workbook.Worksheets.Add("Worksheet1");
                var worksheet = excel.Workbook.Worksheets["Worksheet1"];

                var settings = cache.GetList(name);
                var companies = CompaniesCache.GetList(name);

                var headerRange = "A"+ 1 + ":" + char.ConvertFromUtf32(settings.Count + 64) + 1;
                worksheet.Cells[headerRange].LoadFromArrays(new []{settings.ToArray()});
                var maxLen = 0;
                for(var i = 0;i<companies.Count;i++)
                {
                    var company = companies[i];
                    if(company.Source==null)
                        company.MakeSource(CompanyFactory);
                    var companyRow =
                        settings.Select(company.getSetting)
                            .Concat(new []{company.Source.GetParam("Report")}).ToArray();
                    var companyRange = "A"+ (i+2) + ":" + char.ConvertFromUtf32(companyRow.Length + 64) + (i+2);

                    worksheet.Cells[companyRange].LoadFromArrays(new []{companyRow});
                    //DA Big Clusterfuck
                    worksheet.Cells["A" + (i+2)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A" + (i+2)].Style.Fill.BackgroundColor.SetColor(GetExcelColor(company.CLight));
                    worksheet.Cells["A" + (i+2)].AutoFitColumns();
                }

                for (var i = 0; i < companies.Count; i++)
                {
                    var markerPos = char.ConvertFromUtf32(65) + (i + 2)+':'+char.ConvertFromUtf32(64+maxLen + settings.Count+1) + (i + 2);
                    worksheet.Cells[markerPos].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                //var excelFile = new FileInfo(new OpenFileDialog().file);
                excel.SaveAs(file);
            }
        }

        private static Color GetExcelColor(Light light)
        {
            switch (light)
            {
                case Light.Green : return Color.LawnGreen;
                case Light.Red : return Color.Red;
                case Light.Yellow : return Color.Yellow;
                default : return Color.White;
            }
        } 
        private static Color GetExcelColor(MarkerColour light)
        {
            switch (light)
            {
                case MarkerColour.Green : return Color.LawnGreen;
                case MarkerColour.Red : return Color.Red;
                case MarkerColour.Yellow : return Color.Yellow;
                case MarkerColour.GreenAffiliates : return Color.LawnGreen;
                case MarkerColour.RedAffiliates : return Color.Red;
                case MarkerColour.YellowAffiliates : return Color.Yellow;
                default : return Color.White;
            }
        } 
    }
}