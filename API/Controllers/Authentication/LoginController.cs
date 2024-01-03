using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [AllowAnonymous] // Allows access to the Login endpoints without authentication
    public class LoginController : BaseApiController
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public LoginController(TokenService tokenService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        // Authenticates the user using provided credentials and generates a token cookie
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            // Return Unauthorized if user is not found
            if (user == null) return Unauthorized(
                new
                {
                    code = "UserNotFound",
                    description = "No user found with the provided email address."
                }
            );

            // Check if password matches
            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            // Return a JWT token cookie if credentials are valid
            if (result) return new UserDTO { Token = _tokenService.CreateAndSetCookie(user) };

            // Return Unauthorized if the password is incorrect
            return Unauthorized(
                new
                {
                    code = "IncorrectPassword",
                    description = "Incorrect password. Please check your password and try again."
                }
            );
        }

        // Endpoint to set a test cookie (for demonstration or testing purposes)
        [HttpGet("set-cookie")]
        public IActionResult SetCookie()
        {
            // Create and set a token cookie for a test user
            _tokenService.CreateAndSetCookie(new User
            {
                Email = "test@dad.com",
                Id = Guid.NewGuid().ToString(),
            });

            return Ok("Cookie Set!"); // Returns a success message when the cookie is set
        }
    }
}
