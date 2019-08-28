using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.Thread.Synchronization
{
    class Program
    {
        const string MutexName = "Mutex";

        static async Task Main(string[] args)
        {
            //await UseInterlockedAsync();
            //UseMutex();
            //await UseSemaphoreSlimAsync();
            //await UseAutoResetEventAsync();
            //await UseManualResetEventSlimAsync();
            //await UseCountDownEventAsync();
            //await UseBarrierAsync();
            //UseReaderWriterLockSlim();
            await UseSpinWaitAsync();

            Console.ReadKey();
        }

        /// <summary>
        /// 提供基本数学操作的原子方法
        /// </summary>
        /// <returns></returns>
        static async ValueTask UseInterlockedAsync()
        {
            var counterWithoutLocker = new CounterWithoutLocker();
            var task1 = Task.Run(() => counterWithoutLocker.Test());
            var task2 = Task.Run(() => counterWithoutLocker.Test());
            var task3 = Task.Run(() => counterWithoutLocker.Test());
            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"{nameof(CounterWithoutLocker)}'s {nameof(counterWithoutLocker.Count)}: {counterWithoutLocker.Count}");

            var counterWithLocker = new CounterWithLocker();
            task1 = Task.Run(() => counterWithoutLocker.Test());
            task2 = Task.Run(() => counterWithoutLocker.Test());
            task3 = Task.Run(() => counterWithoutLocker.Test());
            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine($"{nameof(CounterWithLocker)}'s {nameof(counterWithLocker.Count)}: {counterWithLocker.Count}");


            // Interlocked的其他方法：
            var count = 0;

            // 任意增量，如 count + 10，返回最新的count值
            Interlocked.Add(ref count, 10);
            Console.WriteLine($"{nameof(Interlocked.Add)}: {count}");
            // 直接赋值，如 count = 9，返回原来count的值
            var originalCount = Interlocked.Exchange(ref count, 9);
            Console.WriteLine($"{nameof(Interlocked.Exchange)}: {nameof(originalCount)}: {originalCount}\t{nameof(count)}: {count}");

            var newCount = count;
            // var1与var3进行比较，如果想等，则将var1赋值为var2，否则，不进行任何操作，并返回原始的var1的值
            originalCount = Interlocked.CompareExchange(ref count, 100, newCount);
            Console.WriteLine($"{nameof(Interlocked.CompareExchange)}: {nameof(originalCount)}: {originalCount}\t{nameof(count)}: {count}");

            var longCount = 0L;
            // 以原子方式读取64位值，仅用在32位操作系统中
            Interlocked.Read(ref longCount);

            // 执行当前线程的CPU对指令进行重新排序时，不能先执行该语句之后的内存存取，再执行该语句之前的内存存取，而要保证顺序。常用于弱内存排序的多处理器系统中
            Interlocked.MemoryBarrier();
#warning pending: Interlocked.MemoryBarrierProcessWide()
            //Interlocked.MemoryBarrierProcessWide();
        }

        /// <summary>
        /// 系统级别、跨进程的同步锁，线程间互斥
        /// </summary>
        static void UseMutex()
        {
            // initiallyOwned = true 表示如果mutex由该线程创建，那么该线程直接获取该mutex
            using var mutex = new Mutex(false, MutexName, out bool createdNew);
            Console.WriteLine($"mutex是否之前已存在：{!createdNew}");
            //如果该线程在指定时间内获取到mutex，该方法调用多次会使得线程获取多次mutex，导致多次mutex重入，相应的需要多次的释放mutex
            if (mutex.WaitOne(TimeSpan.FromSeconds(5)))
            {
                try
                {
                    Console.WriteLine("Running...");
                    Console.ReadKey();
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                Console.WriteLine("Another Applicaiton is Running!");
            }

            //Mutex的其他方法
            if(Mutex.TryOpenExisting(MutexName + "sdf", out Mutex result))
            {
                Console.WriteLine($"get mutex({MutexName}) succeed!");
            }
            else
            {
                Console.WriteLine($"get mutex({MutexName}) failed!");
            }
        }

        /// <summary>
        /// 限制同时访问同一资源的线程数量
        /// </summary>
        static async ValueTask UseSemaphoreSlimAsync()
        {
            //限制同时访问统一资源的线程数最大为4个
            using var semaphoreSlim = new SemaphoreSlim(4);

            async ValueTask AccessDataAsync(string name, CancellationToken? cancellationToken = null)
            {
                Console.WriteLine($"{name} wait to access data...");
                var granted = false;
                try
                {
                    // 可以通过token在等待期间进行取消
                    granted = await (cancellationToken.HasValue ? semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(10), cancellationToken.Value)
                        : semaphoreSlim.WaitAsync(TimeSpan.FromSeconds(10)));
                }
                catch(OperationCanceledException)
                {
                    Console.WriteLine($"{name} has been canceled!");
                    return;
                }

                if (granted)
                {
                    try
                    {
                        Console.WriteLine($"{name} has been granted permission to access data!");
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        Console.WriteLine($"{name} has completed!");
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                }
            }

            var tasks = new Task[7];
            for (int i = 0; i < 6; i++)
            {
                var name = $"Thread {i + 1}";
                tasks[i] = Task.Run(async () => await AccessDataAsync(name));
            }

            var threadName = "Thread 7";
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            token.Register(() => Console.WriteLine($"{threadName} has gone"));
            tasks[6] = Task.Run(async () => await AccessDataAsync("Thread 7", token));
            cts.CancelAfter(1);
            await Task.WhenAll(tasks);

            Console.WriteLine("all tasks have been compeleted!");
        }

        /// <summary>
        /// [内核模式]用于通知等待的线程有某件事发生（自动关门）
        /// Set-WaitOne一次后自动关门，即一次只能通过一个人
        /// </summary>
        /// <returns></returns>
        static async ValueTask UseAutoResetEventAsync()
        {
            // false: 关门 true: 开门
            using var workerAutoResetEvent = new AutoResetEvent(false);
            using var mainAutoResetEvent = new AutoResetEvent(false);
            var worker = Task.Run(() =>
            {
                Console.WriteLine("Worker: Waiting for worker set 1 ...");
                workerAutoResetEvent.WaitOne();

                mainAutoResetEvent.Set();
                Console.WriteLine("Workder: Main set 1!");

                Console.WriteLine("Worker: Waiting for worker set 2 ...");
                workerAutoResetEvent.WaitOne();

                Console.WriteLine("Worker: Done!");
            });

            Console.WriteLine("Main: Start...");

            workerAutoResetEvent.Set();
            Console.WriteLine("Main: Worker set 1!");

            Console.WriteLine("Main: Waiting for main set 1 ...");
            mainAutoResetEvent.WaitOne();

            workerAutoResetEvent.Set();
            Console.WriteLine("Main: Worker set 2!");

            Console.WriteLine("Main: Done!");

            await worker;
        }

        /// <summary>
        /// [混合模式]用于通知等待的线程有某件事发生（手动关门）
        /// Set-Reset间可以多次Wait，即一次可以通过多个人
        /// </summary>
        static async ValueTask UseManualResetEventSlimAsync()
        {
            // false: 关门 true: 开门
            using var manualResetEventSlim = new ManualResetEventSlim(false);
            var worker = Task.Run(() =>
            {
                Console.WriteLine("Worker: Waiting for set ...");

                manualResetEventSlim.Wait();
                Console.WriteLine("Workder: Passed 1!");

                manualResetEventSlim.Wait();
                Console.WriteLine("Workder: Passed 2!");

                Console.WriteLine("Worker: Done!");
            });

            Console.WriteLine("Main: Start...");

            manualResetEventSlim.Set();
            Console.WriteLine("Main: Set!");

            await Task.Delay(1000);

            manualResetEventSlim.Reset();
            Console.WriteLine("Main: Reset!");

            Console.WriteLine("Main: Done!");

            await worker;
        }

        /// <summary>
        /// 等待一定数量的操作完成再进行下一步操作
        /// </summary>
        /// <returns></returns>
        static async ValueTask UseCountDownEventAsync()
        {
            // 指定需要开门 5 次
            using var countDownEvent = new CountdownEvent(5);
            var task1 = Task.Run(() =>
            {
                Console.WriteLine("Task1: Waiting for other operation complete...");
                countDownEvent.Wait();
                Console.WriteLine("Task1 Done!");
            });

            var task2 = Task.Run(async () =>
            {
                await Task.Delay(1000);
                if (!countDownEvent.IsSet)
                {
                    Console.WriteLine("Task2: Waiting for other operation complete...");
                }
                countDownEvent.Wait();
                Console.WriteLine("Task2 Done!");
            });

            Console.WriteLine("Starting...");
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(100);
                countDownEvent.Signal();
                Console.WriteLine($"Signal {i + 1}");
            }

            await Task.WhenAll(task1, task2);
        }

        /// <summary>
        /// 组织多个线程在某个时刻碰面，并通过回调函数执行操作
        /// </summary>
        /// <returns></returns>
        static async ValueTask UseBarrierAsync()
        {
            // 回调函数多个波次之间是顺序执行的，如果在执行期间，
            // 发出Signal的线程数多于参与者数量时，会抛异常
            using var barrier = new Barrier(3, b =>
            {
                Console.WriteLine($"第 {b.CurrentPhaseNumber + 1} 波完成！共有 {b.ParticipantCount} 个线程参与！");
                // 无法在回调函数中调用该方法
                //b.RemoveParticipant();
                Console.WriteLine();
            });

            var task1 = Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Task1: {i + 1}");
                    Console.WriteLine($"还差 {barrier.ParticipantsRemaining} 个参与者");
                    // 如果在执行该语句时，发出Signal的数量超过指定的参与者数量时，会抛异常
                    barrier.SignalAndWait();
                }
            });

            var task2 = Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Task2: {i + 1}");
                    barrier.SignalAndWait();
                }
            });

            var task3 = Task.Run(async () =>
            {
                for (int i = 0; i < 3; i++)
                {
                    await Task.Delay(1000);
                    Console.WriteLine($"Task3: {i + 1}");
                    barrier.SignalAndWait();
                    if (barrier.ParticipantCount > 1)
                    {

                        Console.WriteLine($"下一波将减少1个参与者");
                        barrier.RemoveParticipant();
                    }
                }
            });

            await Task.WhenAll(task1, task2, task3);
            Console.WriteLine("Done!");
        }

        /// <summary>
        /// 共享读、独占写
        /// </summary>
        /// <returns></returns>
        static void UseReaderWriterLockSlim()
        {
            // 默认不允许递归加锁，使用本次设置可允许递归加锁
            using var rw = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

            // 纯读的操作只需要获取写锁即可
            try
            {
                // 尝试进入读锁，超时1s
                if (rw.TryEnterReadLock(1000))
                {
                    Console.WriteLine("进入读锁成功！");
                }
                else
                {
                    Console.WriteLine("进入读锁超时！");
                }
            }
            finally
            {
                if (rw.IsReadLockHeld)
                {
                    rw.ExitReadLock();
                    Console.WriteLine("已释放读锁！");
                }
            }

            // 可能需要写的操作最好先获取可升级读锁，判断的确需要写操作后，升级为写锁即可，可减少阻塞时间
            try
            {
                if (rw.TryEnterUpgradeableReadLock(1000))
                {
                    Console.WriteLine("进入可升级读锁成功！");
                    try
                    {
                        if (rw.TryEnterWriteLock(10000))
                        {
                            Console.WriteLine("进入写锁成功！");
                        }
                        else
                        {
                            Console.WriteLine("进入写锁超时！");
                        }
                    }
                    finally
                    {
                        if (rw.IsWriteLockHeld)
                        {
                            rw.ExitWriteLock();
                            Console.WriteLine("已释放写锁！");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("进入可升级读锁超时！");
                }
            }
            finally
            {
                if (rw.IsUpgradeableReadLockHeld)
                {
                    rw.ExitUpgradeableReadLock();
                    Console.WriteLine("已释放可升级读锁！");
                }
            }
        }

        /// <summary>
        /// [混合模式]先使用用户模式等待，再使用内核模式等待
        /// </summary>
        static async ValueTask UseSpinWaitAsync()
        {
            var spinWait = new SpinWait();
            var isCompleted = false;

            var task = Task.Run(() =>
            {
                while (!isCompleted)
                {
                    // 第 10 次旋转将进入内核模式
                    spinWait.SpinOnce();
                    Console.WriteLine($"已旋转 {spinWait.Count} 次");
                    Console.WriteLine($"下一次旋转会进入内核模式吗：{spinWait.NextSpinWillYield}");
                }

                Console.WriteLine("Done!");
            });

            await Task.Delay(1000);
            isCompleted = true;

            await task;
        }

        static void UseSpinLock()
        {
            var spinLock = new SpinLock();
        }
    }
}
