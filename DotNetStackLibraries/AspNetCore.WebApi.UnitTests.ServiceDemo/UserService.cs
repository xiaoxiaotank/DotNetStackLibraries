using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNetCore.WebApi.UnitTests.ServiceDemo
{
    public class UserService : IUserService
    {
        public User Create(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<User> Get()
        {
            return Enumerable.Empty<User>().AsQueryable();
        }

        public User Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}
