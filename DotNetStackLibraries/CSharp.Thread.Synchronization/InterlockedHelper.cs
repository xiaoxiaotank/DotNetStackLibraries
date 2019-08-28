using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CSharp.Thread.Synchronization
{
    abstract class CounterBase
    {
        protected int _count;

        public int Count => _count;

        public abstract void Increment();

        public abstract void Decrement();

        public void Test()
        {
            for (int i = 0; i < 10000; i++)
            {
                Increment();
                Decrement();
            }
        }
    }

    /// <summary>
    /// 未有锁的Counter
    /// 多线程环境下，会出现资源竞争，导致获取的Count不同步
    /// 如：
    ///     A线程获取了Count，
    ///     B线程随后获取了线程并执行了Count_new = Count - 1，
    ///     然后A线程仍旧使用了原Count + 1，导致Count值不同步
    /// </summary>
    class CounterWithoutLocker : CounterBase
    {
        public override void Increment()
        {
            _count++;
        }

        public override void Decrement()
        {
            _count--;
        }
    }

    /// <summary>
    /// 加了锁的Counter
    /// 不会出现Count值不同步的情况
    /// </summary>
    class CounterWithLocker : CounterBase
    {
        public override void Increment()
        {
            // 增量1，返回递增之后的值
            Interlocked.Increment(ref _count);
        }

        public override void Decrement()
        {
            Interlocked.Decrement(ref _count);
        }
    }
}
