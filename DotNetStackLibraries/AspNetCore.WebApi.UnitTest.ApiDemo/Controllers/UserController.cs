using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.WebApi.UnitTests.ServiceDemo;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.WebApi.UnitTests.ApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userService.Get());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            return _userService.Get(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]User user)
        {
            _userService.Create(user);
        }

        // PUT api/values/5
        [HttpPatch("{id}")]
        public void Patch([FromBody]User user)
        {
            _userService.Update(user);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _userService.Delete(id);
        }
    }
}
