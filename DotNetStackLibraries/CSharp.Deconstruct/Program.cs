using System;

namespace CSharp.Deconstruct
{
    /// <summary>
    /// 解构函数重载只支持参数个数不同的重载
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var myClass = new MyDeconstructableClass();
            var (a1, b1) = myClass;
            var (a2, b2, c2) = myClass;

            Console.WriteLine("Hello World!");
        }
    }
}
