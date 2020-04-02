using System;

namespace CSharp.Deconstruct
{
    class Program
    {
        static void Main(string[] args)
        {
            var myClass = new MyDeconstructableClass();
            (var a, var b) = myClass;

            Console.WriteLine("Hello World!");
        }
    }
}
