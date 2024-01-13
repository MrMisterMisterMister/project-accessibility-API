using Application.ParticipantsHandlers;
using Microsoft.AspNetCore.Mvc;
using Domain;
using API.DTOs;

namespace API.Controllers
{
    public class ResearchParticipantController : BaseApiController
    {
        [HttpPost("AddResearchParticipant")]
        public async Task<IActionResult> AddResearchParticipant(int researchId)
        {
            return HandleResult(await Mediator.Send(new AddResearchParticipant.Command { ResearchId = researchId }));
        }

        [HttpPost("removeParticipant")]
        public async Task<IActionResult> RemoveParticipant(ParticipantDTO participantDTO)
        {
            return HandleResult(await Mediator.Send(new DeleteResearchParticipants.Command { Participant = participantDTO.PanelMember, Research = participantDTO.Research }));
        }

        // what's even the difference here and with add participant..? A: Dunno?
        [HttpPut("{id}/participate")]
        public async Task<ActionResult> ParticipateInResearch(ParticipantDTO participantDTO)
        {
            return HandleResult(await Mediator.Send(new ParticipateInResearch.Command { Participant = participantDTO.PanelMember, Research = participantDTO.Research }));
        }
    }
}
