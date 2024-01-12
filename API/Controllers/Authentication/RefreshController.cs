using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using API.Services;
using Domain;
using Persistence;

namespace API.Controllers
{
    // inspirational quote here
    public class RefreshController : BaseApiController
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public RefreshController(TokenService tokenService, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _dataContext = dataContext;
        }

        // For expired bearer token
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RefreshBearerToken()
        {
            string? refreshTokenString = _httpContextAccessor.HttpContext?.Request?.Cookies?["refreshToken"];

            // Check if the refresh token is found in the cookie
            if (string.IsNullOrEmpty(refreshTokenString)) return BadRequest("The refresh token has not been provided.");

            // Get the refresh token
            RefreshToken? refreshToken = await _tokenService.GetRefreshTokenFromString(refreshTokenString);

            // Check if the refresh token exists
            if (refreshToken == null) return BadRequest("The refresh token could not be found.");

            // Check if the refresh token is invalid
            if (refreshToken.IsRevoked || refreshToken.Expires < DateTime.UtcNow)
            {
                return BadRequest("The refresh token is invalid.");
            }

            if (refreshToken != null)
            {
                // Get the user information based on the token
                User? user = await _dataContext.Users
                    .Include(x => x.RefreshToken)
                    .FirstOrDefaultAsync(x => x.RefreshToken.Token == refreshToken.Token);

                // If user is not found
                if (user == null) return BadRequest("Invalid refresh token");

                var roles = await _userManager.GetRolesAsync(user);

                return Ok(
                    new
                    {
                        Token = _tokenService.CreateAndSetCookie(user, roles.ToList(), refreshToken)
                    }
                );
            }

            // ۹( ÒہÓ )۶
            return BadRequest("Something went wrong.");
        }

        // new refresh token
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            // Get email from claim
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

            // Check if email claim is found
            if (emailClaim == null || string.IsNullOrEmpty(emailClaim.Value))
                return BadRequest("The email claim could not be found.");

            Console.WriteLine("Hello " +emailClaim.Value);


            // Get the user from database
            User? user = await _userManager.FindByEmailAsync(emailClaim.Value);

            // Check again if user is found
            if (user == null) return BadRequest("The user could not be found.");

            // Email
            if (user.Email == null)
                return BadRequest("Email is not found.");

            // Old refresh token
            RefreshToken? oldRefreshToken = await _tokenService.GetRefreshTokenFromUser(user.Email);

            // To check if there old refresh token is found
            if (oldRefreshToken == null) return BadRequest("There is no refresh token.");

            // Check if the refresh token is invalid
            if (oldRefreshToken.IsRevoked || oldRefreshToken.Expires < DateTime.UtcNow)
                return BadRequest("The refresh token is invalid.");

            // Get a new refresh token
            RefreshToken? newRefreshToken = _tokenService.GenerateRefreshToken();

            // Check if refresh token has been generated
            if (newRefreshToken == null) return BadRequest("The refresh token could not be generated.");

            // Set the refresh for the user
            user.RefreshToken = newRefreshToken;

            // Update the refresh token in database
            await _userManager.UpdateAsync(user);

            // Remove the old refresh token from database
            await _tokenService.RemoveRefreshToken(oldRefreshToken);

            // Roles
            var roles = await _userManager.GetRolesAsync(user);

            return Ok(
                _tokenService.CreateAndSetCookie(user, roles.ToList(), newRefreshToken)
            );
        }
    }
}
