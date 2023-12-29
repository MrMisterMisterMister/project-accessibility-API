using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GoogleSignInController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GoogleSignInController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("googleClientId")]
        public IActionResult GetGoogleClientId()
        {
            // Retrieve the Google Client ID from appsettings.json
            string clientId = _configuration.GetValue<string>("GoogleAuth:ClientId");

            if (string.IsNullOrEmpty(clientId))
            {
                return NotFound("Google Client ID not found");
            }

            return Ok(new { clientId });
        }
    }
}
