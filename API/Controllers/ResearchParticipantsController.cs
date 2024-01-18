using Application.ParticipantsHandlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(Policy = "PanelMemberPolicy")]
    public class ResearchParticipantsController : BaseApiController
    {
        [HttpPost("join-research/{researchId}")]
        public async Task<IActionResult> AddResearchParticipant(int researchId)
        {
            return HandleResult(await Mediator.Send(new AddResearchParticipant.Command { ResearchId = researchId }));
        }

        [HttpDelete("leave-research/{researchId}")]
        public async Task<IActionResult> RemoveResearchParticipant(int researchId)
        {
            return HandleResult(await Mediator.Send(new RemoveResearchParticipants.Command { ResearchId = researchId }));
        }
    }
}
