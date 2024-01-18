using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using API.Services;
using Domain;
using API.DTOs;

namespace API.Controllers
{
    // inspirational quote here
    public class RefreshController : BaseApiController
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public RefreshController(TokenService tokenService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [Authorize] // user needs to be authenticated
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            if (user == null) return Unauthorized(); // just in case, might never happen

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            // user can't refresh with an invalid token due to security reasons
            if (oldToken != null && !oldToken.IsActive) return Unauthorized();

            if (oldToken != null) oldToken.Revoked = DateTime.Now;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDTO { Token = await _tokenService.CreateAndSetCookie(user, roles.ToList()) };
        }
    }
}
