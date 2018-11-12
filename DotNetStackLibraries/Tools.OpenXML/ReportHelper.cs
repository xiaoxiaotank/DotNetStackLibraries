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
using DColor = System.Drawing.Color;

namespace Tools.OpenXML
{
    class ReportHelper : IDisposable
    {
        private const uint FirstCellIndex = 0;
        private const uint LastCellIndex = 24;
        private const uint StartClock = 7;
        private const double ColumnAWidth = 18;
        private static readonly string _filePath = Path.Combine(FileExtension.FilePath, "Excels");

        private readonly object _obj = new object();
        private readonly object _data;
        private readonly OpenXMLExcelBase _openXMLExcel;
        public readonly string ReportFullName;

        private uint _rowIndex = 1;

        /// <summary>
        /// 构造
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
            FillSheet1ByMemory(worksheet1);
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
            FillSheet2ByWriter(worksheetPart);
        }

        public void Dispose()
        {
            if (_openXMLExcel.Document != null)
            {
                _openXMLExcel.Document.Dispose();
            }
            GC.SuppressFinalize(this);
        }


        #region Privites
        /// <summary>
        /// 操作内存填写工作表1
        /// </summary>
        /// <param name="worksheet"></param>
        private void FillSheet1ByMemory(Worksheet worksheet)
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
                    ForegroundColor = new ForegroundColor() { Rgb = HexBinaryValue.FromString(DColor.Red.GetRgbString()) },
                    //暂时未知
                    BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString(DColor.Yellow.GetRgbString()) }
                }
            };
            var border = new Border()
            {
                LeftBorder = new LeftBorder() { Style = BorderStyleValues.Thick, Color = new Color() { Rgb = HexBinaryValue.FromString(DColor.Blue.GetRgbString()) } },
                TopBorder = new TopBorder() { Style = BorderStyleValues.Double, Color = new Color() { Rgb = HexBinaryValue.FromString(DColor.Black.GetRgbString()) } },
                RightBorder = new RightBorder() { Style = BorderStyleValues.Thin, Color = new Color() { Rgb = HexBinaryValue.FromString(DColor.Blue.GetRgbString()) } },
                BottomBorder = new BottomBorder() { Style = BorderStyleValues.Hair, Color = new Color() { Rgb = HexBinaryValue.FromString(DColor.Black.GetRgbString()) } },
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
        /// 操作硬盘填写工作表2
        /// </summary>
        /// <param name="worksheet"></param>
        private void FillSheet2ByWriter(WorksheetPart worksheetPart)
        {
            using (var writer = OpenXmlWriter.Create(worksheetPart))
            {
                //S: Worksheet
                writer.WriteStartElement(new Worksheet());

                InitSheetViews(writer);
                InitColumns(writer);
                MergeCells(writer);
                FillData(writer);

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
                    if (_openXMLExcel.Document == null)
                    {
                        _openXMLExcel.CreateDocument(ReportFullName);
                        _openXMLExcel.AddWorkbook();
                    }
                }
            }
        }

        /// <summary>
        /// 初始化表格视图
        /// </summary>
        /// <param name="writer"></param>
        private static void InitSheetViews(OpenXmlWriter writer)
        {
            //S: SheetViews
            writer.WriteStartElement(new SheetViews());
            writer.WriteElement(new SheetView() { WorkbookViewId = 0 });
            //E: SheetViews
            writer.WriteEndElement();
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        /// <param name="writer"></param>
        private static void InitColumns(OpenXmlWriter writer)
        {
            var columnList = new List<Column>()
            {
                //索引从1开始
                new Column() { Min = FirstCellIndex + 1, Max = 1, Width = ColumnAWidth, CustomWidth = true },
                new Column() { Min = 2, Max = LastCellIndex + 1, Width = 5, CustomWidth = true }
            };

            writer.InitColumns(columnList);
        }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="writer"></param>
        private static void MergeCells(OpenXmlWriter writer)
        {
            var occupyLineIndex = 9 + 10 + 2;
            var mergeCellList = new List<MergeCell>()
            {
                new MergeCell(){ Reference = "A1:Y1" },
                new MergeCell(){ Reference = "A2:Y2" },
                new MergeCell(){ Reference = "A8:Y8" },
                new MergeCell(){ Reference = $"B{ occupyLineIndex }: Y{ occupyLineIndex }" },
                new MergeCell(){ Reference = $"A{ occupyLineIndex + 1 }: K{ occupyLineIndex + 1 }" },
                new MergeCell(){ Reference = $"L{ occupyLineIndex + 1 }: Y{ occupyLineIndex + 1 }" },
                new MergeCell(){ Reference = $"A{ occupyLineIndex + 2 }: Y{ occupyLineIndex + 2 }" }
            };
            for (int i = 3; i <= 7; i++)
            {
                mergeCellList.AddRange(new List<MergeCell>()
                {
                    new MergeCell() { Reference = $"A{i}:C{i}" },
                    new MergeCell() { Reference = $"D{i}:H{i}" },
                    new MergeCell() { Reference = $"I{i}:K{i}" },
                    new MergeCell() { Reference = $"L{i}:P{i}" },
                    new MergeCell() { Reference = $"Q{i}:U{i}" },
                    new MergeCell() { Reference = $"V{i}:Y{i}" },
                });
            }

            writer.MergeCells(mergeCellList);
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="writer"></param>
        private void FillData(OpenXmlWriter writer)
        {
            //S: SheetData
            writer.WriteStartElement(new SheetData());

            FillHeader(writer);
            FillFirstPart(writer);
            FillSecondPart(writer);
            FillFooter(writer);

            //E: SheetData
            writer.WriteEndElement();
        }

        /// <summary>
        /// 填充头部
        /// </summary>
        /// <param name="writer"></param>
        private void FillHeader(OpenXmlWriter writer)
        {
            #region Styles
            var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);

            var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                borderId, 
                fontId: _openXMLExcel.GetFontId(16, "宋体", new Bold(), DColor.DarkRed), 
                fillId: _openXMLExcel.GetFillId(PatternValues.Solid, DColor.FromArgb(255, 255, 153), DColor.FromArgb(255, 255, 153)), 
                alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
            var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId);
            #endregion

            //Height单位：磅
            var row = new Row() { RowIndex = _rowIndex++, Height = 36.95, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row);
            writer.WriteElement(new Cell() { CellReference = $"A{ row.RowIndex }", StyleIndex = cellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "223-235MHz频段占用度测量表(2017年07月07日)" } });
            for (uint i = FirstCellIndex + 1; i <= LastCellIndex; i++)
            {
                writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row.RowIndex }", StyleIndex = borderCellFormatIndex });
            }
            //E: Row
            writer.WriteEndElement();
        }

        /// <summary>
        /// 填充第一部分
        /// </summary>
        /// <param name="writer"></param>
        private void FillFirstPart(OpenXmlWriter writer)
        {
            var paramsDic = new Dictionary<string, KeyValuePair<object, CellValues>>()
            {
                ["监测站名称"] = new KeyValuePair<object, CellValues>("东城花园监测站", CellValues.String),
                ["监测站经度"] = new KeyValuePair<object, CellValues>(76.027214050293, CellValues.Number),
                ["监测站纬度"] = new KeyValuePair<object, CellValues>(39.4658088684082, CellValues.Number),
                ["接收机(频谱仪)型号"] = new KeyValuePair<object, CellValues>(string.Empty, CellValues.String),
                ["天线类型"] = new KeyValuePair<object, CellValues>(string.Empty, CellValues.String),
                ["天线挂高"] = new KeyValuePair<object, CellValues>(null, CellValues.Number),
                ["起始频率(MHz)"] = new KeyValuePair<object, CellValues>(223, CellValues.Number),
                ["终止频率(MHz)"] = new KeyValuePair<object, CellValues>(235, CellValues.Number),
                ["检波方式"] = new KeyValuePair<object, CellValues>("均值", CellValues.String),
                ["中频带宽(RBW，kHz)"] = new KeyValuePair<object, CellValues>(39.0625, CellValues.Number),
                ["门限电平"] = new KeyValuePair<object, CellValues>("自适应门限", CellValues.String),
                ["测量周期(秒)"] = new KeyValuePair<object, CellValues>(null, CellValues.Number),
                ["测量分辨率(分钟)"] = new KeyValuePair<object, CellValues>(null, CellValues.Number),
                ["测量开始时间"] = new KeyValuePair<object, CellValues>(DateTime.Now, CellValues.String),
                ["测量结束时间"] = new KeyValuePair<object, CellValues>(DateTime.Now, CellValues.String)
            };

            #region Styles
            var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
            var fontId = _openXMLExcel.GetFontId(12, "宋体", null, DColor.Black);

            var titleCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
            var paramsCellFormatId = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
            #endregion

            #region Title
            var row1 = new Row() { RowIndex = _rowIndex++, Height = 31.8, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row1);
            writer.WriteElement(new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = titleCellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "一、测量系统参数" } });
            for (uint i = FirstCellIndex + 1; i <= LastCellIndex; i++)
            {
                writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row1.RowIndex }", StyleIndex = _openXMLExcel.GetCellFormatIndex(_openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black)) });
            }
            //E: Row
            writer.WriteEndElement();
            #endregion

            #region Contents
            int pos = 0;
            for (uint i = _rowIndex; i <= 7; i++)
            {
                var row = new Row() { RowIndex = i, Height = 25.85, CustomHeight = true };
                //S: Row
                writer.WriteStartElement(row);

                var cellParam1 = paramsDic.ElementAt(pos++);
                writer.WriteElement(new Cell() { CellReference = $"A{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam1.Key.ToString() }, DataType = CellValues.String });
                writer.WriteElement(new Cell() { CellReference = $"D{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam1.Value.Key?.ToString() }, DataType = cellParam1.Value.Value });

                var cellParam2 = paramsDic.ElementAt(pos++);
                writer.WriteElement(new Cell() { CellReference = $"I{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam2.Key.ToString() }, DataType = CellValues.String });
                writer.WriteElement(new Cell() { CellReference = $"L{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam2.Value.Key?.ToString() }, DataType = cellParam2.Value.Value });

                var cellParam3 = paramsDic.ElementAt(pos++);
                writer.WriteElement(new Cell() { CellReference = $"Q{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam3.Key.ToString() }, DataType = CellValues.String });
                writer.WriteElement(new Cell() { CellReference = $"V{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam3.Value.Key?.ToString() }, DataType = cellParam3.Value.Value });

                for (uint j = FirstCellIndex; j <= LastCellIndex; j++)
                {
                    writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(j) }{ i }", StyleIndex = paramsCellFormatId });
                }

                //E: Row
                writer.WriteEndElement();
            }

            _rowIndex = 8;
            #endregion
        }

        /// <summary>
        /// 填充第二部分
        /// </summary>
        /// <param name="writer"></param>
        private void FillSecondPart(OpenXmlWriter writer)
        {
            #region Styles
            var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
            var fontId = _openXMLExcel.GetFontId(9, "宋体", null, DColor.Black);
            var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                borderId, 
                fontId, 
                alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true });
            var titleCellFormatIndex = _openXMLExcel.GetCellFormatIndex(
               borderId,
               fontId: _openXMLExcel.GetFontId(12, "宋体", null, DColor.Black),
               alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
            var cornerCellFormatIndex = _openXMLExcel.GetCellFormatIndex(_openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black, true), fontId, alignment: new Alignment() { Vertical = VerticalAlignmentValues.Top, WrapText = true });
            #endregion

            #region Title
            var row1 = new Row() { RowIndex = _rowIndex++, Height = 31.8, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row1);
            var cell = new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = titleCellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "二、日占用度测量" } };
            writer.WriteElement(cell);
            for (uint i = FirstCellIndex + 1; i <= LastCellIndex; i++)
            {
                writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row1.RowIndex }", StyleIndex = titleCellFormatIndex });
            }
            //E: Row
            writer.WriteEndElement();
            #endregion

            #region Contents

            #region Header
            var row2 = new Row() { RowIndex = _rowIndex++, Height = 34.75, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row2);
            writer.WriteElement(new Cell() { CellReference = $"A{ row2.RowIndex }", StyleIndex = cornerCellFormatIndex, CellValue = new CellValue() { Text = new string(' ', (int)ColumnAWidth) + "时间\n\n 频率(MHz)" }, DataType = CellValues.String });
            for (uint i = FirstCellIndex + 1; i <= LastCellIndex; i++)
            {
                var clock = (StartClock + i - 1) % 24;
                writer.WriteElement(new Cell() { CellReference = $"{ (char)('A' + i) }{ row2.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = $"{ clock }:00\r\n - \r\n{ clock + 1 }:00" }, DataType = CellValues.String });
            }
            //E: Row
            writer.WriteEndElement();
            #endregion

            #region Body
            for (uint i = 1; i <= 10 + 2; i++)
            {
                var row = new Row() { RowIndex = _rowIndex++, Height = 13.5, CustomHeight = true };
                //S: Row
                writer.WriteStartElement(row);
                for (uint j = FirstCellIndex; j <= LastCellIndex; j++)
                {
                    writer.WriteElement(new Cell() { CellReference = $"{ (char)('A' + j) }{ row.RowIndex }", StyleIndex = cellFormatIndex });
                }
                //E: Row
                writer.WriteEndElement();
            }
            #endregion

            #region Footer
            var row3 = new Row() { RowIndex = _rowIndex - 2, Height = 13.5, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row3);
            writer.WriteElement(new Cell() { CellReference = $"A{ row3.RowIndex }", StyleIndex = cellFormatIndex,  CellValue = new CellValue() { Text = "各时间段频段占用度(%)" }, DataType = CellValues.String });
            //E: Row
            writer.WriteEndElement();

            var row4 = new Row() { RowIndex = row3.RowIndex + 1, Height = row3.Height, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row4);
            writer.WriteElement(new Cell() { CellReference = $"A{ row4.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "日频段占用度(%)" }, DataType = CellValues.String });
            //E: Row
            writer.WriteEndElement();
            #endregion

            #endregion
        }

        /// <summary>
        /// 填充脚步
        /// </summary>
        /// <param name="writer"></param>
        private void FillFooter(OpenXmlWriter writer)
        {
            var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
            var fontId = _openXMLExcel.GetFontId(12, "宋体", new Bold(), DColor.Black);
            var infoCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
            var remarkCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });

            var row1 = new Row() { RowIndex = _rowIndex++, Height = 25.85, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row1);
            for (uint i = FirstCellIndex; i <= LastCellIndex; i++)
            {
                writer.WriteElement(new Cell() { CellReference = $"{ (char)('A' + i) }{ row1.RowIndex }", StyleIndex = infoCellFormatIndex });
            }
            writer.WriteElement(new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = infoCellFormatIndex, CellValue = new CellValue() { Text = "填表人：" }, DataType = CellValues.String });
            writer.WriteElement(new Cell() { CellReference = $"L{ row1.RowIndex }", StyleIndex = infoCellFormatIndex, CellValue = new CellValue() { Text = "填表时间：" }, DataType = CellValues.String });
            //E: Row
            writer.WriteEndElement();

            var row2 = new Row() { RowIndex = _rowIndex++, Height = 29.55, CustomHeight = true };
            //S: Row
            writer.WriteStartElement(row2);
            for (uint i = FirstCellIndex; i <= LastCellIndex; i++)
            {
                writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row2.RowIndex }", StyleIndex = remarkCellFormatIndex });
            }
            writer.WriteElement(new Cell() { CellReference = $"A{ row2.RowIndex }", StyleIndex = remarkCellFormatIndex, CellValue = new CellValue() { Text = "注：日频段占用度，取各时间频段占用度最大值(要注明时间)。" }, DataType = CellValues.String });
            //E: Row
            writer.WriteEndElement();
        }

        ~ReportHelper()
        {
            Dispose();
        } 
        #endregion
    }
}
