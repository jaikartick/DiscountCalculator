using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DiscountCalculator.DataAccess.Interfaces;
using DiscountController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DiscountCalculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<LoginController> _logger;
        private readonly AppSettings _appSettings;

        public LoginController(IUserRepository userRepository, ILogger<LoginController> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] UserCredential userCredential)
        {
            var user = await _userRepository.AuthenticateAsync(userCredential.Username, userCredential.Password);

            if (user == null)
            {
                string message = "Username or password is incorrect.";
                _logger.Log(LogLevel.Warning, message);
                return BadRequest(new { message = message });
            }
            var tokenDetail = Utilities.GetJwtTokenForUser(_appSettings, user.Id);
            return Ok(tokenDetail);
        }

    }
}
