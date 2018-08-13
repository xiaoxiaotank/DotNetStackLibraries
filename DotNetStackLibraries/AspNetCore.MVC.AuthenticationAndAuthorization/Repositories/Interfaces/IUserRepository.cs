using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCore.MVC.AuthenticationAndAuthorization.Repositories.Interfaces
{
    interface IUserRepository
    {
        bool ValidateLastChanged(ClaimsPrincipal principal, string lastChanged);
    }
}
