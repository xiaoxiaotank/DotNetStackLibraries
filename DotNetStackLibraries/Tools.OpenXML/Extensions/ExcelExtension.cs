using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Enums;

namespace Tools.OpenXML
{
    static class ExcelExtension
    {
        /// <summary>
        /// 创建Excel文档
        /// </summary>
        /// <param name="fileFullName"></param>
        public static SpreadsheetDocument Create(string fileFullName)
        {
            var directory = Path.GetDirectoryName(fileFullName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return SpreadsheetDocument.Create(fileFullName, SpreadsheetDocumentType.Workbook);
        }

        /// <summary>
        /// 添加工作簿
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static void AddWorkbook(this SpreadsheetDocument document)
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook()
            {
                Sheets = new Sheets()
            };

            workbookPart.AddNewPart<WorkbookStylesPart>();
            
            workbookPart.WorkbookStylesPart.Stylesheet = GetDefaultStylesheet();
        }

        /// <summary>
        /// 获取默认样式表
        /// </summary>
        /// <param name="stylesPart"></param>
        private static Stylesheet GetDefaultStylesheet()
        {
            var fonts = new Fonts() { Count = 1 };
            fonts.Append(new Font() { FontSize = new FontSize() { Val = 11 }, FontName = new FontName() { Val = "宋体" } });

            var borders = new Borders() { Count = 1 };
            borders.Append(new Border() { LeftBorder = new LeftBorder(), TopBorder = new TopBorder(), RightBorder = new RightBorder(), BottomBorder = new BottomBorder() });

            var fills = new Fills() { Count = 2 };
            fills.Append(new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.None } }, new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 } });

            var cellFormat = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0, Alignment = new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center } };
            var cellStyleFormats = new CellStyleFormats() { Count = 1 };
            cellStyleFormats.Append(cellFormat);
            var cellFormats = new CellFormats() { Count = 1 };
            cellFormats.Append(cellFormat.Clone() as OpenXmlElement);

            var stylesheet = new Stylesheet()
            {
                Fonts = fonts,
                Borders = borders,
                Fills = fills,
                CellFormats = cellFormats,
                CellStyleFormats = cellStyleFormats
            };
            return stylesheet;
        }

        /// <summary>
        /// 添加工作表
        /// </summary>
        /// <param name="document"></param>
        /// <param name="sheet">表示工作表的属性</param>
        /// <returns></returns>
        public static Worksheet AddWorksheet(this SpreadsheetDocument document, Sheet sheet)
        {
            var worksheet = new Worksheet();
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var worksheetPart = document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = worksheet;

            sheet.Id = document.WorkbookPart.GetIdOfPart(worksheetPart);
            document.WorkbookPart.Workbook.Sheets.Append(sheet);

            return worksheet;
        }


        /// <summary>
        /// 获取WorkbookPart
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static WorkbookPart GetWorkbookPart(this Worksheet worksheet)
        {
            return worksheet.WorksheetPart.GetParentParts().First() as WorkbookPart;
        }

    }
}
