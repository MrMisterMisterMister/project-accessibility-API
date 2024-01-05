using Application.PanelMemberHandlers;
using Application.ResearchHandlers;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PanelMembersController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetPanelMember()
        {
            return HandleResult(await Mediator.Send(new GetPanelMember.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPanelMemberById(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetPanelMemberById.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(PanelMember panelMember)
        {
            return HandleResult(await Mediator.Send(new CreatePanelMember.Command { PanelMember = panelMember }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeletePanelMember.Command { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(Guid id, PanelMember panelMember)
        {
            panelMember.Id = id.ToString();
            return HandleResult(await Mediator.Send(new EditPanelMember.Command { PanelMember = panelMember }));
        }
        [HttpPut("{id}/participate")]
        public async Task<ActionResult> ParticipateInResearch(Guid id, PanelMember panelMember){
            panelMember.Id = id.ToString();
        return HandleResult(await Mediator.Send(new ParticipateInResearch.Command { ParticipantId = Guid.Parse(panelMember.Id) }));
        }
    }
}