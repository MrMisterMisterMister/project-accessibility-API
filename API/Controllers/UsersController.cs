using Application.UserHandlers;
using Domain;
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
    }
}
