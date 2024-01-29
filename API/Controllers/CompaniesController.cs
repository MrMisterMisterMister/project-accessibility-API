using Application.CompanyHandlers;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Policy = "AdminPolicy")] // for now
    public class CompaniesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            return HandleResult(await Mediator.Send(new GetCompany.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetCompanyById.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            return HandleResult(await Mediator.Send(new CreateCompany.Command { Company = company }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompanyGetCompany(Guid id)
        {
            return HandleResult(await Mediator.Send(new DeleteCompany.Command { Id = id }));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditCompanyGetCompany(Guid id, Company company)
        {
            company.Id = id.ToString();
            return HandleResult(await Mediator.Send(new EditCompany.Command { Company = company }));
        }
    }
}