using Application.Handlers.ExpertDisabilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ExpertDisabilitiesControllers : BaseApiController
    {
        [HttpPost("assign-disability/{id}")]
        public async Task<IActionResult> AddExpertDisability(int id)
        {
            return HandleResult(await Mediator.Send(new AddExpertDisability.Command { DisabilityId = id }));
        }

        [HttpDelete("remove-disability/{id}")]
        public async Task<IActionResult> RemoveExpertDisability(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteExpertDisability.Command { DisabilityId = id }));
        }

    }
}