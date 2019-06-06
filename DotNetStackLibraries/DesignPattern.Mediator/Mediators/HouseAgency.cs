using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DesignPattern.Mediator
{
    /// <summary>
    /// 房屋中介
    /// </summary>
    class HouseAgency : Mediator
    {
        public virtual void Notify(Customer customer, string message)
        {
            Console.WriteLine($"{customer.Name}({customer.Identity.GetDescription()}):{message}");
        }

    }

    static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值的描述（Description）
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value, string separator = ",")
        {
            var type = value.GetType();
            var descs = value.ToString()
                .Split(',')
                .Select(name =>
                {
                    var desc = type.GetField(name.Trim(' '))?
                        .GetCustomAttribute<DescriptionAttribute>()?
                        .Description;
                    return desc ?? name;
                });

            return string.Join(separator, descs);
        }
    }
}
