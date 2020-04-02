using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Thread.AwaitForeach.Principle
{
    class MyAsyncEnumerable<T>
    {
        public MyAsyncEnumerator<T> GetAsyncEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    class MyAsyncEnumerator<T>
    {
        public T Current { get; private set; }

        public Task<bool> MoveNextAsync()
        {
            throw new NotImplementedException();
        }
    }
}
