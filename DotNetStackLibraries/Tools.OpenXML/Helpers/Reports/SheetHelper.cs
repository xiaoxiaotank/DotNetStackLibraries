using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Enums;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;
using C = DocumentFormat.OpenXml.Drawing.Charts;
using Xdr = DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Tools.OpenXML.Models;
using DocumentFormat.OpenXml.Drawing;

namespace Tools.OpenXML.Helpers.Reports
{
    /// <summary>
    /// 工作表帮助类基类
    /// </summary>
    /// <typeparam name="T">数据的类型</typeparam>
    abstract class SheetHelper<T>
    {
        #region 成员变量
        private static readonly object _obj = new object();

        /// <summary>
        /// 工作表名字
        /// </summary>
        protected readonly string _sheetName;
        /// <summary>
        /// 数据
        /// </summary>
        protected readonly T _data;
        /// <summary>
        /// OpenXMLExcel操作类
        /// </summary>
        protected readonly OpenXMLExcelBase _openXMLExcel;

        /// <summary>
        /// 工作表的SheetId
        /// </summary>
        private uint? _sheetId;
        /// <summary>
        /// 行索引，指示下一行是第几行
        /// </summary>
        protected uint _rowIndex;
        /// <summary>
        /// SAX Writer
        /// </summary>
        protected OpenXmlWriter _writer;

        /// <summary>
        /// 第一个单元格索引
        /// </summary>
        protected abstract uint _firstCellIndex { get; }
        /// <summary>
        /// 最后一个单元格索引
        /// </summary>
        protected abstract uint _lastCellIndex { get; }

        public C.Chart Chart { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="openXMLExcel"></param>
        /// <param name="data"></param>
        /// <param name="sheetId"></param>
        public SheetHelper(string sheetName, OpenXMLExcelBase openXMLExcel, T data, uint? sheetId = null)
        {
            _sheetName = sheetName;
            _openXMLExcel = openXMLExcel;
            _data = data;
            _sheetId = sheetId;
        }

        /// <summary>
        /// 生成工作表
        /// </summary>
        public void Generate(bool includeChart = false)
        {
            DrawingsPart drawingsPart = null;
            var worksheetPart = _openXMLExcel.Document.WorkbookPart.AddNewPart<WorksheetPart>();
            if (includeChart)
            {
                drawingsPart = worksheetPart.AddNewPart<DrawingsPart>();
            }
            _openXMLExcel.AddSheetToPart(worksheetPart, _sheetName, _sheetId);
            CreateSheetBySAX(worksheetPart, drawingsPart);
            if(drawingsPart != null)
            {
                CreateDrawingBySAX(drawingsPart);
            }
        }


        /// <summary>
        /// 通过SAX创建工作表
        /// </summary>
        /// <param name="worksheetPart"></param>
        private void CreateSheetBySAX(WorksheetPart worksheetPart, DrawingsPart drawingsPart)
        {
            using (_writer = OpenXmlWriter.Create(worksheetPart))
            {
                _rowIndex = 1;

                //S: Worksheet
                _writer.WriteStartElement(new Worksheet());

                if(drawingsPart != null)
                {
                    _writer.WriteElement(new Drawing() { Id = worksheetPart.GetIdOfPart(drawingsPart) });
                }

                InitSheetViews();
                InitColumns();
                MergeCells();
                FillData();

                //E: Worksheet
                _writer.WriteEndElement();
                _writer.Close();
            }

        }


        private void CreateDrawingBySAX(DrawingsPart drawingsPart)
        {
            var data = _data as IReadOnlyList<Sheet3Data>;
            var chartPart = drawingsPart.AddNewPart<ChartPart>();

            var dataDic = new Dictionary<C.Values, C.SeriesText>();
            for (uint i = 1; i <= data.First().DataDic.Count; i++)
            {
                var columnName = OpenXMLExcels.GetColumnNameByIndex(i);
                dataDic[new C.Values() { NumberReference = new C.NumberReference() { Formula = new C.Formula($"{_sheetName}!${columnName}$2:${columnName}${data.Count + 2}") } }]
                    = new C.SeriesText() { StringReference = new C.StringReference() { Formula = new C.Formula($"{_sheetName}!${columnName}$1") } };
            }
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

            // Append a GraphicFrame to the TwoCellAnchor object.
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
                Graphic = new Graphic(new GraphicData(new C.ChartReference() { Id = drawingsPart.GetIdOfPart(chartPart) })
                {
                    Uri = "http://schemas.openxmlformats.org/drawingml/2006/chart",
                })
            });

            twoCellAnchor.Append(new Xdr.ClientData());
            using (var writer = OpenXmlWriter.Create(chartPart))
            {
                //S: ChartSpace
                writer.WriteStartElement(new C.ChartSpace());
                writer.WriteElement(new C.EditingLanguage() { Val = "zh-CN" });

                //S: Chart
                writer.WriteStartElement(new C.Chart());

                //S: PlotArea
                writer.WriteStartElement(new C.PlotArea());
                writer.WriteElement(new C.Layout());

                //S: LineChart
                writer.WriteStartElement(new C.LineChart());
                writer.WriteElement(new C.Grouping() { Val = C.GroupingValues.Standard });

                uint index = 0;
                foreach (var dataKvp in dataDic)
                {
                    //S: LineChartSeries
                    writer.WriteStartElement(new C.LineChartSeries());
                    writer.WriteElement(dataKvp.Key);
                    writer.WriteElement(new C.Index() { Val = index });
                    writer.WriteElement(new C.Order() { Val = index });
                    writer.WriteElement(dataKvp.Value);
                    if (index++ == 0)
                    {
                        var axisData = new C.CategoryAxisData()
                        {
                            StringReference = new C.StringReference() { Formula = new C.Formula($"{_sheetName}!$A$2:$A${data.Count + 2}") }
                        };

                        writer.WriteElement(axisData);
                    }
                    //E: LineChartSeries
                    writer.WriteEndElement();
                }

                writer.WriteElement(new C.AxisId() { Val = 0 });
                writer.WriteElement(new C.AxisId() { Val = 1 });

                //E: LineChart
                writer.WriteEndElement();

                writer.WriteElement(new C.CategoryAxis(
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

                writer.WriteElement(new C.ValueAxis(
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
                //E: PlotArea
                writer.WriteEndElement();

                writer.WriteElement(new C.Legend(
                    new C.LegendPosition() { Val = C.LegendPositionValues.Right },
                    new C.Layout()
                ));
                writer.WriteElement(new C.PlotVisibleOnly() { Val = true });
                //E: Chart
                writer.WriteEndElement();
                //E: ChartSpace
                writer.WriteEndElement();

                writer.Close();
            }

           
        }

        /// <summary>
        /// 初始化表格视图
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void InitSheetViews() { }

        /// <summary>
        /// 初始化列
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void InitColumns() { }

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void MergeCells() { }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void FillData() { }

        /// <summary>
        /// 获取单元格引用
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        protected string GetCellReference(uint columnIndex, uint rowIndex)
        => $"{ OpenXMLExcels.GetColumnNameByIndex(columnIndex) }{ rowIndex }";

        /// <summary>
        /// 获取只有普通Border的单元格格式索引
        /// </summary>
        /// <param name="borderStyle"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        protected uint GetBorderCellFormatIndex(BorderStyleValues borderStyle, DColor dColor)
        {
            var borderId = _openXMLExcel.GetBorderId(borderStyle, dColor);
            return _openXMLExcel.GetCellFormatIndex(borderId);
        }

        /// <summary>
        /// 设置合并单元格的样式
        /// </summary>
        /// <param name="startColIndex"></param>
        /// <param name="stopColIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="styleIndex"></param>
        protected void SetMergeCellStyle(uint startColIndex, uint stopColIndex, uint rowIndex, uint styleIndex)
        {
            for (uint i = startColIndex; i <= stopColIndex; i++)
            {
                _writer.WriteElement(new Cell() { CellReference = GetCellReference(i, rowIndex), StyleIndex = styleIndex });
            }
        }
    }
}
