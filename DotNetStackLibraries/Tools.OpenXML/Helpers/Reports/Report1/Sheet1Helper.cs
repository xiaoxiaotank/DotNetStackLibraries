using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML.Helpers.Reports.Report1
{
    partial class Report1Helper
    {
        class Sheet1Helper : SheetHelper<object>
        {
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
                    new MergeCell() { Reference = "A1:D1"},
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
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId: _openXMLExcel.GetFontId(16, "宋体", DColor.Black, new Bold(), new Underline() { Val = UnderlineValues.Single }),
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Center, Vertical = VerticalAlignmentValues.Center });
                var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId);

                var row = new Row() { RowIndex = _rowIndex++, Height = 44.35, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row);
                _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex) }{ row.RowIndex }", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "      省（区、市） 2017年7 月份重点频段占用度统计表" }, DataType = CellValues.String });
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{ OpenXMLExcels.GetColumnNameByIndex(i) }{ row.RowIndex }", StyleIndex = borderCellFormatIndex });
                }
                _writer.WriteEndElement();
            }

            private void FillBody()
            {
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
                    alignment: alignment.Clone() as Alignment);

                var row1 = new Row() { RowIndex = _rowIndex++, Height = 27, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row1);

                _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex)}{row1.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "序号" }, DataType = CellValues.String });
                _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 1)}{row1.RowIndex}", StyleIndex = cornerCellFormatIndex, CellValue = new CellValue() { Text = new string(' ', (int)ColumnBWidth - 10) + "频段(MHz)\n地区" }, DataType = CellValues.String });
                _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 2)}{row1.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "223-235" }, DataType = CellValues.String });
                _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 3)}{row1.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "351-399.9" }, DataType = CellValues.String });

                //E: Row
                _writer.WriteEndElement();

                for (int i = 1; i <= 3; i++)
                {
                    var row = new Row() { RowIndex = _rowIndex++, Height = 14.25, CustomHeight = true };
                    //S: Row
                    _writer.WriteStartElement(row);

                    _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex)}{row.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = i.ToString() }, DataType = CellValues.Number });
                    _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 1)}{row.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = $"监测站{i}" }, DataType = CellValues.String });
                    _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 2)}{row.RowIndex}", StyleIndex = numberingCellFormatIndex, CellValue = new CellValue() { Text = $"0.1{i}" }, DataType = CellValues.Number });
                    _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + 3)}{row.RowIndex}", StyleIndex = numberingCellFormatIndex, CellValue = new CellValue() { Text = $"0.2{i}" }, DataType = CellValues.Number });

                    //E: Row
                    _writer.WriteEndElement();
                }
            }
            private void FillFooter()
            {
                var borderId = _openXMLExcel.GetBorderId(BorderStyleValues.Thin, DColor.Black);
                var borderCellFormatIndex = _openXMLExcel.GetCellFormatIndex(borderId);
                var cellFormatIndex = _openXMLExcel.GetCellFormatIndex(
                    borderId,
                    fontId: _openXMLExcel.GetFontId(9, "宋体", DColor.Black, new Bold()),
                    alignment: new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center });

                var row = new Row() { RowIndex = _rowIndex++, Height = 29.55, CustomHeight = true };
                //S: Row
                _writer.WriteStartElement(row);
                _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex)}{row.RowIndex}", StyleIndex = cellFormatIndex, CellValue = new CellValue() { Text = "注：直辖市填报所有固定站名称和数据，其它省（区）填报省级站及地市中心站名称和数据。" }, DataType = CellValues.String });
                for (uint i = _firstCellIndex + 1; i <= _lastCellIndex; i++)
                {
                    _writer.WriteElement(new Cell() { CellReference = $"{OpenXMLExcels.GetColumnNameByIndex(_firstCellIndex + i)}{row.RowIndex}", StyleIndex = borderCellFormatIndex });
                }

                //E: Row
                _writer.WriteEndElement();
            }

        }
    }

}
