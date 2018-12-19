using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Enums;
using Tools.OpenXML.Models;
using Tools.OpenXML.Tools.OpenXMLExcel;
using C = DocumentFormat.OpenXml.Drawing.Charts;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;


namespace Tools.OpenXML.Helpers.Reports.ReportA
{
    partial class ReportAHelper
    {
        class Sheet3Helper : SheetHelper<IReadOnlyList<Sheet3Data>>
        {
            protected override uint _firstCellIndex => 0;

            protected override uint _lastCellIndex => 2;

            public Sheet3Helper(string sheetName, OpenXMLExcelBase openXMLExcel, IReadOnlyList<Sheet3Data> data, uint? sheetId = null) : base(sheetName, openXMLExcel, data, sheetId)
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

            protected override void FillData()
            {
                //S: SheetData
                _writer.WriteStartElement(new SheetData());

                FillBasicData();

                //E: SheetData
                _writer.WriteEndElement();

            }

            private void FillBasicData()
            {
                var row1 = new Row() { RowIndex = _rowIndex++ };
                //S: Row
                _writer.WriteStartElement(row1);
                uint col = 0;
                foreach (var seriasText in _data.First().DataDic.Keys)
                {
                    _writer.WriteElement(new Cell() { CellReference = GetCellReference(++col, row1.RowIndex), CellValue = new CellValue() { Text = seriasText } });
                }
                //E: Row
                _writer.WriteEndElement();

                for (int i = 0; i < _data.Count; i++)
                {
                    var row = new Row() { RowIndex = _rowIndex++ };
                    //S: Row
                    _writer.WriteStartElement(row);

                    _writer.WriteElement(new Cell() { CellReference = $"A{row.RowIndex}", CellValue = new CellValue() { Text = _data[i].ProductName } });
                    col = 0;
                    foreach (var data in _data[i].DataDic.Values)
                    {
                        _writer.WriteElement(new Cell() { CellReference = GetCellReference(++col, row.RowIndex), CellValue = new CellValue() { Text = data.ToString() }, DataType = CellValues.Number });
                    }

                    //E: Row
                    _writer.WriteEndElement();
                }
            }
        }
    }
}
