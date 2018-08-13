using AspNetCore.MVC.AuthenticationAndAuthorization.Entities;
using AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Implements
{
    public class UserStore : IUserStore
    {
        private static readonly List<User> _userList = new List<User>()
        {
            new User
            {
                Id = 1,
                Name = "alice",
                Password = "alice",
                Email = "alice@qq.com",
                PhoneNumber = "18123456789"
            },
            new User
            {
                Id = 2,
                Name = "bob",
                Password = "bob",
                Email = "bob@qq.com",
                PhoneNumber = "13123456789"
            },
        };

        public User GetUser(string name, string password) =>
            _userList.FirstOrDefault(u => u.Name.Equals(name) && u.Password.Equals(password));
    }
}
