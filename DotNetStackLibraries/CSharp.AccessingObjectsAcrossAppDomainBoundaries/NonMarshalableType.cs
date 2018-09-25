using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.AccessingObjectsAcrossAppDomainBoundaries
{
    /// <summary>
    /// 该类的实例不能跨AppDomain边界进行封送
    /// </summary>
    public sealed class NonMarshalableType
    {
        public NonMarshalableType()
        {
            Console.WriteLine($"Executing in {AppDomain.CurrentDomain.FriendlyName}");
        }
    }
}
