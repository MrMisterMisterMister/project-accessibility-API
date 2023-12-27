
using Application.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Controller handling user-related operations
    public class UsersController : BaseApiController
    {
        // Retrieves list of users
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            // Mediator sends a request to the handler responsible for getting users
            return await Mediator.Send(new GetUser.Query());
        }

        // Retrieves a user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            // Mediator sends a request to the handler responsible for getting a user by id
            return await Mediator.Send(new GetUserById.Query { Id = id });
        }

        // Creates a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            // Mediator sends a request to the handler responsible for creating a new user
            await Mediator.Send(new CreateUser.Command { User = user });

            return Ok(); // Placeholder reponse
        }

        // Deletes a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            // Mediator sends a request to the handler responsible for deleting a user by ID
            await Mediator.Send(new DeleteUser.Command { Id = id });

            return Ok(); // Placeholder response
        }

        // Updates/edit a user by ID, might add more options later
        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, User user)
        {
            user.Id = id; // Sets the ID in the user object
            // Mediator sends a request to the handler responsible for updating/editing a user by ID
            await Mediator.Send(new EditUser.Command { User = user });

            return Ok(); // Placeholder response
        }
    }
}