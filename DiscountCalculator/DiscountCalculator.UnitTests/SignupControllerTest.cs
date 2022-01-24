using System;
using System.Threading.Tasks;
using DiscountCalculator.Controllers;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;

namespace DiscountCalculator.UnitTests
{
    [TestFixture]
    public class SignupControllerTest
    {
        private SignupController _signupController;
        private IUserRepository _mockedUserRepository;
        private ILogger<SignupController> _mockedLogger;
        private IOptions<AppSettings> _appSettings;

        private User user = new User() { Email = "karthick@gmail.com", Username = "karthick", Id = Guid.NewGuid() };

        [SetUp]
        public void CreateConverter()
        {
            _mockedUserRepository = Substitute.For<IUserRepository>();
            _mockedLogger = Substitute.For<ILogger<SignupController>>();
            _appSettings = Options.Create<AppSettings>(new AppSettings() { Secret = "MyPasswordIsPassword" });
            _mockedUserRepository.GetAsync(Arg.Any<Guid>()).Returns(user);
            _signupController = new SignupController(_mockedUserRepository, _mockedLogger, _appSettings);
        }

        [Test]
        public async Task PostUserTest_ValidUser_OkResult()
        {
            _mockedUserRepository.IsUserEmailAlreadyExists(Arg.Is<string>("karthick@gmail.com")).Returns(false);
            _mockedUserRepository.IsUsernameAlreadyExists(Arg.Is<string>("karthick")).Returns(false);
            var actual = await _signupController.PostUser(new RegisterModel() { Email = "karthick@gmail.com", Username = "karthick", Password = "karthick" });
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(OkObjectResult), actual);
            var result = ((OkObjectResult)actual).Value;
            Assert.IsInstanceOf(typeof(UserToken), result);
            Assert.AreEqual(user.Id, ((UserToken)result).UserId);
        }

        [Test]
        public async Task PostUserTest_UserExists_BadRequest()
        {
            _mockedUserRepository.IsUserEmailAlreadyExists(Arg.Is<string>("karthick@gmail.com")).Returns(true);
            _mockedUserRepository.IsUsernameAlreadyExists(Arg.Is<string>("karthick")).Returns(true);
            var actual = await _signupController.PostUser(new RegisterModel() { Email = "karthick@gmail.com", Username = "karthick", Password = "karthick" });
            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(typeof(Microsoft.AspNetCore.Mvc.BadRequestObjectResult), actual);
        }
    }
}
