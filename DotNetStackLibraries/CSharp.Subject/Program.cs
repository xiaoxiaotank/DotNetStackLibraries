using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
        static void Main(string[] args)
        {
            // Subject既相当于LocationReporter，又相当于LocationTracker
            // 即Subject既是观察者(observer)，又是被观察者(observable)
            var subject = new Subject<string>();

            var subscriber = subject
                .ObserveOn(Scheduler.Default)
                // 在紧跟的Subscribe Action执行前先执行Do Action
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

        }
    }
}
