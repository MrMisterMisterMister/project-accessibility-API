using Application.PanelMemberHandlers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> CreatePanelMember(PanelMember panelMember)
        {
            return HandleResult(await Mediator.Send(new CreatePanelMember.Command { PanelMember = panelMember }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePanelMember(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeletePanelMember.Command { PanelmemberId = id }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditPanelMember(Guid id, PanelMember panelMember)
        {
            panelMember.Id = id.ToString();
            return HandleResult(await Mediator.Send(new EditPanelMember.Command { PanelMember = panelMember }));
        }
    }
}
