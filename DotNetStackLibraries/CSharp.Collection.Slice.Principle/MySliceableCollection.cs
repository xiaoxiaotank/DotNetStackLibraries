using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Collection.Slice.Principle
{
    class MySliceableCollection
    {
        private int[] _array = new int[10];

        public int Count => _array.Length;

        public int this[int index] => throw new NotImplementedException();

        public int[] Slice(int x, int y) => throw new NotImplementedException();
    }
}
