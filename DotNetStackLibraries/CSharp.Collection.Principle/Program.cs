using System;

namespace CSharp.Collection.Principle
{
    class Program
    {
        static void Main(string[] args)
        {
            TestMyCollection();
        }

        static void TestMyCollection()
        {
            var collection1 = new MyCollection1<int>(new[] { 1, 2, 3, 4, 5 });
            var collection2 = new MyCollection2<int>(new[] { 6, 7, 8, 9, 10 });

            #region collection1
            foreach (var item in collection1)
            {
                Console.WriteLine(item);
            }

            var rator1 = collection1.GetEnumerator();
            while (rator1.MoveNext())
            {
                Console.WriteLine(rator1.Current);
            }

            #endregion
            Console.WriteLine("----------------------------------------");
            #region collection2
            foreach (var item in collection2)
            {
                Console.WriteLine(item);
            }

            var rator2 = collection2.GetEnumerator();
            while (rator2.MoveNext())
            {
                Console.WriteLine(rator2.Current);
            }
            #endregion
        }
    }
}
