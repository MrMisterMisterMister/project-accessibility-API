using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator = null!;

        protected IMediator Mediator => _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>() ??
                throw new InvalidOperationException("IMediator service not found in HttpContext.RequestServices");

        // W.I.P
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();

            return BadRequest(result.Error);
        }
    }
}