using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseApiController
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public LoginController(TokenService tokenService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // on crack
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) return Unauthorized(
                new
                {
                    code = "UserNotFound",
                    description = "No user found with the provided email address."
                }
            );

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (result) return new UserDTO { Token = _tokenService.CreateAndSetCookie(user) };

            return Unauthorized(
                new
                {
                    code = "IncorrectPassword",
                    description = "Incorrect password. Please check your password and try again."
                }
            );
        }

        // for testing purposes
        [HttpGet("set-cookie")]
        public IActionResult SetCookie()
        {
            _tokenService.CreateAndSetCookie(new User
            {
                Email = "test@dad.com",
                Id = Guid.NewGuid().ToString(),
            });

            return Ok("Cookie Set!");
        }
    }
}