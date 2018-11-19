using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tools.OpenXML.Extensions
{
    static class Enums
    {
        /// <summary>
        /// 获取EnumMember的Value
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue<TEnum>(this TEnum value) where TEnum : struct
        {
            var memInfo = typeof(TEnum).GetMember(value.ToString());
            var attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();

            return attr?.Value;
        }
    }
}
