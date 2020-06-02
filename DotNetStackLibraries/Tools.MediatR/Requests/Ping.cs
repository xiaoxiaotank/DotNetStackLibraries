using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediatR
{
    /// <summary>
    /// 实现 <see cref="IRequest{T}"/> 的类均为Send（单播）的请求参数
    /// 有返回值
    /// </summary>
    public class Ping : IRequest<string>
    {

    }
}
