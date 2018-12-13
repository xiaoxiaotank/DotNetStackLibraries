using AspNetCore.WebApi.UnitTests.ApiDemo.Controllers;
using AspNetCore.WebApi.UnitTests.ServiceDemo;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AspNetCore.WebApi.UnitTests.xUnit
{
    public class UserControllerTest : BaseControllerTest<UserController>
    {
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public void Get_ReturnUsers()
        {
            _mockUserService.Setup(s => s.Get()).Returns(GetUseSamples());

            var result = _controller.Get();

            var value = Assert.IsType<OkObjectResult>(result.Result).Value;
            var content = Assert.IsAssignableFrom<IEnumerable<User>>(value);
            Assert.NotNull(content);
            Assert.Equal(2, content.Count());
        }


        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Get_ReturnUser_WhenIdValid(int id)
        {
            _mockUserService.Setup(s => s.Get(id)).Returns(GetUseSamples().First(u => u.Id.Equals(id)));

            var result = _controller.Get(id);
            var content = result.Value;

            Assert.NotNull(content);
            Assert.NotNull(content);
            Assert.Equal(id, content.Id);
        }


        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void Get_ReturnNull_WhenIdInvalid(int id)
        {
            _mockUserService.Setup(s => s.Get(id)).Returns(GetUseSamples().FirstOrDefault(u => u.Id.Equals(id)));

            var result = _controller.Get(id);
            var content = result.Value;

            Assert.Null(content);
        }

        private IQueryable<User> GetUseSamples()
        {
            var list = new List<User>()
            {
                new User(){ Id = 1 },
                new User(){ Id = 2 },
            };
            return list.AsQueryable();
        }
    }
}
