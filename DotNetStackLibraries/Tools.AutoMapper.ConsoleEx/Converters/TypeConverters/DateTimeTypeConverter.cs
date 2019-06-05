using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx.Converters
{
    /// <summary>
    /// 类型转换器范围是全局的，所有实体的映射都会遵循这个规则
    /// </summary>
    class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }
}
