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
using Tools.OpenXML.Models;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML.Helpers.Reports.Report1
{
    partial class Report1Helper : ReportHelper<ReportDataModel>
    {
        #region 构造器
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="data">数据</param>
        public Report1Helper(string fileName, ReportDataModel data) : base(fileName, data)
        {
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 通过操作内存生成报告
        /// </summary>
        public void GenerateReportByMemory()
        {
            InitDocument();

            var worksheet1 = _openXMLExcel.AddWorksheet("TestSheet-0", 1);
            FillSheet1ByMemory(worksheet1);
        }

        /// <summary>
        /// 通过操作硬盘生成报告
        /// </summary>
        public override async Task GenerateAsync()
        {
            await base.GenerateAsync();

            //var task1 = Task.Factory.StartNew(() => new Sheet1Helper("TestSheet-1", _openXMLExcel, _data.Sheet1Data, 2).Generate());
            //var task2 = Task.Factory.StartNew(() => new Sheet2Helper("TestSheet-2", _openXMLExcel, _data.Sheet2Data, 3).Generate());
            //需要公式的表格命名不能带 “-”
            var task3 = Task.Factory.StartNew(() => new Sheet3Helper("TestSheet3", _openXMLExcel, _data.Sheet3DataList, 4).Generate(true));
            
            await Task.WhenAll(task3);
        }

        #endregion

        #region 私有方法
        /// <summary>
        /// 操作内存填写工作表1，速度慢
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
        #endregion
    }
}
