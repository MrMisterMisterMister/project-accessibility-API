using Application.UserHandlers;
using Domain;
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
            await Mediator.Send(new CreateUser.Command { User = user });

            return Ok();
        }

        // Deletes a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await Mediator.Send(new DeleteUser.Command { Id = id });

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, User user)
        {
            user.Id = id;
            await Mediator.Send(new EditUser.Command { User = user });

            return Ok();
        }
    }
}