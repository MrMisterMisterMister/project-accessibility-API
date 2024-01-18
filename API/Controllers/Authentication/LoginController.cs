using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    // Allows access to the Login endpoints without authentication
    [AllowAnonymous]
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
                return new UserDTO { Token = await _tokenService.CreateAndSetCookie(user, roles.ToList()) };
            }

            _logger.LogWarning("Incorrect password.");

            // Return Unauthorized if the password is incorrect
            return Unauthorized(new
            {
                code = "IncorrectPassword",
                description = "Incorrect password. Please check your password and try again."
            });
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

            // Check user exists
            if (existingUser != null)
            {
                // User exists, generate a JWT token for them
                var roles = await _userManager.GetRolesAsync(existingUser);
                return new UserDTO { Token = await _tokenService.CreateAndSetCookie(existingUser, roles.ToList()) };
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
                    // Give them admin.. for now.... :O... don't let mommy "T" know .. ⁀⊙﹏☉⁀ I saw it. (⊙_◎)
                    await _userManager.AddToRoleAsync(panelMember, nameof(RoleTypes.PanelMember));

                    var roles = await _userManager.GetRolesAsync(panelMember);
                    return new UserDTO { Token = await _tokenService.CreateAndSetCookie(panelMember, roles.ToList()) };
                }

                // Return errors if user creation fails
                return BadRequest(result.Errors);
            }
        }
    }
}
