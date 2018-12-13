using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.WebApi.UnitTests.ServiceDemo
{
    public interface IUserService
    {
        IQueryable<User> Get();

        User Get(int id);

        User Create(User user);

        void Update(User user);

        void Delete(int id);
    }
}
