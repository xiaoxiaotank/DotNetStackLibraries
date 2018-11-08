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
    }
}
