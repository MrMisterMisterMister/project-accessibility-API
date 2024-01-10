using System;
using System.Threading.Tasks;
using Application.Core;
using Application.ResearchHandlers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Controllers
{
    [ApiController]
[Route("api/[controller]")]
    public class ResearchController : ControllerBase{
        private readonly IMediator _mediator;

        public ResearchController(IMediator mediator){
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> CreateResearch([FromBody] CreateResearch.Command command){
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResearch(Guid id, [FromBody] UpdateResearch.Command command){
            command.ResearchId = id;
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResearch(Guid id){
            return HandleResult(await _mediator.Send(new DeleteResearch.Command { ResearchId = id }));
        }

        [HttpPost("addparticipant")]
        public async Task<IActionResult> AddParticipantToResearch([FromBody] AddParticipant.Command command)
        {
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut("removeparticipant")]
        public async Task<IActionResult> RemoveParticipantFromResearch([FromBody] RemoveParticipant.Command command){
            return HandleResult(await _mediator.Send(command));
        }
        [HttpGet("testendpoint")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEndpoint(){
    try{
         var result = "Endpoint werkt.";
        return Ok(result);
    }
    catch (Exception ex){
        return BadRequest($"Error: {ex.Message}");
    }
}

        private IActionResult HandleResult<T>(Result<T> result){
            if (result.IsSuccess){
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }
    }
}
