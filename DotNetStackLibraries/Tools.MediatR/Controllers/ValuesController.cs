using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
            await _mediator.Publish(new MessageNotification("我使用了MediatR！"));
            var result = await _mediator.Send(new PingRequest());

            return result;
        }
    }
}
