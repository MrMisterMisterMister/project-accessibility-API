using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;

        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor, DataContext dataContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _dataContext = dataContext;
        }

        // Generates a JWT token based on the provided user details
        public string CreateToken(User user, List<string> roles)
        {
            // Claims represent the user's identity information
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Key to sign the token using a secret key stored in configuration
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]!));

            // Credentials used to sign the token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Token descriptor contains the claims, expiry, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30), // Token expiration time
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            // Create the token based on the token descriptor
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the written token
            return tokenHandler.WriteToken(token);
        }

        // Creates a JWT token and sets it as a cookie in the HTTP response
        public string CreateAndSetCookie(User user, List<string> roles, RefreshToken refreshToken)
        {
            // Create a JWT token
            var jwtToken = CreateToken(user, roles);

            // Cookie options for the JWT token
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Restricts cookie access to HTTP requests
                Secure = true,   // Requires HTTPS to send the cookie
                SameSite = SameSiteMode.None, // Allows cross-site requests
                Expires = DateTime.UtcNow.AddMinutes(30) // Cookie expiration time
            };

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Set the cookie named 'userCookie' in the HTTP response
                httpContext.Response.Cookies.Append("userCookie", jwtToken, cookieOptions);
                httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
            }

            return jwtToken; // Return the generated JWT token
        }

        // Generating refresh token
        public RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(1)), // Should probably put this somewhere else
                LastModified = DateTime.UtcNow
            };
        }

        // Get refresh token with email address from user
        public async Task<RefreshToken?> GetRefreshTokenFromUser(string email)
        {
            // Look up the token from user
            User? userToken = await _dataContext.Users
                .Include(x => x.RefreshToken)
                .FirstOrDefaultAsync(x => x.Email == email);

            // If there is a token with user
            if (userToken != null)
            {
                return userToken.RefreshToken;
            }

            // null
            return null;
        }

        // Get refresh token with the string
        public async Task<RefreshToken?> GetRefreshTokenFromString(string token)
        {
            RefreshToken? refreshToken = await _dataContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            return refreshToken;
        }

        // Removing the refresh token from database
        public async Task RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dataContext.RefreshTokens.Remove(refreshToken);
            await _dataContext.SaveChangesAsync();
        }
    }
}
