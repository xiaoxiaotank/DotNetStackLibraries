using System;
using System.Collections.Generic;

namespace CSharp.Collection.Slice.Principle
{
    /// <summary>
    /// 并不是必须提供一个接收 <see cref="Range"/> 类型参数的 indexer 才能使用切片
    /// 可切片的原理：
    /// <list type="number">
    ///     <item>该类可以被计数（拥有 <c>Length</c> 或 <c>Count</c> 属性）</item>
    ///     <item>该类拥有 <c>Slice(int, int)</c> 方法</item>
    /// </list>
    /// 
    /// 并不是只有 <see cref="Index"/> 才能使用索引
    /// 可索引的原理：
    /// <list type="number">
    ///     <item>该类可以被计数（拥有 <c>Length</c> 或 <c>Count</c> 属性）</item>
    ///     <item>该类拥有一个接收 <see cref="int"/> 参数的索引器</item>
    /// </list>
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new MySliceableCollection();
            Console.WriteLine(collection[1..]);
            Console.WriteLine(collection[^1]);

            Console.WriteLine("Hello World!");
        }
    }
}
