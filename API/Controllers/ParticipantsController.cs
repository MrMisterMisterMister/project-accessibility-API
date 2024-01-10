using System;
using API.Controllers;
using Application.ResearchHandlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ParticipantController : BaseApiController{
        private readonly IMediator _mediator;

        public ParticipantController(IMediator mediator){
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("addParticipant")]
        public async Task<IActionResult> AddParticipant([FromBody] AddParticipant.Command command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess){
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [HttpPost("removeParticipant")]
        public async Task<IActionResult> RemoveParticipant([FromBody] RemoveParticipant.Command command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}
