using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp.AccessingObjectsAcrossAppDomainBoundaries
{
    /// <summary>
    /// 该类的实例可跨AppDomain的边界“按引用封送”
    /// </summary>
    public sealed class MarshalByRefType : MarshalByRefObject
    {
        public MarshalByRefType()
        {
            Console.WriteLine($"{GetType()} ctor running in {AppDomain.CurrentDomain.FriendlyName}");
        }

        public void SomeMethod()
        {
            Console.WriteLine($"Executing in {AppDomain.CurrentDomain.FriendlyName}");
        }

        public MarshalByValType MethodWithReturn()
        {
            Console.WriteLine($"Executing in {AppDomain.CurrentDomain.FriendlyName}");
            var t = new MarshalByValType();
            return t;
        }

        public NonMarshalableType MethodArgAndReturn(string callingDomainName)
        {
            Console.WriteLine($"calling from to {callingDomainName} to {AppDomain.CurrentDomain.FriendlyName}");
            var t = new NonMarshalableType();
            return t;
        }
    }
}
