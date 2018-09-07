using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Common
{
    public static class StringExtension
    {
        /// <summary>
        /// 指示指定的字符串是 null、空 还是 仅由空白字符组成
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 指示指定的字符串不是 null、空 和 仅由空白字符组成
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullAndWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 指示指定的字符串是 null 还是 System.String.Empty 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 指示指定的字符串不是 null 和 System.String.Empty 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }
    }
}
