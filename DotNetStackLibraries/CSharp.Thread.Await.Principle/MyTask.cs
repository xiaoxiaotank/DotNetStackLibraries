using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CSharp.Thread.Await.Principle
{
    class MyTask<T>
    {
        public MyAwaiter<T> GetAwaiter()
        {
            return new MyAwaiter<T>();
        }
    }

    class MyAwaiter<T> : INotifyCompletion
    {
        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
            throw new NotImplementedException();
        }

        public T GetResult()
        {
            throw new NotImplementedException();
        }
    }
}
