using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Extensions;
using Colors = System.Drawing.Color;

namespace Tools.OpenXML
{
    class ReportHelper : IDisposable
    {
        private static readonly string _filePath = Path.Combine(FileExtension.FilePath, "Excels");

        private readonly object _data;
        public readonly string ReportFullName;

        private SpreadsheetDocument _document;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public ReportHelper(string fileName, object data)
        {
            ReportFullName = Path.Combine(_filePath, fileName);
            _data = data;
        }

        /// <summary>
        /// 生成报告
        /// </summary>
        public void GenerateReport()
        {
            _document = ExcelExtension.Create(ReportFullName);
            _document.AddWorkbook();

            var worksheet1 = _document.AddWorksheet(new Sheet()
            {
                Name = "TestSheet-1",
                SheetId = 1u
            });
            FillSheet1(worksheet1);
        }

        /// <summary>
        /// 填写工作表1
        /// </summary>
        /// <param name="worksheet"></param>
        private void FillSheet1(Worksheet worksheet)
        {
            //CellReference:用于设置单元格的位置；用于表格数据更新时，公式和图表会自动更新
            var cellA1 = new Cell() { CellReference = "A1", DataType = CellValues.String, CellValue = new CellValue() { Text = "我是A1" } };
            var cellB2 = new Cell() { CellReference = "B2", DataType = CellValues.String, CellValue = new CellValue() { Text = "我是B2" } };
            //RowIndex:指示该行对应于工作表中的哪一行索引
            var row = new Row() { RowIndex = 10u };
            row.Append(cellA1, cellB2);

            var sheetData = new SheetData();
            sheetData.Append(row);
            worksheet.Append(sheetData);

            var fill = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    //填充色
                    ForegroundColor = new ForegroundColor() { Rgb = HexBinaryValue.FromString(Colors.Red.GetRgbString()) },
                    //暂时未知
                    BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString(Colors.Yellow.GetRgbString())}
                }
            };
            var border = new Border()
            {
                LeftBorder = new LeftBorder() { Style = BorderStyleValues.Thick, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Blue.GetRgbString()) } },
                TopBorder = new TopBorder() { Style = BorderStyleValues.Double, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Black.GetRgbString()) } },
                RightBorder = new RightBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Blue.GetRgbString()) } },
                BottomBorder = new BottomBorder() { Style = BorderStyleValues.Hair, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Black.GetRgbString()) } },
            };

            var borders = _document.WorkbookPart.WorkbookStylesPart.Stylesheet.Borders;
            borders.Append(border);
            borders.Count++;

            var fills = _document.WorkbookPart.WorkbookStylesPart.Stylesheet.Fills;
            fills.Append(fill);
            fills.Count++;

            var cellFormats = _document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
            cellFormats.Append(new CellFormat() { BorderId = borders.Count - 1, FillId = fills.Count - 1, ApplyFill = true, ApplyBorder = true });
            cellFormats.Count++;

            cellB2.StyleIndex = cellFormats.Count - 1;
        }

        /// <summary>
        /// 填写工作表2
        /// </summary>
        /// <param name="worksheet"></param>
        private static void FillSheet2(Worksheet worksheet)
        {

        }

        public void Dispose()
        {
            _document.Dispose();
            GC.SuppressFinalize(this);
        }

        ~ReportHelper()
        {
            Dispose();
        }
    }
}
