using Application.ParticipantsHandlers;
using Microsoft.AspNetCore.Mvc;
using Domain;

namespace API.Controllers
{
    public class ParticipantController : BaseApiController
    {
        [HttpPost("addParticipant")]
        public async Task<IActionResult> AddParticipant(PanelMember participant, Research research)
        {
            return HandleResult(await Mediator.Send(new AddParticipant.Command { Participant = participant, Research = research }));
        }

        [HttpPost("removeParticipant")]
        public async Task<IActionResult> RemoveParticipant(PanelMember participant, Research research)
        {
            return HandleResult(await Mediator.Send(new DeleteParticipants.Command { Participant = participant, Research = research }));
        }

        // what's even the difference here and with add participant..?
        [HttpPut("{id}/participate")]
        public async Task<ActionResult> ParticipateInResearch(PanelMember participant, Research research)
        {
            return HandleResult(await Mediator.Send(new ParticipateInResearch.Command { Participant = participant, Research = research }));
        }
    }
}
