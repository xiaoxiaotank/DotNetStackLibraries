using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML.Helpers.Reports.ReportA
{
    partial class ReportAHelper
    {
        class Sheet1Helper : SheetHelper<object>
        {
            /// <summary>
            /// 列B的宽度
            /// </summary>
            private const uint ColumnBWidth = 24;

            protected override uint _firstCellIndex => 0;
            protected override uint _lastCellIndex => 3;

            public Sheet1Helper(string sheetName, OpenXMLExcelBase openXMLExcel, object data, uint? sheetId = null) : base(sheetName, openXMLExcel, data, sheetId)
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
                    new Column() { Min = _firstCellIndex + 1, Max = 1, Width = 12, CustomWidth = true },
                    new Column() { Min = 2, Max = _lastCellIndex + 1, Width = ColumnBWidth, CustomWidth = true }
                };
                _writer.InitColumns(columnList);
            }

            protected override void MergeCells()
            {
                var lastRowIndex = 2 + 3 + 1;
                var mergeCellList = new List<MergeCell>()
                {
                    new MergeCell() { Reference = $"A1:D1"},
                    new MergeCell() { Reference = $"A{ lastRowIndex }:D{ lastRowIndex }"}
                };
                _writer.MergeCells(mergeCellList);
            }

            protected override void FillData()
            {
                //S: SheetData
                _writer.WriteStartElement(new SheetData());

                FillHeader();
                FillBody();
                FillFooter();

                //E: SheetData
                _writer.WriteEndElement();
            }

            private void FillHeader()
            {
                #region Styles
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId: _openXMLExcel.GetFontId(16, "宋体", DColor.Black, new Bold(), new Underline() { Val = UnderlineValues.Single }),
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
                var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId); 
                #endregion

                var row = new Row() { RowIndex = _rowIndex++, Height = 44.35, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "      省（区、市） 2017年7 月份重点频段占用度统计表" } });
                SetMergeCellStyle(_firstCellIndex + 1, _lastCellIndex, row.RowIndex, borderCellFormatIndex);
                //E: Row
                _writer.WriteEndElement();
            }

            private void FillBody()
            {
                #region Styles
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var fontId = _openXMLExcel.GetFontId(12, "宋体", DColor.Black);
                var alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center };

                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId,
                    alignment: alignment);
                var cornerCellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId: _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black, true),
                    alignment: new Alignment() { Vertical = VerticalAlignmentValues.Top, WrapText = true });
                var numberingCellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId,
                    numberFormatId: _openXMLExcel.GetNumberingFormatId("0.0%"),
                    alignment: alignment.CloneSafely());
                #endregion

                var row1 = new Row() { RowIndex = _rowIndex++, Height = 27, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row1);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row1.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "序号" } });
                _writer.WriteElement(new Cell() { CellReference = $"B{ row1.RowIndex }", StyleIndex = cornerCellFormatIndex, CellValue = new CellValue() { Text = new string(' ', (int)ColumnBWidth - 10) + "频段(MHz)\n地区" } });
                _writer.WriteElement(new Cell() { CellReference = $"C{ row1.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "223-235" } });
                _writer.WriteElement(new Cell() { CellReference = $"D{ row1.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "351-399.9" } });
                //E: Row
                _writer.WriteEndElement();

                for (int i = 1; i <= 3; i++)
                {
                    var row = new Row() { RowIndex = _rowIndex++, Height = 14.25, CustomHeight = true };
                    //S: Row
                    _writer.WriteStartElement(row);
                    _writer.WriteElement(new Cell() { CellReference = $"A{ row.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = i.ToString() }, DataType = CellValues.Number });
                    _writer.WriteElement(new Cell() { CellReference = $"B{ row.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = $"监测站{i}" } });
                    _writer.WriteElement(new Cell() { CellReference = $"C{ row.RowIndex }", StyleIndex = numberingCellFormatIndex, CellValue = new CellValue() { Text = $"0.1{i}" }, DataType = CellValues.Number });
                    _writer.WriteElement(new Cell() { CellReference = $"D{ row.RowIndex }", StyleIndex = numberingCellFormatIndex, CellValue = new CellValue() { Text = $"0.2{i}" }, DataType = CellValues.Number });
                    //E: Row
                    _writer.WriteEndElement();
                }
            }

            private void FillFooter()
            {
                #region Styles
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId);
                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId: _openXMLExcel.GetFontId(9, "宋体", DColor.Black, new Bold()),
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center }); 
                #endregion

                var row = new Row() { RowIndex = _rowIndex++, Height = 29.55, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row);
                _writer.WriteElement(new Cell() { CellReference = $"A{ row.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "注：直辖市填报所有固定站名称和数据，其它省（区）填报省级站及地市中心站名称和数据。" } });
                SetMergeCellStyle(_firstCellIndex + 1, _lastCellIndex, row.RowIndex, borderCellFormatIndex);
                //E: Row
                _writer.WriteEndElement();
            }

        }
    }

}
