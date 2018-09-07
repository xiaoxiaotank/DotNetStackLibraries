using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Common
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 判断集合是否为null或空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// 判断集合是否不为null或空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool IsNotNullAndEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }
    }
}
