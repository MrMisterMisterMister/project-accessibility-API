using API.DTOs;
using API.DTOs.RegisterDTOs;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers.Authentication
{
    [AllowAnonymous]
    public class GoogleSignInButtonController : BaseApiController
    {
        // on crack
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public GoogleSignInButtonController(UserManager<User> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost("google-signup")]
        public async Task<ActionResult<UserDTO>> GoogleSignUp(GoogleSignInDTO signInDTO)
        {
            string googleJWTToken = signInDTO.GoogleJWTToken;

            // Decode the JWT token to extract user information
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(googleJWTToken) as JwtSecurityToken;

            // Extract user email from the decoded JWT token
            var userEmail = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "email")?.Value;
            var userFirstName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "given_name")?.Value;
            var userLastName = jwtToken?.Claims?.FirstOrDefault(c => c.Type == "family_name")?.Value;

            if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userFirstName) || string.IsNullOrEmpty(userLastName))
            {
                return BadRequest("Invalid Google JWT token or missing required information.");
            }

            // Check if the email already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(userEmail);

            if (existingUser != null)
            {
                // User exists, generate a JWT token for them
                var roles = await _userManager.GetRolesAsync(existingUser);
                return new UserDTO { Token = _tokenService.CreateToken(existingUser, roles.ToList()) };
            }
            else
            {

                // If the email doesn't exist, create a new panelmember in the database
                var panelMember = new PanelMember
                {
                    Email = userEmail,
                    UserName = userEmail,
                    FirstName = userFirstName,
                    LastName = userLastName
                };

                var result = await _userManager.CreateAsync(panelMember);

                if (result.Succeeded)
                {
                    // User creation successful, generate a JWT token for the new user
                    var newRoles = await _userManager.GetRolesAsync(panelMember);
                    return new UserDTO { Token = _tokenService.CreateAndSetCookie(panelMember, newRoles.ToList()) };
                }

                // Return errors if user creation fails
                return BadRequest(result.Errors);
            }
        }
    }
}
