using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Services;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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
            return Ok();
        }
    }
}
