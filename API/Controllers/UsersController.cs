using System.Security.Claims;
using Application.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        // Will add more specific comments later
        // meh
        // Retrieves all users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return HandleResult(await Mediator.Send(new GetUser.Query()));
        }

        // Retrieves a specific user by their ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetUserById.Query { Id = id }));
        }

        // Creates a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            return HandleResult(await Mediator.Send(new CreateUser.Command { User = user }));
        }

        // Deletes a user by their ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteUser.Command { Id = id }));
        }

        // Updates a user's details by their ID
        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, User user)
        {
            user.Id = id.ToString();
            return HandleResult(await Mediator.Send(new EditUser.Command { User = user }));
        }

        // For testing purposes: Retrieves user information including UserID, Email, Cookie, and JWT Token
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            // Extracts UserID and Email from the authenticated user's claims
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            // Retrieves the 'userCookie' from the HTTP request cookies
            var cookie = Request.Cookies["userCookie"];

            // Retrieves the JWT token from the HTTP context's authentication tokens
            var jwtToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result;

            // Constructs a response object with retrieved user information
            var response = new
            {
                UserId = userId,
                Email = userEmail,
                Cookie = cookie,
                JwtToken = jwtToken
            };

            return Ok(response); // Returns the constructed response as OK
        }
    }
}
