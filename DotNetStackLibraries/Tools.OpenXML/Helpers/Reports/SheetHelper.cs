using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Tools.OpenXMLExcel;
using DColor = System.Drawing.Color;

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
