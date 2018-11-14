using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML.Helpers.Reports.Report1
{
    partial class Report1Helper
    {
        class Sheet2Helper : SheetHelper<object>
        {
            /// <summary>
            /// 开始时钟
            /// </summary>
            private const uint StartClock = 7;
            /// <summary>
            /// 列A的宽度
            /// </summary>
            private const uint ColumnAWidth = 18;

            protected override uint _firstCellIndex => 0;

            protected override uint _lastCellIndex => 24;


            public Sheet2Helper(string sheetName, OpenXMLExcelBase openXMLExcel, object data, uint? sheetId = null) : base(sheetName, openXMLExcel, data, sheetId)
            {
            }

            protected override void InitSheetViews()
            {
                //S: SheetViews
                _writer.WriteStartElement(new SheetViews());
                _writer.WriteElement(new SheetView() { WorkbookViewId = 0 });
                //E: SheetViews
                _writer.WriteEndElement();
            }

            protected override void InitColumns()
            {
                var columnList = new List<Column>()
                {
                    //索引从1开始
                    new Column() { Min = _firstCellIndex + 1, Max = 1, Width = ColumnAWidth, CustomWidth = true },
                    new Column() { Min = 2, Max = _lastCellIndex + 1, Width = 5, CustomWidth = true }
                };

                _writer.InitColumns(columnList);
            }

            protected override void MergeCells()
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

                _writer.MergeCells(mergeCellList);
            }

            protected override void FillData()
            {
                //S: SheetData
                _writer.WriteStartElement(new SheetData());

                FillHeader();
                FillFirstPart();
                FillSecondPart();
                FillFooter();

                //E: SheetData
                _writer.WriteEndElement();
            }

            /// <summary>
            /// 填充头部
            /// </summary>
            /// <param name="writer"></param>
            private void FillHeader()
            {
                #region Styles
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);

                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId: _openXMLExcel.GetFontId(16, "宋体", DColor.DarkRed, new Bold()),
                    fillId: _openXMLExcel.GetFillId(PatternValues.Solid, DColor.FromArgb(255, 255, 153), DColor.FromArgb(255, 255, 153)),
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
                var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId);
                #endregion

                //Height单位：磅
                var row = new Row() { RowIndex = _rowIndex++, Height = 36.95, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row.RowIndex }", StyleIndex = cellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "223-235MHz频段占用度测量表(2017年07月07日)" } });
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row.RowIndex }", StyleIndex = borderCellFormatIndex });
                }
                //E: Row
                _writer.WriteEndElement();
            }

            /// <summary>
            /// 填充第一部分
            /// </summary>
            /// <param name="writer"></param>
            private void FillFirstPart()
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
                var fontId = _openXMLExcel.GetFontId(12, "宋体", DColor.Black);

                var titleCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
                var paramsCellFormatId = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
                #endregion

                #region Title
                var row1 = new Row() { RowIndex = _rowIndex++, Height = 31.8, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row1);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = titleCellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "一、测量系统参数" } });
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row1.RowIndex }", StyleIndex = _openXMLExcel.GetCellFormatIndex(_openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black)) });
                }
                //E: Row
                _writer.WriteEndElement();
                #endregion

                #region Contents
                int pos = 0;
                for (uint i = _rowIndex; i <= _rowIndex + 4; i++)
                {
                    var row = new Row() { RowIndex = i, Height = 25.85, CustomHeight = true };
                    //S: Row
                    _writer.WriteStartElement(row);

                    var cellParam1 = paramsDic.ElementAt(pos++);
                    _writer.WriteElement(new Cell() { CellReference = $"A{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam1.Key.ToString() }, DataType = CellValues.String });
                    _writer.WriteElement(new Cell() { CellReference = $"D{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam1.Value.Key?.ToString() }, DataType = cellParam1.Value.Value });

                    var cellParam2 = paramsDic.ElementAt(pos++);
                    _writer.WriteElement(new Cell() { CellReference = $"I{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam2.Key.ToString() }, DataType = CellValues.String });
                    _writer.WriteElement(new Cell() { CellReference = $"L{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam2.Value.Key?.ToString() }, DataType = cellParam2.Value.Value });

                    var cellParam3 = paramsDic.ElementAt(pos++);
                    _writer.WriteElement(new Cell() { CellReference = $"Q{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam3.Key.ToString() }, DataType = CellValues.String });
                    _writer.WriteElement(new Cell() { CellReference = $"V{ i }", StyleIndex = paramsCellFormatId, CellValue = new CellValue() { Text = cellParam3.Value.Key?.ToString() }, DataType = cellParam3.Value.Value });

                    for (uint j = _firstCellIndex; j <= _lastCellIndex; j++)
                    {
                        _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(j) }{ i }", StyleIndex = paramsCellFormatId });
                    }

                    //E: Row
                    _writer.WriteEndElement();
                }

                _rowIndex += 5;
                #endregion
            }

            /// <summary>
            /// 填充第二部分
            /// </summary>
            /// <param name="writer"></param>
            private void FillSecondPart()
            {
                #region Styles
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var fontId = _openXMLExcel.GetFontId(9, "宋体", DColor.Black);
                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId,
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center, WrapText = true });
                var titleCellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                   borderId,
                   fontId: _openXMLExcel.GetFontId(12, "宋体", DColor.Black),
                   alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
                var cornerCellFormatIndex = _openXMLExcel.GetCellFormatIndex(_openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black, true), fontId, alignment: new Alignment() { Vertical = VerticalAlignmentValues.Top, WrapText = true });
                #endregion

                #region Title
                var row1 = new Row() { RowIndex = _rowIndex++, Height = 31.8, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row1);
                var cell = new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = titleCellFormatIndex, DataType = CellValues.String, CellValue = new CellValue() { Text = "二、日占用度测量" } };
                _writer.WriteElement(cell);
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row1.RowIndex }", StyleIndex = titleCellFormatIndex });
                }
                //E: Row
                _writer.WriteEndElement();
                #endregion

                #region Contents

                #region Header
                var row2 = new Row() { RowIndex = _rowIndex++, Height = 34.75, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row2);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row2.RowIndex }", StyleIndex = cornerCellFormatIndex, CellValue = new CellValue() { Text = new string(' ', (int)ColumnAWidth) + "时间\n\n频率(MHz)" }, DataType = CellValues.String });
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    var clock = (StartClock + i - 1) % 24;
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row2.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = $"{ clock }:00\r\n- \r\n{ clock + 1 }:00" }, DataType = CellValues.String });
                }
                //E: Row
                _writer.WriteEndElement();
                #endregion

                #region Body
                for (uint i = 1; i <= 10 + 2; i++)
                {
                    var row = new Row() { RowIndex = _rowIndex++, Height = 13.5, CustomHeight = true };
                    //S: Row
                    _writer.WriteStartElement(row);
                    for (uint j = _firstCellIndex; j <= _lastCellIndex; j++)
                    {
                        _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(j) }{ row.RowIndex }", StyleIndex = cellFormatIndex });
                    }
                    //E: Row
                    _writer.WriteEndElement();
                }
                #endregion

                #region Footer
                var row3 = new Row() { RowIndex = _rowIndex - 2, Height = 13.5, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row3);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row3.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "各时间段频段占用度(%)" }, DataType = CellValues.String });
                //E: Row
                _writer.WriteEndElement();

                var row4 = new Row() { RowIndex = row3.RowIndex + 1, Height = row3.Height, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row4);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row4.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "日频段占用度(%)" }, DataType = CellValues.String });
                //E: Row
                _writer.WriteEndElement();
                #endregion

                #endregion
            }

            /// <summary>
            /// 填充脚步
            /// </summary>
            /// <param name="writer"></param>
            private void FillFooter()
            {
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var fontId = _openXMLExcel.GetFontId(12, "宋体", DColor.Black, new Bold());
                var infoCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });
                var remarkCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId, fontId, alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });

                var row1 = new Row() { RowIndex = _rowIndex++, Height = 25.85, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row1);
                for (uint i = _firstCellIndex; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ (char)('A' + i) }{ row1.RowIndex }", StyleIndex = infoCellFormatIndex });
                }
                _writer.WriteElement(new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = infoCellFormatIndex, CellValue = new CellValue() { Text = "填表人：" }, DataType = CellValues.String });
                _writer.WriteElement(new Cell() { CellReference = $"L{ row1.RowIndex }", StyleIndex = infoCellFormatIndex, CellValue = new CellValue() { Text = "填表时间：" }, DataType = CellValues.String });
                //E: Row
                _writer.WriteEndElement();

                var row2 = new Row() { RowIndex = _rowIndex++, Height = 29.55, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row2);
                for (uint i = _firstCellIndex; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row2.RowIndex }", StyleIndex = remarkCellFormatIndex });
                }
                _writer.WriteElement(new Cell() { CellReference = $"A{ row2.RowIndex }", StyleIndex = remarkCellFormatIndex, CellValue = new CellValue() { Text = "注：日频段占用度，取各时间频段占用度最大值(要注明时间)。" }, DataType = CellValues.String });
                //E: Row
                _writer.WriteEndElement();
            }
        }
    }
}
