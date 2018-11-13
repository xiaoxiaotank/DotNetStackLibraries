using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Enums;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML.Tools.OpenXMLExcel
{
    abstract class OpenXMLExcelBase
    {
        /// <summary>
        /// key:key
        /// value:样式下标
        /// </summary>
        protected Lazy<Dictionary<string, uint>> _styleIdDic = new Lazy<Dictionary<string, uint>>();

        public SpreadsheetDocument Document { get; protected set; }

        /// <summary>
        /// 创建电子表格文档
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public abstract SpreadsheetDocument CreateDocument(string fileFullName);

        /// <summary>
        /// 添加工作簿
        /// </summary>
        /// <param name="document"></param>
        public abstract void AddWorkbook();

        /// <summary>
        /// 添加工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public abstract Worksheet AddWorksheet(string sheetName);

        /// <summary>
        /// 添加Sheet
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="worksheetPart"></param>
        public abstract void AddSheetToPart(string sheetName, WorksheetPart worksheetPart);

        /// <summary>
        /// 添加字体
        /// </summary>
        /// <param name="font"></param>
        /// <returns>最后一个样式的Id</returns>
        public abstract uint AddFonts(params Font[] fonts);

        /// <summary>
        /// 添加边框
        /// </summary>
        /// <param name="border"></param>
        /// <returns>最后一个样式的Id</returns>
        public abstract uint AddBorders(params Border[] borders);

        /// <summary>
        /// 添加填充
        /// </summary>
        /// <param name="fill"></param>
        /// <returns>最后一个样式的Id</returns>
        public abstract uint AddFills(params Fill[] fills);

        /// <summary>
        /// 添加单元格格式
        /// </summary>
        /// <param name="cellFormat"></param>
        /// <returns>最后一个样式的Id</returns>
        public abstract uint AddCellFormats(params CellFormat[] cellFormats);

        /// <summary>
        /// 获取字体Id
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        /// <param name="bold"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public abstract uint GetFontId(double fontSize, string fontName, Bold bold, DColor dColor);

        /// <summary>
        /// 获取边框Id
        /// </summary>
        /// <param name="style"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public abstract uint GetBorderId(BorderStyleValues style, DColor dColor, bool includeDiagonal = false, bool isDiagonalDown = true);

        /// <summary>
        /// 获取填充Id
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="foreDColor"></param>
        /// <param name="backDColor"></param>
        /// <returns></returns>
        public abstract uint GetFillId(PatternValues pattern, DColor foreDColor, DColor backDColor);

        /// <summary>
        /// 获取单元格格式Id
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <param name="formatId"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public abstract uint GetCellFormatIndex(uint? borderId = null, uint? fontId = null, uint? fillId = null, uint? formatId = null, Alignment alignment = null);

        /// <summary>
        /// 获取默认样式表
        /// </summary>
        /// <param name="stylesPart"></param>
        protected static Stylesheet GetDefaultStylesheet()
        {
            var fonts = new Fonts() { Count = 1 };
            fonts.Append(OpenXMLExcels.DefaultFont);

            var borders = new Borders() { Count = 1 };
            borders.Append(OpenXMLExcels.DefaultBorder);

            var fills = new Fills() { Count = 2 };
            fills.Append(OpenXMLExcels.DefaultFills);

            var cellStyleFormats = new CellStyleFormats() { Count = 1 };
            cellStyleFormats.Append(OpenXMLExcels.DefaultCellForamt);
            var cellFormats = new CellFormats() { Count = 1 };
            //一个对象只能属于一个XML节点，所以使用clone
            cellFormats.Append(OpenXMLExcels.DefaultCellForamt.Clone() as OpenXmlElement);

            var stylesheet = new Stylesheet()
            {
                Fonts = fonts,
                Borders = borders,
                Fills = fills,
                CellFormats = cellFormats,
                CellStyleFormats = cellStyleFormats
            };
            return stylesheet;
        }

        /// <summary>
        /// 获取样式Id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected uint? GetStyleId(string key)
        {
            return _styleIdDic.Value.ContainsKey(key) ? _styleIdDic.Value[key] : default(uint?);
        }
    }
}
