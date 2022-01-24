using System;
using System.Threading.Tasks;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DiscountCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<SignupController> _logger;
        private readonly AppSettings _appSettings;

        public SignupController(IUserRepository userRepository, ILogger<SignupController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] RegisterModel registerModel)
        {
            var isUsernameAlreadyExist = await _userRepository.IsUserEmailAlreadyExists(registerModel.Email);
            if (isUsernameAlreadyExist)
            {
                string message = "Username already exists.";
                _logger.Log(LogLevel.Warning, message);
                return BadRequest(new { message = message });
            }
            var isEmailAlreadyExist = await _userRepository.IsUserEmailAlreadyExists(registerModel.Email);
            if (isEmailAlreadyExist)
            {
                string message = "Email already exists.";
                _logger.Log(LogLevel.Warning, message);
                return BadRequest(new { message = message });
            }
            var userId = Guid.NewGuid();
            var user = new User()
            {
                Id = userId,
                Email = registerModel.Email,
                Username = registerModel.Username,
                Password = registerModel.Password,
                FirstName = string.Empty,
                LastName = string.Empty,
                CreatedBy = userId,
                LastModifiedBy = userId,
                CreatedOn = DateTime.UtcNow,
                LastModifiedOn = DateTime.UtcNow,
            };
            await _userRepository.AddAsync(user);
            await _userRepository.CommitAsync();
            user = await _userRepository.GetAsync(user.Id);
            var tokenDetail = Utilities.GetJwtTokenForUser(_appSettings, user.Id);
            return Ok(tokenDetail);
        }

    }
}
