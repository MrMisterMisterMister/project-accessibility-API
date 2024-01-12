using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
            if (result)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(user, roles.ToList()) };
            }
            // Return Unauthorized if the password is incorrect
            return Unauthorized(
                new
                {
                    code = "IncorrectPassword",
                    description = "Incorrect password. Please check your password and try again."
                }
            );
        }

        [HttpPost("google")]
        public async Task<ActionResult<UserDTO>> GoogleSignUp(LoginGoogleDTO loginGoogleDTO)
        {
            // Check if login dto for google has all the fields
            if (loginGoogleDTO == null || string.IsNullOrEmpty(loginGoogleDTO.Email) || string.IsNullOrEmpty(loginGoogleDTO.Name) || string.IsNullOrEmpty(loginGoogleDTO.FirstName) || string.IsNullOrEmpty(loginGoogleDTO.LastName))
            {
                return BadRequest("Invalid or missing information in the request.");
            }

            // Check if the email already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(loginGoogleDTO.Email);

            if (existingUser != null)
            {
                // User exists, generate a JWT token for them
                var roles = await _userManager.GetRolesAsync(existingUser);
                return new UserDTO { Token = _tokenService.CreateAndSetCookie(existingUser, roles.ToList()) };
            }
            else
            {
                // If the email doesn't exist, create a new panelmember in the database
                var panelMember = new PanelMember
                {
                    Email = loginGoogleDTO.Email,
                    UserName = loginGoogleDTO.Email, // Just set email as username too
                    FirstName = loginGoogleDTO.FirstName,
                    LastName = loginGoogleDTO.LastName
                };

                var result = await _userManager.CreateAsync(panelMember);

                if (result.Succeeded)
                {
                    // User creation successful, generate a JWT token for the new user
                    // Give them admin.. for now.... :O... don't let mommy "T" know
                    var newRoles = new List<string>() { nameof(RoleTypes.Admin) };
                    await _userManager.AddToRolesAsync(panelMember, newRoles);

                    return new UserDTO { Token = _tokenService.CreateAndSetCookie(panelMember, newRoles.ToList()) };
                }

                // Return errors if user creation fails
                return BadRequest(result.Errors);
            }
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
