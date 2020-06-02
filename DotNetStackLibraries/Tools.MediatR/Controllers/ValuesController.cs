using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tools.MediatR.Notifications;
using Tools.MediatR.Requests;

namespace Tools.MediatR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ValuesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<string> Get()
        {
            // 默认的发布策略是按照顺序一个一个的顺序执行（即使中间有Handler报错了，下一个也会执行）
            await _mediator.Publish(new PublishMessage("我使用了MediatR！"));

#warning 父类PublishMessage的Handler以及AnythingHandler都不会进行处理
            await _mediator.Publish<PublishMessage>(new SubPublishMessage("Sub：我使用了MediatR！"));

            // 单播 & 无返回值
            await _mediator.Send(new OneWay());

            // 单播 & 有返回值
            var result = await _mediator.Send(new Ping());

            return result;
        }
    }
}
