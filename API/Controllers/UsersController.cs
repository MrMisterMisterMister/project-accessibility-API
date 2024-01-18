using System.Security.Claims;
using Application.Handlers.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
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

        // Updates a user's email using DTO's, dunno why string arguments won't work
        [HttpPut("update-email")]
        public async Task<ActionResult> EditEmail(UpdateEmailDTO updateEmailDTO)
        {
            return HandleResult(await Mediator.Send(new EditUserEmail.Command
            {
                NewEmail = updateEmailDTO.NewEmail,
                ConfirmEmail = updateEmailDTO.ConfirmEmail,
                Password = updateEmailDTO.Password
            }));
        }

        // Updates a user's password using DTO's, dunno why string arguments won't work
        [HttpPut("update-password")]
        public async Task<ActionResult> EditPassword(UpdatePasswordDTO updatePasswordDTO)
        {
            return HandleResult(await Mediator.Send(new EditUserPassword.Command
            {
                CurrentPassword = updatePasswordDTO.CurrentPassword,
                NewPassword = updatePasswordDTO.NewPassword,
                ConfirmPassword = updatePasswordDTO.ConfirmPassword
            }));
        }

        // Get's the current user using the auth token... whack
        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> GetCurrentUser([FromServices] UserManager<User> userManager)
        {
            // Extracts user Email from the authenticated user's claims
            // and finds it in the database  
            var user = await userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email)!);

            if (user == null) return BadRequest(new { Code = "UserNotFound", Message = "User could not be found." });

            var roles = await userManager.GetRolesAsync(user!);

            // Retrieves the 'userCookie' from the HTTP request cookies
            var cookie = Request.Cookies["userCookie"];

            // Retrieves the JWT token from the HTTP context's authentication tokens
            var jwtToken = HttpContext.GetTokenAsync("Bearer", "access_token").Result;

            // Constructs a response object with retrieved user information
            var response = new
            {
                UserId = user!.Id,
                user.UserName,
                user.Email,
                Cookie = cookie,
                Token = jwtToken,
                UserRoles = roles
            };

            return Ok(response); // Returns the constructed response as OK
        }
    }
}
