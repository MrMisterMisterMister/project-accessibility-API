using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    public class CookiesController : BaseApiController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookiesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getAllCookies")]
        public IActionResult GetAllCookies()
        {
            // Access the HttpContext from the accessor
            var httpContext = _httpContextAccessor.HttpContext;

            // Get all cookies from the Request object
            var cookies = httpContext!.Request.Cookies;

            return Ok(cookies);
        }

        [HttpPost("removeCookie")]
        public IActionResult RemoveCookie([FromBody] string cookieName)
        {
            // Access the HttpContext from the accessor
            var httpContext = _httpContextAccessor.HttpContext;

            // Remove the specified cookie
            httpContext!.Response.Cookies.Delete(cookieName);

            return Ok("Cookie removed successfully");
        }
    }
}