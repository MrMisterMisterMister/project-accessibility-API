using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GoogleSignInController : BaseApiController
    {
        private readonly IConfiguration _configuration;

        public GoogleSignInController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("googleClientId")]
        public IActionResult GetGoogleClientId()
        {
            string clientId = _configuration.GetValue<string>("GoogleAuth:ClientId");

            if (string.IsNullOrEmpty(clientId))
            {
                return NotFound("Google Client ID not found or invalid");
            }

            return Ok(new { clientId });
        }

    }
}
