using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tools.MediatR.Requests
{
    /// <summary>
    /// 实现 <see cref="IRequest"/> 的类均为Send（单播）的请求参数
    /// --无返回值
    /// <remark>
    /// <br/><see cref="IRequest"/> 实际上继承了 <c>IRequest{Unit}</c>，<see cref="Unit"/> 表示忽略返回值，这是为了简化执行管道而设计的
    /// </remark>
    /// </summary>
    public class OneWay : IRequest
    {

    }
}
