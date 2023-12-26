
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
            return await Mediator.Send(new Read.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetActivity(Guid id)
        {
            return await Mediator.Send(new ReadId.Query { Id = id });
        }

        // TODO create handlers for post, put and delete
    }
}