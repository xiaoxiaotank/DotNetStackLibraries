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
using Tools.OpenXML.Tools.OpenXMLExcel;
using Colors = System.Drawing.Color;

namespace Tools.OpenXML
{
    class ReportHelper : IDisposable
    {
        private static readonly string _filePath = Path.Combine(FileExtension.FilePath, "Excels");

        private readonly object _obj = new object();
        private readonly object _data;
        private readonly OpenXMLExcelBase _openXMLExcel;
        public readonly string ReportFullName;

        /// <summary>
        /// key:key
        /// value:样式下标
        /// </summary>
        private Lazy<Dictionary<string, uint>> _styleIdDic = new Lazy<Dictionary<string, uint>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public ReportHelper(string fileName, object data)
        {
            ReportFullName = Path.Combine(_filePath, fileName);
            _openXMLExcel = new OpenXMLExcel();
            _data = data;
        }

        /// <summary>
        /// 通过操作内存生成报告
        /// </summary>
        public void GenerateReportByMemory()
        {
            InitDocument();

            var worksheet1 = _openXMLExcel.AddWorksheet(new Sheet()
            {
                Name = "TestSheet-1",
                SheetId = 1
            });
            FillSheet1(worksheet1);
        }

        /// <summary>
        /// 通过操作硬盘生成报告
        /// </summary>
        public void GenerateReportByWriter()
        {
            InitDocument();

            var worksheetPart = _openXMLExcel.Document.WorkbookPart.AddNewPart<WorksheetPart>();
            _openXMLExcel.AddSheetToPart(new Sheet()
            {
                Name = "TestSheet-2",
                SheetId = 2
            }, worksheetPart);
            FillSheet2(worksheetPart);
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

            var sheetData = worksheet.FirstChild;
            sheetData.Append(row);

            #region Styles
            var fill = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = PatternValues.Solid,
                    //填充色
                    ForegroundColor = new ForegroundColor() { Rgb = HexBinaryValue.FromString(Colors.Red.GetRgbString()) },
                    //暂时未知
                    BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString(Colors.Yellow.GetRgbString()) }
                }
            };
            var border = new Border()
            {
                LeftBorder = new LeftBorder() { Style = BorderStyleValues.Thick, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Blue.GetRgbString()) } },
                TopBorder = new TopBorder() { Style = BorderStyleValues.Double, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Black.GetRgbString()) } },
                RightBorder = new RightBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Blue.GetRgbString()) } },
                BottomBorder = new BottomBorder() { Style = BorderStyleValues.Hair, Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Black.GetRgbString()) } },
            };

            var borderId = _openXMLExcel.AddBorders(border);


            var fills = _openXMLExcel.Document.WorkbookPart.WorkbookStylesPart.Stylesheet.Fills;
            fills.Append(fill);
            fills.Count++;

            var cellFormats = _openXMLExcel.Document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
            cellFormats.Append(new CellFormat() { BorderId = borderId, FillId = fills.Count - 1, ApplyFill = true, ApplyBorder = true });
            cellFormats.Count++;

            cellB2.StyleIndex = cellFormats.Count - 1; 
            #endregion
        }

        /// <summary>
        /// 填写工作表2
        /// </summary>
        /// <param name="worksheet"></param>
        private void FillSheet2(WorksheetPart worksheetPart)
        {
            using(var writer = OpenXmlWriter.Create(worksheetPart))
            {
                //S: Worksheet
                writer.WriteStartElement(new Worksheet());

                #region Init
                //S: SheetViews
                writer.WriteStartElement(new SheetViews());
                writer.WriteElement(new SheetView()
                {
                    WorkbookViewId = 0
                });
                //E: SheetViews
                writer.WriteEndElement(); 
                #endregion

                #region Header

                #region 合并单元格
                //S: MergeCell
                var mergeCell = new MergeCell() { Reference = "A1:Y1" };
                writer.WriteStartElement(new MergeCells());
                writer.WriteElement(mergeCell);
                //E: MergeCells
                writer.WriteEndElement();
                #endregion

                #region 设置列
                //S: Columns
                writer.WriteStartElement(new Columns());
                var column1 = new Column() { Min = 1, Max = 1, Width = 20, CustomWidth = true };
                var column2_25 = new Column() { Min = 2, Max = 25, Width = 6, CustomWidth = true };
                writer.WriteElement(column1);
                writer.WriteElement(column2_25);
                //E: Columns
                writer.WriteEndElement();
                #endregion

                #region 填充数据
                #region Styles

                var font = new Font()
                {
                    FontSize = new FontSize() { Val = 16 },
                    FontName = new FontName() { Val = "宋体" },
                    Bold = new Bold(),
                    Color = new Color() { Rgb = HexBinaryValue.FromString(Colors.Red.GetRgbString()) },
                    VerticalTextAlignment = new VerticalTextAlignment() { Val = VerticalAlignmentRunValues.Baseline }
                };
                var fontId = _openXMLExcel.AddFonts(font);

                var pattern = PatternValues.Solid;
                var foreDColor = Colors.Yellow;
                var backDColor = Colors.Yellow;
                var fillKey = $"{pattern}.{foreDColor}.{backDColor}";
                var fillId = GetStyleId(fillKey);
                if (!fillId.HasValue)
                {
                    var fill = OpenXMLExcels.GetFill(pattern, foreDColor, backDColor);
                    fillId = _styleIdDic.Value[fillKey] = _openXMLExcel.AddFills(fill);
                }

                var dColor = Colors.Black;
                var borderKey = $"{BorderStyleValues.Thin}.{dColor}";
                var borderId = GetStyleId(borderKey);
                if (!borderId.HasValue)
                {
                    var border = OpenXMLExcels.GetBorder(BorderStyleValues.Thin, dColor);
                    borderId = _styleIdDic.Value[borderKey] = _openXMLExcel.AddBorders(border);
                }

                var alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center };
                var cellFormatId = _openXMLExcel.AddCellFormats(new CellFormat() { FontId = fontId, FillId = fillId, BorderId = borderId, Alignment = alignment, ApplyFont = true, ApplyFill = true, ApplyBorder = true, ApplyAlignment = true },
                    new CellFormat() { BorderId = borderId, ApplyBorder = true });

                #endregion
                //S: SheetData
                writer.WriteStartElement(new SheetData());
                //Height单位：磅
                var row = new Row() { RowIndex = 1, Height = 40, CustomHeight = true };
                //S: Row
                writer.WriteStartElement(row);
                var cell = new Cell() { CellReference = "A1", StyleIndex = cellFormatId - 1, DataType = CellValues.String, CellValue = new CellValue() { Text = "我 是 Header" } };
                writer.WriteElement(cell);
                for (int i = 1; i < 25; i++)
                {
                    writer.WriteElement(new Cell() { CellReference = $"{ (char)('A' + i) }1", StyleIndex = cellFormatId });
                }
                //E: Row
                writer.WriteEndElement();
                //E: SheetData
                writer.WriteEndElement(); 
                #endregion

                #endregion


                //E: Worksheet
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// 初始化文档
        /// </summary>
        private void InitDocument()
        {
            if (_openXMLExcel.Document == null)
            {
                lock (_obj)
                {
                    if(_openXMLExcel.Document == null)
                    {
                        _openXMLExcel.CreateDocument(ReportFullName);
                        _openXMLExcel.AddWorkbook();
                    }
                }
            }
        }

        /// <summary>
        /// 获取样式Id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private uint? GetStyleId(string key)
        {
            return _styleIdDic.Value.ContainsKey(key) ? _styleIdDic.Value[key] : default(uint?);
        }

        public void Dispose()
        {
            if(_openXMLExcel.Document != null)
            {
                _openXMLExcel.Document.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        ~ReportHelper()
        {
            Dispose();
        }
    }
}
