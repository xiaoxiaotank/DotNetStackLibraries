using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.AutoMapper.ConsoleEx.ValueResolvers
{
    /// <summary>
    /// 值解析器是针对单一类型的
    /// </summary>
    class UserTotalMoneyValueResolver : IValueResolver<User, UserDto, int>
    {
        public int Resolve(User source, UserDto destination, int destMember, ResolutionContext context)
        {
            return source.Cash + source.Deposit;
        }
    }
}
