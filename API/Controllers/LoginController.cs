using Microsoft.AspNetCore.Mvc;
using Domain;

namespace API.Controllers
{
    public class LoginController : BaseApiController
    {
        //
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            return Ok("your mom");
        }
    }
}