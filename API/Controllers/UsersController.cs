
using Application.UserHandlers;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetUser.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetActivity(Guid id)
        {
            return await Mediator.Send(new GetUserById.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await Mediator.Send(new CreateUser.Command { User = user });

            return Ok(); // will fix this later
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await Mediator.Send(new DeleteUser.Command { Id = id });

            return Ok(); // will fix this later
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