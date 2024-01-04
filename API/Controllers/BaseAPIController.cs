using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // This controller is the base for other controllers to inherit common functionalities
    // Ensures that only authenticated users can access these controllers
    // Will add other roles or a policy when we have more roles
    [Authorize(Roles = nameof(RoleTypes.Admin))]
    [ApiController] // Indicates that this controller handles HTTP API requests
    [Route("[controller]")] // Routes requests to endpoints based on the controller's name
    public class BaseApiController : ControllerBase // Inherits from ControllerBase for API functionalities
    {
        private IMediator _mediator = null!; // Nullable IMediator field

        // Property that gets the IMediator instance from the request services
        protected IMediator Mediator => _mediator ??=
            HttpContext.RequestServices.GetService<IMediator>() ?? // Retrieves IMediator from request services
                throw new InvalidOperationException("IMediator service not found in HttpContext.RequestServices");

        // Method to handle different types of results and return appropriate HTTP responses
        protected ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound(); // Returns a Not Found response if the result is null
            if (result.IsSuccess && result.Value != null) return Ok(result.Value); // Returns OK with the value if successful and value exists
            if (result.IsSuccess && result.Value == null) return NotFound(); // Returns Not Found if the result is successful but value is null

            return BadRequest(result.Error); // Returns a Bad Request with the error if there's an error in the result
        }

        // Endpoint for testing purposes to throw an exception
        [HttpGet("exception-test")]
        public IActionResult ThrowException()
        {
            throw new Exception("This is a test exception");
        }
    }
}
