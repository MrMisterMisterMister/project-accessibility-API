using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace API.Controllers{
    [AllowAnonymous]
 // Allows access to the Login endpoints without authentication
    public class LoginController : BaseApiController
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginController> _logger;

        public LoginController(TokenService tokenService, UserManager<User> userManager, ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        // Authenticates the user using provided credentials and generates a token cookie
        [HttpPost]
       public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
{
    // Find user by email
    var user = await _userManager.FindByEmailAsync(loginDTO.Email);

    // Log user information (for debugging)
    _logger.LogInformation($"User found: {user?.Email}");

    // Return Unauthorized if user is not found
    if (user == null) 
    {
        _logger.LogWarning("User not found.");
        return Unauthorized(new
        {
            code = "UserNotFound",
            description = "No user found with the provided email address."
        });
    }

    // Check if password matches
    var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

    // Log authentication result (for debugging)
    _logger.LogInformation($"Authentication result: {result}");

    // Return a JWT token cookie if credentials are valid
    if (result)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return new UserDTO { Token = _tokenService.CreateAndSetCookie(user, roles.ToList()) };
    }

    // Return Unauthorized if the password is incorrect
    _logger.LogWarning("Incorrect password.");
    return Unauthorized(new
    {
        code = "IncorrectPassword",
        description = "Incorrect password. Please check your password and try again."
    });
}

        // For testing purposes: Retrieves user information including 
        // UserID, Email, Cookie, and JWT Token and roles
        [HttpGet("userinfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            // Extracts user Email from the authenticated user's claims
            // and finds it in the database  
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)!);

            var roles = await _userManager.GetRolesAsync(user!);

            // Retrieves the 'userCookie' from the HTTP request cookies
            var cookie = Request.Cookies["userCookie"];

            // Retrieves the JWT token from the HTTP context's authentication tokens
            var jwtToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result;

            // Constructs a response object with retrieved user information
            var response = new
            {
                UserId = user!.Id,
                Email = user.Email,
                Cookie = cookie,
                JwtToken = jwtToken,
                UserRoles = roles
            };

            return Ok(response); // Returns the constructed response as OK
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
            }, new List<string> { "Admin" });

            return Ok("Cookie Set!"); // Returns a success message when the cookie is set
        }
    }
}
