using API.Controllers;
using Application.Core;
using Application.ResearchHandlers;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Persistence;
public class ResearchControllerTest{
    private readonly ResearchesController _researchController;
    private readonly Mock<IMediator> _mediatorMock;

    public ResearchControllerTest(){
 _mediatorMock = new Mock<IMediator>();
        _researchController = new ResearchesController{
            ControllerContext = new ControllerContext{
                HttpContext = new DefaultHttpContext{
                    RequestServices = new ServiceCollection()
                        .AddSingleton<IMediator>(_mediatorMock.Object)
                        .BuildServiceProvider()
                }
            }
            
        };
    }
    [Fact]
    public async Task Should_Create_A_Research(){
    
        var research = new Research{
            Title = "Test research",
            Description = "Test beschrijving",
            Date = DateTime.UtcNow.AddMonths(1),
            Type = "Online",
            Category = "Parkinson",
            Reward = 54.3,
            Organizer = CreateCompany(),
             Participants = new List<ResearchParticipant>(){
                new ResearchParticipant() {
                PanelMember = CreatePanelMember(),
                DateJoined = DateTime.UtcNow.AddYears(new Random().Next(0, 100))
                            }

        }
    };
    var command = new CreateResearch.Command{Research = research };

    _mediatorMock.Setup(x => x.Send(It.IsAny<CreateResearch.Command>(), default))
            .ReturnsAsync(Result<Unit>.Success(Unit.Value));
//Act
            var resultaat = await _researchController.CreateResearch(research) as ObjectResult;

 //Assert           

            Assert.NotNull(resultaat);
            Assert.Equal(StatusCodes.Status200OK, resultaat.StatusCode);

    }
[Fact]
public async Task Should_Delete_Research()
{
    // Arrange
    var researchId = new Random().Next(1, 1000);

    _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteResearch.Command>(), default))
        .ReturnsAsync(Result<Unit>.Success(Unit.Value));

    // Act
    var result = await _researchController.DeleteResearch(researchId);

    // Assert
    Assert.IsType<OkObjectResult>(result);
}
[Fact]
public async Task EditResearch_Should_Edit_Research()
{
    // Arrange
    var researchId = 1; 
    var editedResearch = new Research
    {
        Id = researchId,
        Title = "Edited Title",
        Description = "Edited Description",
    };
    // Set up off the mock mediator
    _mediatorMock.Setup(x => x.Send(It.IsAny<EditResearch.Command>(), default))
        .ReturnsAsync(Result<Unit>.Success(Unit.Value));

    // Act
    var result = await _researchController.EditResearch(researchId, editedResearch);

    // Assert
    Assert.IsType<OkObjectResult>(result);

    Assert.Equal("Edited Title", editedResearch.Title);
    Assert.Equal("Edited Description", editedResearch.Description);

}
  private Company CreateCompany()
    {
        var company = new Company
        {
            Email = "company@example.com",
            Kvk = "123456789",
            CompanyName = "Test Company",
            Phone = "123-456-7890",
            Address = "123 Main St",
            PostalCode = "12345",
            Province = "Test Province",
            Country = "Test Country",
            WebsiteUrl = "https://www.testcompany.com",
            ContactPerson = "John Doe"
        };
        return company;
    }

    private PanelMember CreatePanelMember()
    {
        var panelMember = new PanelMember
        {
            Email = "panelmember@example.com",
            Guardian = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateTime(1990, 1, 1),
            Address = "456 Oak St",
            PostalCode = "67890",
            City = "Test City",
            Country = "Test Country"
        };
        return panelMember;
    }
}
