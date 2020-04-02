using System;

namespace CSharp.Struct.AutoDispose
{
    /// <summary>
    /// <c>ref struct</c> 因为必须在栈上且不能被装箱，所以不能实现接口，但是如果你的 <c>ref struct</c> 中有一个 <c>void Dispose()</c> 那么就可以用 <c>using</c> 语法实现对象的自动销毁
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using var myStruct = new MyDisposableStruct();
            Console.WriteLine("Hello World!");
        }
    }
}
