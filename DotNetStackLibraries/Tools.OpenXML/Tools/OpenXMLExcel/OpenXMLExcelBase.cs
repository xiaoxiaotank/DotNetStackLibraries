using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Concurrent;
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
        protected Lazy<ConcurrentDictionary<string, uint>> _styleIdDic = new Lazy<ConcurrentDictionary<string, uint>>();

        /// <summary>
        /// Excel文档
        /// </summary>
        public SpreadsheetDocument Document { get; protected set; }

        /// <summary>
        /// 创建电子表格文档
        /// </summary>
        /// <param name="fileFullName">文件全路径</param>
        /// <returns></returns>
        public abstract SpreadsheetDocument CreateDocument(string fileFullName);

        #region Add
        /// <summary>
        /// 添加工作簿
        /// </summary>
        /// <param name="document"></param>
        public abstract void AddWorkbook();

        /// <summary>
        /// 添加工作表
        /// </summary>
        /// <param name="sheetName"></param>
        /// <param name="sheetId">null表示自动赋值</param>
        /// <returns></returns>
        public abstract Worksheet AddWorksheet(string sheetName, uint? sheetId = null);

        /// <summary>
        /// 添加Sheet
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="sheetName"></param>
        /// <param name="sheetId">null表示自动赋值</param>
        public abstract void AddSheetToPart(WorksheetPart worksheetPart, string sheetName, uint? sheetId = null);

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
        /// 添加数字格式
        /// </summary>
        /// <param name="numberingFormats"></param>
        /// <returns></returns>
        public abstract uint AddNumberingFormats(params NumberingFormat[] numberingFormats);

        /// <summary>
        /// 添加单元格格式
        /// </summary>
        /// <param name="cellFormat"></param>
        /// <returns>最后一个样式的Id</returns>
        public abstract uint AddCellFormats(params CellFormat[] cellFormats);
        #endregion

        #region Add or Get
        /// <summary>
        /// 创建或获取字体Id
        /// </summary>
        /// <param name="fontSize"></param>
        /// <param name="fontName"></param>
        /// <param name="dColor"></param>
        /// <param name="bold"></param>
        /// <param name="underline"></param>
        /// <returns></returns>
        public abstract uint GetFontId(double fontSize, string fontName, DColor dColor, Bold bold = null, Underline underline = null);

        /// <summary>
        /// 创建或获取边框Id
        /// </summary>
        /// <param name="style"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public abstract uint GetBorderId(BorderStyleValues style, DColor dColor, bool includeDiagonal = false, bool isDiagonalDown = true);

        /// <summary>
        /// 创建或获取填充Id
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="foreDColor"></param>
        /// <param name="backDColor"></param>
        /// <returns></returns>
        public abstract uint GetFillId(PatternValues pattern, DColor foreDColor, DColor backDColor);

        /// <summary>
        /// 创建或获取数字格式Id
        /// </summary>
        /// <param name="formatCode"></param>
        /// <returns></returns>
        public abstract uint GetNumberingFormatId(string formatCode);

        /// <summary>
        /// 创建或获取单元格格式Id
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <param name="formatId"></param>
        /// <param name="numberFormatId"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public abstract uint GetCellFormatIndex(uint? borderId = null, uint? fontId = null, uint? fillId = null, uint? formatId = null, uint? numberFormatId = null, Alignment alignment = null); 
        #endregion

        /// <summary>
        /// 获取默认样式表
        /// </summary>
        /// <param name="stylesPart"></param>
        protected static Stylesheet GetDefaultStylesheet()
        {
            //一个对象只能属于一个XML节点，所以使用clone
            var fonts = new Fonts() { Count = 1 };
            fonts.Append(OpenXMLExcels.DefaultFont.Clone() as Font);

            var borders = new Borders() { Count = 1 };
            borders.Append(OpenXMLExcels.DefaultBorder.Clone() as Border);

            var fills = new Fills() { Count = (uint)OpenXMLExcels.DefaultFills.Count() };
            fills.Append(OpenXMLExcels.DefaultFills.Select(f => f.Clone() as Fill));

            var numberingFormats = new NumberingFormats() { Count = 1 };
            numberingFormats.Append(OpenXMLExcels.DefaultNumberingFormat.Clone() as NumberingFormat);

            var cellStyleFormats = new CellStyleFormats() { Count = 1 };
            cellStyleFormats.Append(OpenXMLExcels.DefaultCellForamt.Clone() as Border);

            var cellFormats = new CellFormats() { Count = 1 };
            cellFormats.Append(OpenXMLExcels.DefaultCellForamt.Clone() as OpenXmlElement);

            var stylesheet = new Stylesheet()
            {
                Fonts = fonts,
                Borders = borders,
                Fills = fills,
                CellFormats = cellFormats,
                CellStyleFormats = cellStyleFormats,
                NumberingFormats = numberingFormats
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
            return key != null && _styleIdDic.Value.ContainsKey(key) ? _styleIdDic.Value[key] : default(uint?);
        }
    }
}
