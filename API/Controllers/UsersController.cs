using System.Security.Claims;
using Application.Handlers.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

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
            var user = await userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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

        // Search for users by email, by name not implemented yet
        [Authorize]
        [HttpGet("search/{query}")]
        public async Task<IActionResult> SearchUsers(string query, [FromServices] UserManager<User> userManager, [FromServices] DataContext context)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }

            // Initial search in the Users table

            var users = await userManager.Users
                             .Where(u => (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(query)) ||
                                         (!string.IsNullOrEmpty(u.UserName) && u.UserName.Contains(query)))
                             .ToListAsync();

            var results = new List<object>();

            foreach (var user in users)
            {
                // Determine if the user is a PanelMember
                var panelMember = await context.PanelMembers.FindAsync(user.Id);
                if (panelMember != null)
                {
                    results.Add(new
                    {
                        Type = "PanelMember",
                        user.Id,
                        Name = $"{panelMember.FirstName} {panelMember.LastName}",
                        user.Email
                    });
                    continue;
                }

                // Determine if the user is a Company
                var company = await context.Companies.FindAsync(user.Id);
                if (company != null)
                {
                    results.Add(new
                    {
                        Type = "Company",
                        user.Id,
                        Name = company.CompanyName,
                        user.Email
                    });
                    continue;
                }

                // Default to user if not a PanelMember or Company
                results.Add(new
                {
                    Type = "User",
                    user.Id,
                    Name = user.UserName,
                    user.Email
                });
            }

            if (!results.Any())
            {
                return NotFound("No users found.");
            }

            return Ok(results);
        }

        [Authorize]
        [HttpGet("byEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email, [FromServices] UserManager<User> userManager, [FromServices] DataContext context)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be empty.");
            }

            // Find user by email
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if the user is a PanelMember
            var panelMember = await context.PanelMembers.FindAsync(user.Id);
            if (panelMember != null)
            {
                return Ok(new
                {
                    Type = "PanelMember",
                    user.Id,
                    Name = $"{panelMember.FirstName} {panelMember.LastName}",
                    user.Email
                });
            }

            // Check if the user is a Company
            var company = await context.Companies.FindAsync(user.Id);
            if (company != null)
            {
                return Ok(new
                {
                    Type = "Company",
                    user.Id,
                    Name = company.CompanyName,
                    user.Email
                });
            }

            // Default to user if not a PanelMember or Company
            return Ok(new
            {
                Type = "User",
                user.Id,
                Name = user.UserName,
                user.Email
            });
        }

    }
}
