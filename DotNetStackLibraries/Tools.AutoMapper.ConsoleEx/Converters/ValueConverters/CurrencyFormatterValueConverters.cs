using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx.Converters
{
    /// <summary>
    /// 值转换器范围限定为单个映射
    /// </summary>
    class CurrencyFormatterValueConverters : IValueConverter<decimal, string>
    {
        public string Convert(decimal sourceMember, ResolutionContext context)
        {
            return sourceMember.ToString("c");
        }
    }
}
