using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Tools.OpenXMLExcel;

namespace Tools.OpenXML.Helpers.Reports
{
    /// <summary>
    /// 工作表帮助类基类
    /// </summary>
    /// <typeparam name="T">数据的类型</typeparam>
    abstract class SheetHelper<T>
    {
        private static readonly object _obj = new object();
        /// <summary>
        /// 第一个单元格索引
        /// </summary>
        protected abstract uint _firstCellIndex { get; }
        /// <summary>
        /// 最后一个单元格索引
        /// </summary>
        protected abstract uint _lastCellIndex { get; }
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
        /// 
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="openXMLExcel"></param>
        /// <param name="data"></param>
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
        public void Generate()
        {
            WorksheetPart worksheetPart;
            lock (_obj)
            {
                worksheetPart = _openXMLExcel.Document.WorkbookPart.AddNewPart<WorksheetPart>();
            }
            _openXMLExcel.AddSheetToPart(worksheetPart, _sheetName, _sheetId);
            CreateSheetBySAX(worksheetPart);
        }

        /// <summary>
        /// 通过SAX创建工作表
        /// </summary>
        /// <param name="worksheetPart"></param>
        private void CreateSheetBySAX(WorksheetPart worksheetPart)
        {
            using (_writer = OpenXmlWriter.Create(worksheetPart))
            {
                _rowIndex = 1;
                //S: Worksheet
                _writer.WriteStartElement(new Worksheet());

                InitSheetViews();
                InitColumns();
                MergeCells();
                FillData();

                //E: Worksheet
                _writer.WriteEndElement();
                _writer.Close();
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
    }
}
