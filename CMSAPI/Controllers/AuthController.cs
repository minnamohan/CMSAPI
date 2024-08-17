using CMSAPI.Interfaces;
using CMSAPI.Models.DTOs;
using CMSAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;       

        public AuthController(IAuthService authService)
        {
            _authService = authService;            
        }        

        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginModel">The login details.</param>
        /// <response code="200">Returns the JWT token.</response>
        /// <response code="401">If the login credentials are invalid.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto loginModel)
        {
            var token = await _authService.AuthenticateAsync(loginModel.Username, loginModel.Password);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }
    }
}
