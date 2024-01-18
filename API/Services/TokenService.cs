using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<string> CreateAndSetCookie(User user, List<string> roles)
        {
            // Create a JWT token
            var jwtToken = CreateToken(user, roles);

            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);

            // Save changes to the database
            await _dataContext.SaveChangesAsync();

            // Cookie options
            var cookieOptionsJwt = CreateCookieOptions(DateTime.UtcNow.AddMinutes(30)); // same as jwt token
            var cookieOptionsRefresh = CreateCookieOptions(DateTime.UtcNow.AddDays(7)); // same as refresh token

            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext != null)
            {
                // Set the cookie named 'userCookie' in the HTTP response
                httpContext.Response.Cookies.Append("userCookie", jwtToken, cookieOptionsJwt);
                httpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptionsRefresh);
            }

            return jwtToken; // Return the generated JWT token
        }

        // Create cookie options
        public CookieOptions CreateCookieOptions(DateTime expireDate)
        {
            // Cookie options for the JWT token
            return new CookieOptions
            {
                HttpOnly = true, // Restricts cookie access to HTTP requests and not JS
                Secure = true,   // Requires HTTPS to send the cookie
                SameSite = SameSiteMode.None, // Allows cross-site requests
                Expires = expireDate // Cookie expiration time
            };
        }

        // Generating refresh token
        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create(); // for security reasons
            rng.GetBytes(randomNumber);

            return new RefreshToken { Token = Convert.ToBase64String(randomNumber) };
        }

        // Unused method for now
        public async Task<List<RefreshToken>> GetRefreshTokensFromUser(string email)
        {
            // Look up the tokens from the user
            User? user = await _dataContext.Users
                .Include(x => x.RefreshTokens)
                .FirstOrDefaultAsync(x => x.Email == email);

            // If there is a user with tokens
            if (user != null)
            {
                // Return the collection of refresh tokens
                return user.RefreshTokens.ToList();
            }

            // If no user found or no tokens associated, return an empty list
            return new List<RefreshToken>();
        }

        // Get refresh token with the string
        // Unused method for now
        public async Task<RefreshToken?> GetRefreshTokenFromString(string token)
        {
            RefreshToken? refreshToken = await _dataContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token);

            return refreshToken;
        }

        // Removing the refresh token from database
        // Unused method for now
        public async Task RemoveRefreshToken(RefreshToken refreshToken)
        {
            _dataContext.RefreshTokens.Remove(refreshToken);
            await _dataContext.SaveChangesAsync();
        }
    }
}
