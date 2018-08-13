using AspNetCore.MVC.AuthenticationAndAuthorization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Storages.Interfaces
{
    public interface IUserStore
    {
        User GetUser(string userName, string password);
    }
}
