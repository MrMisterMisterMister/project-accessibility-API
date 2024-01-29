using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Domain.Models.Disabilities;
using Application.DisabilityHandlers;

namespace API.Controllers
{
    [Authorize]
    public class DisabilitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetDisabilities()
        {
            return HandleResult(await Mediator.Send(new GetDisabilities.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDisabilityById(int id)
        {
            return HandleResult(await Mediator.Send(new GetDisabilityById.Query { DisabilityId = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateDisability(Disability disability)
        {
            return HandleResult(await Mediator.Send(new CreateDisability.Command { Disability = disability }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditDisability(int id, Disability disability)
        {
            disability.Id = id;
            return HandleResult(await Mediator.Send(new EditDisability.Command { Disability = disability }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisability(int id)
        {
            return HandleResult(await Mediator.Send(new DeleteDisability.Command { DisabilityId = id }));
        }
    }
}
