using Application.CompanyHandlers;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CompaniesController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetCompany()
        {
            return await Mediator.Send(new GetCompany.Query());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(Guid id)
        {
            return await Mediator.Send(new GetCompanyById.Query { Id = id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            await Mediator.Send(new CreateCompany.Command { Company = company });

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            await Mediator.Send(new DeleteCompany.Command { Id = id });

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditCompany(Guid id, Company company)
        {
            company.Id = id.ToString();
            await Mediator.Send(new EditCompany.Command { Company = company });

            return Ok();
        }
    }
}