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
        public const double DefaultFontSize = 11;
        public const string DefaultFontName = "宋体";

        public static readonly Font DefaultFont = new Font() { FontSize = new FontSize() { Val = DefaultFontSize }, FontName = new FontName() { Val = DefaultFontName } };
        public static readonly Border DefaultBorder = new Border() { LeftBorder = new LeftBorder(), TopBorder = new TopBorder(), RightBorder = new RightBorder(), BottomBorder = new BottomBorder() };
        public static readonly Fill[] DefaultFills = new[] { new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.None } }, new Fill() { PatternFill = new PatternFill() { PatternType = PatternValues.Gray125 } } };
        public static readonly CellFormat DefaultCellForamt = new CellFormat() { FontId = 0, FillId = 0, BorderId = 0, Alignment = new Alignment() { WrapText = true, Horizontal = HorizontalAlignmentValues.Left, Vertical = VerticalAlignmentValues.Center } };

        /// <summary>
        /// 获取字体
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontName"></param>
        /// <param name="bold"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public static Font GetFont(double size, string fontName, Bold bold, DColor dColor)
        {
            var font = new Font()
            {
                FontSize = new FontSize() { Val = size },
                FontName = new FontName() { Val = fontName },
                Bold = bold,
                Color = new Color() { Rgb = HexBinaryValue.FromString(dColor.GetRgbString()) },
            };
            return font;
        }

        /// <summary>
        /// 获取边框
        /// </summary>
        /// <param name="style"></param>
        /// <param name="rgb"></param>
        /// <returns></returns>
        public static Border GetBorder(BorderStyleValues style, DColor dColor)
        {
            var color = new Color() { Rgb = new HexBinaryValue(dColor.GetRgbString()) };
            var border = new Border()
            {
                LeftBorder = new LeftBorder() { Color = color, Style = style },
                TopBorder = new TopBorder() { Color = color.Clone() as Color, Style = style },
                RightBorder = new RightBorder() { Color = color.Clone() as Color, Style = style },
                BottomBorder = new BottomBorder() { Color = color.Clone() as Color, Style = style }
            };

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
        /// 获取单元格样式
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <returns></returns>
        public static CellFormat GetCellFormat(uint? borderId = null, uint? fontId = null, uint? fillId = null, uint? formatId = null, Alignment alignment = null)
        {
            var cellFormat = new CellFormat()
            {
                BorderId = borderId,
                FontId = fontId,
                FillId = fillId,
                FormatId = formatId,
                Alignment = alignment,
                ApplyBorder = borderId.HasValue,
                ApplyFont = fontId.HasValue,
                ApplyFill = fillId.HasValue,
                ApplyAlignment = alignment != null
            };
            return cellFormat;
        }

        /// <summary>
        /// 获取Border的Key
        /// </summary>
        /// <param name="borderStyle"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public static string GetBorderStyleKey(BorderStyleValues borderStyle = BorderStyleValues.None, DColor? dColor = null)
        => $"{borderStyle}.{dColor}";

        /// <summary>
        /// 获取Fill的Key
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="foreDColor"></param>
        /// <param name="backDColor"></param>
        /// <returns></returns>
        public static string GetFillStyleKey(PatternValues pattern = PatternValues.None, DColor? foreDColor = null, DColor? backDColor = null)
        => $"{pattern}.{foreDColor}.{backDColor}";

        /// <summary>
        /// 获取Font的Key
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontName"></param>
        /// <param name="bold"></param>
        /// <param name="dColor"></param>
        /// <returns></returns>
        public static string GetFontStyleKey(double size = DefaultFontSize, string fontName = DefaultFontName, Bold bold = null, DColor? dColor = null)
            => $"{size}.{fontName}.{bold}.{dColor}";

        /// <summary>
        /// 获取CellFormat的Key
        /// </summary>
        /// <param name="borderId"></param>
        /// <param name="fontId"></param>
        /// <param name="fillId"></param>
        /// <param name="formatId"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static string GetCellFormatStyleKey(uint? borderId = null, uint? fontId = null, uint? fillId = null, uint? formatId = null, Alignment alignment = null)
            => $"{borderId}.{fontId}.{fillId}.{formatId}.{alignment.Vertical}.{alignment.Horizontal}";
    }
}
