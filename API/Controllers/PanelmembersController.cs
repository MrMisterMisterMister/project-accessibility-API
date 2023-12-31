using Application.PanelMemberHandlers;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PanelMembersController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<PanelMember>>> GetPanelMembers()
        {
            return await Mediator.Send(new GetPanelMember.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PanelMember>> GetCompanyById(Guid id)
        {
            return await Mediator.Send(new GetPanelMemberById.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePanelMember(PanelMember panelMember)
        {
            await Mediator.Send(new CreatePanelMember.Command { PanelMember = panelMember });

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePanelMember(Guid id)
        {
            await Mediator.Send(new DeletePanelMember.Command { Id = id });

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditPanelMember(Guid id, PanelMember panelMember)
        {
            panelMember.Id = id.ToString();
            await Mediator.Send(new EditPanelMember.Command { PanelMember = panelMember });

            return Ok();
        }
    }
}