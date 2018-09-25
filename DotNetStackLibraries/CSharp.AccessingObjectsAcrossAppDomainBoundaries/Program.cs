using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.AccessingObjectsAcrossAppDomainBoundaries
{
    class Program
    {
        static void Main(string[] args)
        {
            Marshalling();

            Console.ReadKey();
        }

        /// <summary>
        /// 封送
        /// </summary>
        private static void Marshalling()
        {
            //获取当前应用程序域，等同于AppDomain.CurrentDomain
            var callingAd = Thread.GetDomain(); 
            var callingDomainName = callingAd.FriendlyName;
            Console.WriteLine($"Default AppDomain's friendly name:{callingDomainName}");

            //获取当前应用程序域中包含“Main”方法的程序集全名
            var exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine($"Main assembly:{exeAssembly}");

            AppDomain anotherAd = null;

            #region Demo1：使用按引用封送进行跨AppDomain通信，性能上会有一部分开销，尽量少用
            Console.WriteLine($"{Environment.NewLine}Demo #1");
            
            //将当前程序集加载到新建的AppDomain中，构造一个对象并封送会当前程序集（实际上得到对一个代理的引用）
            anotherAd = AppDomain.CreateDomain("Another AD");
            var mbrt = (MarshalByRefType)anotherAd.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            //CLR在类型上撒谎了，应该是个代理
            Console.WriteLine($"Type={mbrt.GetType()}");
            //证明得到的是对一个代理对象的引用
            Console.WriteLine($"Is proxy? {RemotingServices.IsTransparentProxy(mbrt)}");

            //看起来像是在MarshalByRefType上调用方法，
            //实则是我们在代理类型上调用方法，代理使线程切换到拥有对象的那个AppDomain（这里是AnotherAd）,并在真实的对象上调用该方法
            mbrt.SomeMethod();

            //由于AppDomain被卸载，代理找不到真实对象，遂失败
            AppDomain.Unload(anotherAd);
            try
            {
                mbrt.SomeMethod();
                Console.WriteLine("Successful call.");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Fail call.");
            }
            #endregion

            #region Demo2: 使用按值封送进行跨AppDomain通信
            Console.WriteLine($"{Environment.NewLine}Demo #2");

            anotherAd = AppDomain.CreateDomain("Another AD");
            mbrt = (MarshalByRefType)anotherAd.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            //对象的方法返回所返回对象的副本，对象按值封送，即得到的是一个真是的对象
            var mbvt = mbrt.MethodWithReturn();
            //证明得到的不是对一个代理对象的引用
            Console.WriteLine($"Is proxy? {RemotingServices.IsTransparentProxy(mbvt)}");
            //实际上就是在 MarshalByValType上调用方法
            Console.WriteLine($"Returned object created {mbvt}");

            //mbvt是一个真实的对象，而不是代理，与AnotherAd没关系，所以不会报错
            AppDomain.Unload(anotherAd);
            try
            {
                Console.WriteLine($"Returned object Created {mbvt}");
                Console.WriteLine($"Successfull call.");
            }
            catch (AppDomainUnloadedException)
            {
                Console.WriteLine("Failed call.");
            }

            #endregion

            #region Demo3：使用不可封送的类型进行跨AppDomain通信
            Console.WriteLine($"{Environment.NewLine} Demo #3");

            anotherAd = AppDomain.CreateDomain("Another AD");
            mbrt = (MarshalByRefType)anotherAd.CreateInstanceAndUnwrap(exeAssembly, typeof(MarshalByRefType).FullName);

            //对象的方法返回一个不可封送的对象，抛出异常
            var nmt = mbrt.MethodArgAndReturn(callingDomainName);
            //这里永远执行不到。。
            #endregion

        }
    }
}
