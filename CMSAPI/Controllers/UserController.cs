using CMSAPI.Interfaces;
using CMSAPI.Models.DTOs;
using CMSAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registrationModel">The registration details.</param>
        /// <response code="201">User registered successfully.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto registrationModel)
        {
            var result = await _userService.RegisterUserAsync(registrationModel.Username, registrationModel.Password);
            if (!result)
            {
                return BadRequest("Registration failed.");
            }

            return StatusCode(201);
        }
    }
}
