using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace CSharp.Subject
{
    /// <summary>
    /// 与项目 CSharp.Observer 一起食用
    /// </summary>
    class Program
    {
        /// <summary>
        /// 本例会
        ///     输出三次1（因为共三次OnNext并在一开始就创建了一个接收全部通知的订阅）
        ///     输出一次2（因为创建了仅接收字符串“jjj”的订阅后仅出现了一次符合条件的OnNext）
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            // Subject既相当于LocationReporter，又相当于LocationTracker
            // 即Subject既是观察者(observer)，又是被观察者(observable)
            var subject = new Subject<string>();

            var subscriber = subject
                .ObserveOn(Scheduler.Default)
                // 在紧跟的Subscribe Action执行前先执行Do Action（与subscribe的区别是返回值不是Disposable的对象）
                .Do(value => Console.WriteLine(1))
                // 作为观察者：设置当接收到通知(OnNext)时的行为
                .Subscribe(value =>
                {
                    Console.WriteLine($"该订阅处理所有类型的通知：{value}");
                });
            // 作为被观察者：发送通知
            subject.OnNext("发送第一条通知");

            // 仅订阅特定通知中包含字符串“jjj”的通知
            subject
                .Where(value => value.Contains("jjj"))
                .Do(value => Console.WriteLine(2))
                .Subscribe(value => Console.WriteLine($"该订阅仅处理包含字符串\"jjj\"的通知：{value}"));
            subject.OnNext("jjj：test");
            subject.OnNext("kkk: test");

            var user = new User() { Age = 1 };
            // 启动一个定时器定时去执行后方的一系列操作
            var subscriber2 = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1))
                .Select(_ => 
                {
                    //throw new Exception();
                    return user.Age <= 3;
                })
                // 这里使用TakeWhile是用来确定Observable，如果为空则直接执行onCompleted，因为没有可观察的对象
                .TakeWhile(v => v)
                // 这里的Where表示当Observable中为true时执行onNext
                .Where(v => v)
                // 抛出异常时，继续去处理下一个Observable
                .Catch(Observable.Empty<bool>())
                .Finally(() =>
                {
                    Console.WriteLine($"当 subscriber2.Dispose 或 抛异常 时执行 Finaly");
                })
                .Subscribe(
                    onNext: _ => 
                    { 
                        Console.WriteLine("当 Select 中 条件为 true 时执行 onNext，因为 Where 中只选择了为 true的"); 
                    }, 
                    onCompleted: () =>
                    {
                        Console.WriteLine("所有的 Observable 处理完成后执行 Completed，同时Timer结束");
                    }
                );
            await Task.Delay(1000);
            user.Age = 2;
            await Task.Delay(1000);
            user.Age = 4;
            await Task.Delay(1000);
            user.Age = 1;

            //subscriber2.Dispose();
            Console.ReadKey();
        }

        class User
        {
            public int Age { get; set; }
        }
    }
}
