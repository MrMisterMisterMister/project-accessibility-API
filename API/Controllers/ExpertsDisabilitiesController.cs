using Application.Handlers.ExpertDisabilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ExpertsDisabilitiesController : BaseApiController
    {
        [HttpPost("add-disability/{disabilityId}")]
        public async Task<IActionResult> AddDisabilityToExpert(int disabilityId)
        {
            return HandleResult(await Mediator.Send(new AddExpertDisability.Command { DisabilityId = disabilityId }));
        }

        [HttpDelete("remove-disability/{disabilityId}")]
        public async Task<IActionResult> RemoveDisabilityFromExpert(int disabilityId)
        {
            return HandleResult(await Mediator.Send(new DeleteExpertDisability.Command { DisabilityId = disabilityId }));
        }
    }
}