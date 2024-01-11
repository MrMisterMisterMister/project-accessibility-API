using Application.ResearchHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ResearchController : BaseApiController 
    {
        private readonly IMediator _mediator;

        public ResearchController(IMediator mediator){
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResearchById(int id)
        {
            return HandleResult(await _mediator.Send(new GetResearchById.Query { ResearchId = id }));
        }
 [HttpGet("Title")]
public async Task<IActionResult> GetResearchByTitle(string titel)
{
    Console.WriteLine(titel);
    return HandleResult(await _mediator.Send(new GetResearchByTitle.Query { Title = titel }));
}
        [HttpPost]
        public async Task<IActionResult> CreateResearch([FromBody] CreateResearch.Command command){
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResearch(int id, [FromBody] UpdateResearch.Command command){
            command.ResearchId = id;
            return HandleResult(await _mediator.Send(command));
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResearch(int id){
        return HandleResult(await _mediator.Send(new DeleteResearch.Command {ResearchId  = id}));
        }
    }
}