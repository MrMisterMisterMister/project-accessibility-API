using Application.ParticipantsHandlers;
using Microsoft.AspNetCore.Mvc;
using Domain;
using API.DTOs;

namespace API.Controllers
{
    public class ParticipantController : BaseApiController
    {
        [HttpPost("addParticipant")]
        public async Task<IActionResult> AddParticipant(ParticipantDTO participantDTO)
        {
            return HandleResult(await Mediator.Send(new AddParticipant.Command { Participant = participantDTO.PanelMember, Research = participantDTO.Research }));
        }

        [HttpPost("removeParticipant")]
        public async Task<IActionResult> RemoveParticipant(ParticipantDTO participantDTO)
        {
            return HandleResult(await Mediator.Send(new DeleteParticipants.Command { Participant = participantDTO.PanelMember, Research = participantDTO.Research }));
        }

        // what's even the difference here and with add participant..?
        [HttpPut("{id}/participate")]
        public async Task<ActionResult> ParticipateInResearch(ParticipantDTO participantDTO)
        {
            return HandleResult(await Mediator.Send(new ParticipateInResearch.Command { Participant = participantDTO.PanelMember, Research = participantDTO.Research }));
        }
    }
}
