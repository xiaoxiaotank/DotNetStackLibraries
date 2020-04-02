using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CSharp.Thread.Await.Principle
{
    /// <summary>
    /// 并不是只有 <see cref="Task" /> 和 <see cref="ValueTask" /> 才能 <c>await</c>
    /// <para>可 <c>await</c> 原理：
    /// <list type="number">
    ///     <item>该类包含 <c>GetAwaiter()</c> 方法和 <c>bool IsCompleted</c> 属性</item>
    ///     <item>并且 <c>GetAwaiter()</c> 返回值包含 <c>GetResult()</c> 方法、 <c>bool IsCompleted</c> 属性、实现了 <see cref="INotifyCompletion" /> 接口</item>
    /// </list>
    /// </para>
    /// </summary>
    class Program
    {
        static async Task Main(string[] args)
        {
            await new MyTask<int>();

            Console.WriteLine("Hello World!");
        }
    }
}
