using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace NoSQL.StackExchange.Redis.Core
{
    /// <summary>
    /// 序列化辅助类
    /// </summary>
    public class SerializeHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Serialize<T>(T target) where T : class
        {
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, target);
                ms.Seek(0, SeekOrigin.Begin);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Derialize<T>(string target)
        {
            using (var ms = new MemoryStream(Convert.FromBase64String(target)))
            {
                var result = new BinaryFormatter().Deserialize(ms);
                return (T)result;
            }
        }
    }
}