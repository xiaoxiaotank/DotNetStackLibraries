using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DotNet.Common
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举值的描述
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            string value = enumValue.ToString();
            var field = enumValue.GetType().GetField(value);
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性

            //当描述属性没有时，直接返回名称
            if (attributes.Length == 0)
                return value;
            var descriptionAttribute = attributes[0] as DescriptionAttribute;
            return descriptionAttribute.Description;
        }
    }
}
