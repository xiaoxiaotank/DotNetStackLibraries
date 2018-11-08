using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Tools.OpenXML.Enums;

namespace Tools.OpenXML.Tools.OpenXMLExcel
{
    class OpenXMLExcel : OpenXMLExcelBase
    {
        public override SpreadsheetDocument CreateDocument(string fileFullName)
        {
            var directory = Path.GetDirectoryName(fileFullName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            else if (fileFullName.IsBusy())
            {
                throw new IOException("文件被占用");
            }

            Document = SpreadsheetDocument.Create(fileFullName, SpreadsheetDocumentType.Workbook);
            return Document;
        }

        public override void AddWorkbook()
        {
            //添加工作簿区域，除了包含工作簿外，还有其他属性
            var workbookPart = Document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook()
            {
                Sheets = new Sheets()
            };

            workbookPart.AddNewPart<WorkbookStylesPart>();
            workbookPart.WorkbookStylesPart.Stylesheet = GetDefaultStylesheet();
        }

        public override Worksheet AddWorksheet(Sheet sheet)
        {
            var worksheet = new Worksheet(new SheetData());
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var worksheetPart = Document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = worksheet;

            AddSheetToPart(sheet, worksheetPart);
            return worksheet;
        }

        public override void AddSheetToPart(Sheet sheet, WorksheetPart worksheetPart)
        {
            sheet.Id = Document.WorkbookPart.GetIdOfPart(worksheetPart);
            Document.WorkbookPart.Workbook.Sheets.Append(sheet);
        }

        public override uint AddFonts(params Font[] fonts)
        {
            var styleFonts = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.Fonts;
            styleFonts.Append(fonts);
            styleFonts.Count += (uint)fonts.Length;

            return styleFonts.Count - 1;
        }

        public override uint AddBorders(params Border[] borders)
        {
            var styleBorders = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.Borders;
            styleBorders.Append(borders);
            styleBorders.Count += (uint)borders.Length;

            return styleBorders.Count - 1;
        }


        public override uint AddFills(params Fill[] fills)
        {
            var styleFills = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.Fills;
            styleFills.Append(fills);
            styleFills.Count += (uint)fills.Length;

            return styleFills.Count - 1;
        }

        public override uint AddCellFormats(params CellFormat[] cellFormats)
        {
            var styleCellFormats = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
            styleCellFormats.Append(cellFormats);
            styleCellFormats.Count += (uint)cellFormats.Length;

            return styleCellFormats.Count - 1;
        }
    }
}
