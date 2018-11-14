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
using DColor = System.Drawing.Color;

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
            else if (Files.IsBusy(fileFullName))
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

        public override Worksheet AddWorksheet(string sheetName, uint? sheetId = null)
        {
            var worksheet = new Worksheet(new SheetData());
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var worksheetPart = Document.WorkbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = worksheet;

            AddSheetToPart(worksheetPart, sheetName, sheetId);
            return worksheet;
        }

        public override void AddSheetToPart(WorksheetPart worksheetPart, string sheetName, uint? sheetId = null)
        {
            var sheets = Document.WorkbookPart.Workbook.Sheets;
            var sheet = new Sheet()
            {
                Name = sheetName,
                SheetId = sheetId ?? (uint)(sheets.Count() + 1),
                Id = Document.WorkbookPart.GetIdOfPart(worksheetPart)
            };
            
            var refChild = sheets.FirstOrDefault(s => (s as Sheet).SheetId > sheet.SheetId);
            if(refChild != null)
            {
                sheets.InsertBefore(sheet, refChild);
            }
            else
            {
                sheets.Append(sheet);
            }
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

        public override uint AddNumberingFormats(params NumberingFormat[] numberingFormats)
        {
            var styleNumberingFormats = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats;
            for (int i = 0; i < numberingFormats.Length; i++)
            {
                numberingFormats[i].NumberFormatId = (uint)(styleNumberingFormats.Count + i);
            }

            styleNumberingFormats.Append(numberingFormats);
            styleNumberingFormats.Count += (uint)numberingFormats.Length;

            return styleNumberingFormats.Count - 1;
        }

        public override uint AddCellFormats(params CellFormat[] cellFormats)
        {
            var styleCellFormats = Document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
            styleCellFormats.Append(cellFormats);
            styleCellFormats.Count += (uint)cellFormats.Length;

            return styleCellFormats.Count - 1;
        }

        public override uint GetFontId(double fontSize, string fontName, DColor dColor, Bold bold = null, Underline underline = null)
        {
            var fontKey = OpenXMLExcels.GetFontStyleKey(fontSize, fontName, dColor, bold, underline);
            var fontId = GetStyleId(fontKey);
            if (!fontId.HasValue)
            {
                var font = OpenXMLExcels.GetFont(fontSize, fontName, dColor, bold, underline);
                fontId = _styleIdDic.Value[fontKey] = AddFonts(font);
            }

            return fontId.Value;
        }

        public override uint GetBorderId(BorderStyleValues style, DColor dColor, bool includeDiagonal = false, bool isDiagonalDown = true)
        {
            var borderKey = OpenXMLExcels.GetBorderStyleKey(style, dColor, includeDiagonal, isDiagonalDown);
            var borderId = GetStyleId(borderKey);
            if (!borderId.HasValue)
            {
                var border = OpenXMLExcels.GetBorder(style, dColor, includeDiagonal, isDiagonalDown);
                borderId = _styleIdDic.Value[borderKey] = AddBorders(border);
            }

            return borderId.Value;
        }

        public override uint GetFillId(PatternValues pattern, DColor foreDColor, DColor backDColor)
        {
            var fillKey = OpenXMLExcels.GetFillStyleKey(pattern, foreDColor, backDColor);
            var fillId = GetStyleId(fillKey);
            if (!fillId.HasValue)
            {
                var fill = OpenXMLExcels.GetFill(pattern, foreDColor, backDColor);
                fillId = _styleIdDic.Value[fillKey] = AddFills(fill);
            }

            return fillId.Value;
        }

        public override uint GetNumberingFormatId(string formatCode)
        {
            var key = OpenXMLExcels.GetNumberingFormatKey(formatCode);
            var id = GetStyleId(key);
            if (!id.HasValue)
            {
                var numberingFormat = OpenXMLExcels.GetNumberingFormat(formatCode);
                id = _styleIdDic.Value[key] = AddNumberingFormats(numberingFormat);
            }

            return id.Value;
        }

        public override uint GetCellFormatIndex(uint? borderId = null, uint? fontId = null, uint? fillId = null, uint? formatId = null, uint? numberFormatId = null, Alignment alignment = null)
        {
            var cellFormatKey = OpenXMLExcels.GetCellFormatStyleKey(borderId, fontId, fillId, formatId, numberFormatId, alignment);
            var cellFormatId = GetStyleId(cellFormatKey);
            if (!cellFormatId.HasValue)
            {
                var cellFormat = OpenXMLExcels.GetCellFormat(borderId, fontId, fillId, formatId, numberFormatId, alignment);
                cellFormatId = _styleIdDic.Value[cellFormatKey] = AddCellFormats(cellFormat);
            }

            return cellFormatId.Value;
        }

    }
}
