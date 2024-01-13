using Domain;
using Microsoft.AspNetCore.Mvc;
using Application.ResearchesHandlers;
using Application.Handlers.ResearchesHandlers;

namespace API.Controllers
{
    public class ResearchesController : BaseApiController
    {
        // all researches
        [HttpGet]
        public async Task<IActionResult> GetResearches()
        {
            return HandleResult(await Mediator.Send(new GetResearches.Query()));
        }

        // get researches by organizer id
        [HttpGet("organizer/{id}")]
        public async Task<IActionResult> GetResearchesByOrganizer(string id)
        {
            return HandleResult(await Mediator.Send(new GetResearchesByOrganizer.Query { OrganizerId = id }));
        }

        // research by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResearchById(int id)
        {
            return HandleResult(await Mediator.Send(new GetResearchById.Query { ResearchId = id }));
        }

        // new research
        [HttpPost]
        public async Task<IActionResult> CreateResearch(ResearchDTO researchDto)
        {
            return HandleResult(await Mediator.Send(new CreateResearch.Command { Research = researchDto }));
        }
        
        // edit research
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResearch(int id, Research research)
        {
            research.Id = id;
            return HandleResult(await Mediator.Send(new EditResearch.Command { Research = research }));
        }

        // delete research
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResearch(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteResearch.Command { ResearchId = id }));
        }
    }
}
