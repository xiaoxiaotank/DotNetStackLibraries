using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Deconstruct
{
    class MyDeconstructableClass
    {
        public int A => 1;

        public int B => 2;

        public string C => "jjj";

        public void Deconstruct(out int a, out int b)
        {
            a = A;
            b = B;
        }

        public void Deconstruct(out int a, out int b, out string c)
        {
            a = A;
            b = B;
            c = C;
        }
    }
}
