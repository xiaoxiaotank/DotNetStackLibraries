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


namespace Tools.OpenXML.Helpers.Reports.Report1
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
            //FillChart();

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


        private void FillChart()
        {
            var axisData = new C.CategoryAxisData()
            {
                StringReference = new C.StringReference() { Formula = new C.Formula($"{_sheetName}!$A$2:$A${_data.Count + 2}") }
            };

            var dataDic = new Dictionary<C.Values, C.SeriesText>();
            for (uint i = 1; i <= _data.First().DataDic.Count; i++)
            {
                var columnName = OpenXMLExcels.GetColumnNameByIndex(i);
                dataDic[new C.Values() { NumberReference = new C.NumberReference() { Formula = new C.Formula($"{_sheetName}!${columnName}$2:${columnName}${_data.Count + 2}") } }]
                    = new C.SeriesText() { StringReference = new C.StringReference() { Formula = new C.Formula($"{_sheetName}!${columnName}$1") } };
            }

            var lineChart = _openXMLExcel.AddLineChart(Chart, axisData, dataDic);
            
            #region 修饰
            lineChart.Append(new C.AxisId() { Val = 0 });
            lineChart.Append(new C.AxisId() { Val = 1 });

            var plotArea = Chart.ChildElements.OfType<C.PlotArea>().FirstOrDefault();
            //x 轴.
            plotArea.Append(new C.CategoryAxis(
                new C.Crosses() { Val = C.CrossesValues.AutoZero },
                new C.AutoLabeled() { Val = true },
                new C.LabelAlignment() { Val = C.LabelAlignmentValues.Center },
                new C.LabelOffset() { Val = 100 })
            {
                AxisId = new C.AxisId() { Val = 0 },
                Scaling = new C.Scaling(new C.Orientation() { Val = C.OrientationValues.MinMax }),
                AxisPosition = new C.AxisPosition() { Val = C.AxisPositionValues.Bottom },
                TickLabelPosition = new C.TickLabelPosition() { Val = C.TickLabelPositionValues.NextTo },
                CrossingAxis = new C.CrossingAxis() { Val = 1 },
            });
            // y轴.
            plotArea.Append(new C.ValueAxis(
                new C.Crosses() { Val = C.CrossesValues.AutoZero },
                new C.CrossBetween() { Val = C.CrossBetweenValues.Between })
            {
                AxisId = new C.AxisId() { Val = 1 },
                Scaling = new C.Scaling(new C.Orientation() { Val = C.OrientationValues.MinMax }),
                AxisPosition = new C.AxisPosition() { Val = C.AxisPositionValues.Left },
                MajorGridlines = new C.MajorGridlines(),
                NumberingFormat = new C.NumberingFormat() { FormatCode = "General", SourceLinked = true },
                TickLabelPosition = new C.TickLabelPosition() { Val = C.TickLabelPositionValues.NextTo },
                CrossingAxis = new C.CrossingAxis() { Val = 0 },
            });
            // 图例.
            Chart.Append(new C.Legend(
                new C.LegendPosition() { Val = C.LegendPositionValues.Right },
                new C.Layout()
            ));
            // 设置只读
            Chart.Append(new C.PlotVisibleOnly() { Val = true });

            var drawingsPart = (Chart.Parent as C.ChartSpace).ChartPart.GetParentParts().First() as DrawingsPart;
            // Position the chart on the worksheet using a TwoCellAnchor object.
            drawingsPart.WorksheetDrawing = new Xdr.WorksheetDrawing();
            var twoCellAnchor = drawingsPart.WorksheetDrawing.AppendChild(new Xdr.TwoCellAnchor()
            {
                FromMarker = new Xdr.FromMarker()
                {
                    ColumnId = new Xdr.ColumnId("5"),
                    ColumnOffset = new Xdr.ColumnOffset("581025"),
                    RowId = new Xdr.RowId("4"),
                    RowOffset = new Xdr.RowOffset("114300")
                },
                ToMarker = new Xdr.ToMarker()
                {
                    ColumnId = new Xdr.ColumnId("13"),
                    ColumnOffset = new Xdr.ColumnOffset("276225"),
                    RowId = new Xdr.RowId("19"),
                    RowOffset = new Xdr.RowOffset("0")
                }
            });

            twoCellAnchor.Append(new Xdr.GraphicFrame()
            {
                NonVisualGraphicFrameProperties = new Xdr.NonVisualGraphicFrameProperties()
                {
                    NonVisualDrawingProperties = new Xdr.NonVisualDrawingProperties() { Id = 2, Name = "Chart 1", Title = "产品每月产量折线图" },
                    NonVisualGraphicFrameDrawingProperties = new Xdr.NonVisualGraphicFrameDrawingProperties()
                },
                Transform = new Xdr.Transform()
                {
                    Offset = new Offset() { X = 0, Y = 0 },
                    Extents = new Extents() { Cx = 0, Cy = 0 }
                },
                Graphic = new Graphic(new GraphicData(new C.ChartReference() { Id = drawingsPart.GetIdOfPart((Chart.Parent as C.ChartSpace).ChartPart) })
                {
                    Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart",
                })
            });

            twoCellAnchor.Append(new Xdr.ClientData());
            #endregion
        }

    }
}
