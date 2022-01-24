using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountCalculator.DomainModel.Models;
using LoginController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DiscountCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignupController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginController> _logger;
        private readonly AppSettings _appSettings;

        public SignupController(IUserRepository userRepository, ILogger<LoginController> logger, IOptions<AppSettings> appSettings)
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
            return Created(new Uri($"{Request.Path}/{user.Id}", UriKind.Relative), tokenDetail);
        }

    }
}
