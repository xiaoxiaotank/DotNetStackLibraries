using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.AccessingObjectsAcrossAppDomainBoundaries
{
    /// <summary>
    /// 该类的实例可以跨AppDomain的边界“按值封送”
    /// </summary>
    [Serializable]
    public sealed class MarshalByValType
    {
        private DateTime _creationTime = DateTime.Now;

        public MarshalByValType()
        {
            Console.WriteLine($"{GetType()} ctor running in {AppDomain.CurrentDomain.FriendlyName}, Created on {_creationTime : D}");
        }

        public override string ToString()
        {
            return _creationTime.ToLongDateString();
        }
    }
}
