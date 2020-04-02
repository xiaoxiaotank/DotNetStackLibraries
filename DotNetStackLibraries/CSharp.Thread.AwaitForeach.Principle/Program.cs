using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharp.Thread.AwaitForeach.Principle
{
    /// <summary>
    /// 并不是只有 <see cref="IAsyncEnumerable{T}" /> 和 <see cref="IAsyncEnumerator{T}" /> 才能 <c>await</c>
    /// <para>可 <c>await</c> 原理：
    /// <list type="number">
    ///     <item>该类包含 <c>GetAsyncEnumerator()</c> 方法</item>
    ///     <item><c>GetAsyncEnumerator()</c> 方法返回的对象包含 <c>MoveNextAsync()</c> 方法和 <c>Current</c> 属性</item>
    ///     <item>其中，<c>MoveNextAsync()</c> 方法返回的对象应该是一个 <c>Awaitable{bool}</c></item>
    /// </list>
    /// </para>
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            await foreach (var item in new MyAsyncEnumerable<int>())
            {

            }
            Console.WriteLine("Hello World!");
        }
    }
}
