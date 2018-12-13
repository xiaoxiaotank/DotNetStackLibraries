using AspNetCore.WebApi.UnitTests.ApiDemo.Controllers;
using AspNetCore.WebApi.UnitTests.ServiceDemo;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class UserControllerTest
    {
        [SetUp]
        public void Setup()
        {
            //��ÿһ�����Է���ִ��ǰ����Ҫִ��һ��
            
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Get_ReturnUser_WhenIdValid(int id)
        {
            var userService = new UserService();
            var controller = new UserController(userService);

            var result = controller.Get(id);

            Assert.IsInstanceOf(typeof(User), result);
        }
    }
}