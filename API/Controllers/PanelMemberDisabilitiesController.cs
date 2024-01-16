using Application.Handlers.PanelMemberDisabilityHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PanelMemberDisabilitiesController : BaseApiController
    {
        [HttpPost("add-disability/{disabilityId}")]
        public async Task<IActionResult> AddDisabilityToPanelMember(int disabilityId)
        {
            return HandleResult(await Mediator.Send(new AddPanelMemberDisability.Command { DisabilityId = disabilityId }));
        }

        [HttpDelete("remove-disability/{disabilityId}")]
        public async Task<IActionResult> RemoveDisabilityFromPanelMember(int disabilityId)
        {
            return HandleResult(await Mediator.Send(new DeletePanelMemberDisability.Command { DisabilityId = disabilityId }));
        }
    }
}