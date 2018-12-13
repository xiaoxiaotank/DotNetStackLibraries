using AspNetCore.WebApi.UnitTests.ApiDemo.Controllers;
using AspNetCore.WebApi.UnitTests.ServiceDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.WebApi.UnitTests.MSTest
{
    [TestClass]
    public class UserControllerTest
    {
        [DataTestMethod]
        [DataRow(1)]
        public void Get_ReturnUser_WhenIdValid(int id)
        {
            var userService = new UserService();
            var controller = new UserController(userService);

            var result = controller.Get(id);

            Assert.IsInstanceOfType(result, typeof(User));
        }
    }
}
