using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML.Extensions
{
    static class ColorExtension
    {
        /// <summary>
        /// 将颜色转为16进制RGB
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetRgbString(this Color color)
        {
            return $"{color.R:x2}{color.G:x2}{color.B:x2}";
        }
    }
}
