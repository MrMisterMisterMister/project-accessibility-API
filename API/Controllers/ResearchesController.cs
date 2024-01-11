using Domain;
using Application.ResearchHandlers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ResearchesController : BaseApiController
    {
        // all researches
        [HttpGet]
        public async Task<IActionResult> GetResearches()
        {
            return HandleResult(await Mediator.Send(new GetResearch.Query()));
        }

        // research by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResearchById(int id)
        {
            return HandleResult(await Mediator.Send(new GetResearchById.Query { ResearchId = id }));
        }

        // new research
        [HttpPost]
        public async Task<IActionResult> CreateResearch(Research research)
        {
            return HandleResult(await Mediator.Send(new CreateResearch.Command { Research = research }));
        }

        // edit research
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResearch(int id, Research research)
        {
            research.Id = id;
            return HandleResult(await Mediator.Send(new UpdateResearch.Command { Research = research }));
        }

        // delete research
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResearch(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteResearch.Command { ResearchId = id }));
        }
    }
}
