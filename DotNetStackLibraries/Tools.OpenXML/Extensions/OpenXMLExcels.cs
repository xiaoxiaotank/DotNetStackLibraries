using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.OpenXML.Enums;
using Tools.OpenXML.Extensions;
using DColor = System.Drawing.Color;

namespace Tools.OpenXML
{
    static class OpenXMLExcels
    {
        #region 成员变量
        /// <summary>
        /// 字母个数
        /// </summary>
        public const byte LetterCount = 26;
        /// <summary>
        /// 默认字号
        /// </summary>
        public const double DefaultFontSize = 11;
        /// <summary>
        /// 默认字体名
        /// </summary>
        public const string DefaultFontName = "宋体";
        /// <summary>
        /// 默认字体
        /// </summary>
        public static readonly Font DefaultFont = new Font() { FontSize = new FontSize() { Val = DefaultFontSize }, FontName = new FontName() { Val = DefaultFontName }, FontScheme = new FontScheme() { Val = FontSchemeValues.Minor } };
        /// <summary>
        /// 默认边框
        /// </summary>
        public static readonly Border DefaultBorder = new Border() { LeftBorder = new LeftBorder(), TopBorder = new TopBorder(), RightBorder = new RightBorder(), BottomBorder = new BottomBorder(), DiagonalBorder = new DiagonalBorder() };
        /// <summary>
        /// 默认填充数组
        /// </summary>
        public static readonly Fill[] DefaultFills = new[]
        {
            new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.None } },
            new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 } }
        };
        /// <summary>
        /// 默认数字格式
        /// </summary>
        public static readonly NumberingFormat[] DefaultNumberingFormats = new NumberingFormat[]
        {
            new NumberingFormat() { NumberFormatId = 42, FormatCode = "_ \"￥\"* #,##0_ ;_ \"￥\"* \\-#,##0_ ;_ \"￥\"* \"-\"_ ;_ @_ " },
            new NumberingFormat() { NumberFormatId = 44, FormatCode = "_ \"￥\"* #,##0.00_ ;_ \"￥\"* \\-#,##0.00_ ;_ \"￥\"* \"-\"??_ ;_ @_ " },
            new NumberingFormat() { NumberFormatId = 41, FormatCode = "_ * #,##0_ ;_ * \\-#,##0_ ;_ * \"-\"_ ;_ @_ " },
            new NumberingFormat() { NumberFormatId = 43, FormatCode = "_ * #,##0.00_ ;_ * \\-#,##0.00_ ;_ * \"-\"??_ ;_ @_ " },
        };
        /// <summary>
        /// 默认单元格格式
        /// </summary>
        public static readonly CellFormat DefaultCellForamt = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0, NumberFormatId = 0, Alignment = new Alignment() { Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center } }; 
        #endregion

        /// <summary>
        /// 合并单元格
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="mergeCells"></param>
        public static void MergeCells(this OpenXmlWriter writer, IEnumerable<MergeCell> mergeCells)
        {
            //S: MergeCells
            writer.WriteStartElement(new MergeCells());
            foreach (var mergeCell in mergeCells)
            {
                writer.WriteElement(mergeCell);
            }
            //E: MergeCells
            writer.WriteEndElement();
        }

        /// <summary>
        /// 初始化列
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="columns"></param>
        public static void InitColumns(this OpenXmlWriter writer, IEnumerable<Column> columns)
        {
            //S: Columns
            writer.WriteStartElement(new Columns());
            foreach (var column in columns)
            {
                writer.WriteElement(column);
            }
            //E: Columns
            writer.WriteEndElement();
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="size"></param>
        /// <param name="name"></param>
        /// <param name="dColor"></param>
        /// <param name="bold"></param>
        /// <param name="underline"></param>
        /// <returns></returns>
        public static Font GetFont(double size, string name, DColor dColor, Bold bold, Underline underline)
        {
            var font = new Font()
            {
                FontSize = new FontSize() { Val = size },
                FontName = new FontName() { Val = name },
                Bold = bold,
                Color = new Color() { Rgb = HexBinaryValue.FromString(dColor.GetRgbString()) },
                Underline = underline
            };
            return font;
        }

        /// <summary>
        /// 获取边框
        /// </summary>
        /// <param name="style"></param>
        /// <param name="dColor"></param>
        /// <param name="includeDiagonal"></param>
        /// <returns></returns>
        public static Border GetBorder(BorderStyleValues style, DColor dColor, bool includeDiagonal, bool isDiagonalDown)
        {
            var color = new Color() { Rgb = new HexBinaryValue(dColor.GetRgbString()) };
            var border = new Border()
            {
                LeftBorder = new LeftBorder() { Color = color, Style = style },
                TopBorder = new TopBorder() { Color = color.CloneSafely(), Style = style },
                RightBorder = new RightBorder() { Color = color.CloneSafely(), Style = style },
                BottomBorder = new BottomBorder() { Color = color.CloneSafely(), Style = style }
            };
            if (includeDiagonal)
            {
                border.DiagonalBorder = new DiagonalBorder() { Color = color.CloneSafely(), Style = style };
                if (isDiagonalDown)
                {
                    border.DiagonalDown = true;
                }
                else
                {
                    border.DiagonalUp = true;
                }
            }

            return border;
        }

        /// <summary>
        /// 获取填充
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="foreDColor"></param>
        /// <param name="backDColor"></param>
        public static Fill GetFill(PatternValues pattern, DColor foreDColor, DColor backDColor)
        {
            var fill = new Fill()
            {
                PatternFill = new PatternFill()
                {
                    PatternType = pattern,
                    //填充色
                    ForegroundColor = new ForegroundColor() { Rgb = HexBinaryValue.FromString(foreDColor.GetRgbString()) },
                    //暂时未知
                    BackgroundColor = new BackgroundColor() { Rgb = HexBinaryValue.FromString(backDColor.GetRgbString()) }
                }
            };

            return fill;
        }

        /// <summary>
        /// 获取数字格式
        /// </summary>
        /// <param name="formatCode"></param>
        /// <returns></returns>
        public static NumberingFormat GetNumberingFormat(string formatCode)
        {
            var numberingFormat = new NumberingFormat()
            {
                FormatCode = StringValue.FromString(formatCode)
            };

            return numberingFormat;
        }

        /// <summary>
        /// 获取单元格样式
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <returns></returns>
        public static CellFormat GetCellFormat(uint? borderId, uint? fontId, uint? fillId, uint? formatId, uint? numberFormatId, Alignment alignment)
        {
            var cellFormat = new CellFormat()
            {
                BorderId = borderId,
                FontId = fontId,
                FillId = fillId,
                FormatId = formatId,
                NumberFormatId = numberFormatId,
                Alignment = alignment,
                ApplyBorder = borderId.HasValue,
                ApplyFont = fontId.HasValue,
                ApplyFill = fillId.HasValue,
                ApplyNumberFormat = numberFormatId.HasValue,
                ApplyAlignment = alignment != null,
            };
            return cellFormat;
        }

        /// <summary>
        /// 获取Border的Key
        /// </summary>
        /// <param name="borderStyle"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public static string GetBorderStyleKey(BorderStyleValues borderStyle, DColor? dColor, bool includeDiagonal, bool isDiagonalDown)
        => $"Border.{borderStyle}.{dColor}.{includeDiagonal}.{isDiagonalDown}";

        /// <summary>
        /// 获取Fill的Key
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="foreDColor"></param>
        /// <param name="backDColor"></param>
        /// <returns></returns>
        public static string GetFillStyleKey(PatternValues pattern, DColor? foreDColor, DColor? backDColor)
        => $"Fill.{pattern}.{foreDColor}.{backDColor}";

        /// <summary>
        /// 获取Font的Key
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontName"></param>
        /// <param name="dColor"></param>
        /// <param name="bold"></param>
        /// <param name="underline"></param>
        /// <returns></returns>
        public static string GetFontStyleKey(double size, string fontName, DColor? dColor, Bold bold, Underline underline)
        => $"Font.{size}.{fontName}.{dColor}.{bold}.{underline?.Val}";

        /// <summary>
        /// 获取CellFormat的Key
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <param name="formatId"></param>
        /// <param name="numberFormatId"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static string GetCellFormatStyleKey(uint? borderId, uint? fontId, uint? fillId, uint? formatId, uint? numberFormatId, Alignment alignment)
        => $"CellFormat.{borderId}.{fontId}.{fillId}.{formatId}.{numberFormatId}.{alignment?.Vertical}.{alignment?.Horizontal}";

        /// <summary>
        /// 获取NumberingFormat的Key
        /// </summary>
        /// <param name="formatCode"></param>
        /// <returns></returns>
        public static string GetNumberingFormatKey(string formatCode)
        => $"NumberingFormat.{formatCode}";

        /// <summary>
        /// 通过索引获取列的符号,从 0 开始
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetColumnNameByIndex(uint index)
        {
            var name = new StringBuilder();

            var i = index;
            while(i / LetterCount > 0)
            {
                name.Append((char)('A' + i % LetterCount));
                i = i / LetterCount - 1;
            }

            return string.Concat(name.Append((char)('A' + i)).ToString().Reverse());
        }

        /// <summary>
        /// 类型安全的克隆
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T CloneSafely<T>(this T element) where T : OpenXmlElement
        {
            return element.Clone() as T;
        }
    }
}
