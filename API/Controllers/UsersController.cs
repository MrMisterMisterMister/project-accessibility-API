using System.Security.Claims;
using Application.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return HandleResult(await Mediator.Send(new GetUser.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetUserById.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            return HandleResult(await Mediator.Send(new CreateUser.Command { User = user }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteUser.Command { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, User user)
        {
            user.Id = id.ToString();
            return HandleResult(await Mediator.Send(new EditUser.Command { User = user }));
        }

        // for testing purposes
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var cookie = Request.Cookies["userCookie"];

            var jwtToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result;

            var response = new
            {
                UserId = userId,
                Email = userEmail,
                Cookie = cookie,
                JwtToken = jwtToken
            };

            return Ok(response);
        }
    }
}