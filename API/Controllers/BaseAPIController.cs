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
    }
}