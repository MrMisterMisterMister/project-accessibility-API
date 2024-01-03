using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        // Generates a JWT token based on the provided user details
        public string CreateToken(User user)
        {
            // Claims represent the user's identity information
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };

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
        public string CreateAndSetCookie(User user)
        {
            // Create a JWT token
            var jwtToken = CreateToken(user);

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
                // Set the cookie named 'userCookie' in the HTTP response
                httpContext.Response.Cookies.Append("userCookie", jwtToken, cookieOptions);

            return jwtToken; // Return the generated JWT token
        }
    }
}
