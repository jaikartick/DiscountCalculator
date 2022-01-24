using System;
using System.Threading.Tasks;
using DiscountCalculator.Controllers;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DiscountCalculator.UnitTests
{
    [TestFixture]
    public class LoginControllerTest
    {
        private LoginController _loginController;
        private IUserRepository _mockedUserRepository;
        private ILogger<LoginController> _mockedLogger;
        private IOptions<AppSettings> _appSettings;

        [SetUp]
        public void CreateConverter()
        {
            _mockedUserRepository = Substitute.For<IUserRepository>();
            _mockedLogger = Substitute.For<ILogger<LoginController>>();
            _appSettings = Options.Create<AppSettings>(new AppSettings() { Secret = "MyPasswordIsPassword" });
            var user = new User() { Email = "karthick@gmail.com", Username = "karthick", Id = Guid.NewGuid() };
            _mockedUserRepository.AuthenticateAsync(Arg.Is<string>("karthick"), Arg.Is<string>("karthick")).Returns(user);
            _loginController = new LoginController(_mockedUserRepository, _mockedLogger, _appSettings);
        }

        [Test]
        public async Task AuthenticateUserTest_ValidPassword_OkResult()
        {
            var actual = await _loginController.AuthenticateUser(new UserCredential() { Username = "karthick", Password = "karthick" });
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult), actual);
        }

        [Test]
        public async Task AuthenticateUserTest_InvalidPassword_BadRequest()
        {
            var actual = await _loginController.AuthenticateUser(new UserCredential() { Username = "karthick", Password = "invalidPassword" });
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(Microsoft.AspNetCore.Mvc.BadRequestObjectResult), actual);
        }
    }
}
