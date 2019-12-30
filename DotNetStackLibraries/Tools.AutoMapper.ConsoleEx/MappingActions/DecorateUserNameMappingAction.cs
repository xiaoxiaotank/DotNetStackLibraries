using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx.MappingActions
{
    class DecorateUserNameMappingAction : IMappingAction<User, UserDto>
    {
        //在Asp .net core 环境下可以使用此方式获取HttpContext
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public DecorateUserNameMappingAction(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        //}

        public void Process(User source, UserDto destination)
        {
            source.Name = $"[{source.Name}]";
        }
    }
}
