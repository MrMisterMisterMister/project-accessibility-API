using API.Controllers;
using Application.CompanyHandlers;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

public class CompaniesControllerTest
{
    private readonly CompaniesController _companiesController;
    private readonly Mock<IMediator> _mediatorMock;

    public CompaniesControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _companiesController = new CompaniesController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    RequestServices = new ServiceCollection()
                        .AddSingleton<IMediator>(_mediatorMock.Object)
                        .BuildServiceProvider()
                }
            }
        };
    }

    private Company CreateSampleCompany()
    {
        return new Company
        {
            Kvk = "12345678",
            CompanyName = "Doofenshmirtz Evil Incorporated",
            Phone = "123-456-7890",
            Address = "123 Sesame Street",
            PostalCode = "1234 AB",
            Province = "Amsterdam",
            Country = "The Netherlands",
            WebsiteUrl = "https://www.evilcompany.com",
            ContactPerson = "Heinz Doofenshmirtz"
        };
    }

    [Fact]
    public async Task GetCompanies_Should_Return_Companies()
    {
        // Arrange
        var companies = new List<Company>
        {
            CreateSampleCompany(),
            CreateSampleCompany()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCompany.Query>(), default))
                     .ReturnsAsync(Application.Core.Result<List<Company>>.Success(companies));

        // Act
        var result = await _companiesController.GetCompanies();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Successful data http return
        var returnedCompanies = Assert.IsType<List<Company>>(okResult.Value); // Confirms is of type Company
    }

    [Fact]
    public async Task GetCompanyById_Should_Return_Company()
    {
        // Arrange
        var company = CreateSampleCompany();

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetCompanyById.Query>(), default))
                     .ReturnsAsync(Application.Core.Result<Company>.Success(company));

        var companyId = Guid.NewGuid(); // Doesn't really matter what Id u pass here

        // Act
        var result = await _companiesController.GetCompanyById(companyId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
        var returnedCompany = Assert.IsType<Company>(okResult.Value); // Confirms is of type Company
    }

    [Fact]
    public async Task CreateCompany_Should_Create_New_Company()
    {
        // Arrange
        var company = CreateSampleCompany();

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateCompany.Command>(), default))
                     .ReturnsAsync(Application.Core.Result<Unit>.Success(Unit.Value));

        // Act
        var result = await _companiesController.CreateCompany(company);

        // Assert
        Assert.IsType<OkObjectResult>(result); // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
    }

    [Fact]
    public async Task DeleteCompany_Should_Delete_Company()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteCompany.Command>(), default))
                     .ReturnsAsync(Application.Core.Result<Unit>.Success(Unit.Value));

        // Act
        var result = await _companiesController.DeleteCompanyGetCompany(companyId);

        // Assert
        Assert.IsType<OkObjectResult>(result); // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
    }

    [Fact]
    public async Task EditCompany_Should_Update_Company()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = CreateSampleCompany();

        _mediatorMock.Setup(m => m.Send(It.IsAny<EditCompany.Command>(), default))
                     .ReturnsAsync(Application.Core.Result<Unit>.Success(Unit.Value));

        // Act
        var result = await _companiesController.EditCompanyGetCompany(companyId, company);

        // Assert
        Assert.IsType<OkObjectResult>(result); // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
    }
}
