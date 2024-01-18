using API.Controllers;
using Application.Core;
using Application.PanelMemberHandlers;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class PanelMembersControllerTest
{
    private readonly PanelMembersController _panelMemberController;
    private readonly Mock<IMediator> _mediatorMock;

    public PanelMembersControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();

        _panelMemberController = new PanelMembersController
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

    [Fact]
    public async Task CreatePanelMember_Should_Create_New_PanelMember()
    {
        // Arrange
        // the strongest there ever has been..
        var panelMember = new PanelMember
        {
            Email = "setokaiba@email.com",
            UserName = "setokaiba@email.com",
            FirstName = "Seto",
            LastName = "Kaiba",
            DateOfBirth = DateTime.UtcNow.AddYears(1),
            Address = "Street under KaibaCorp",
            PostalCode = "1111AA",
            City = "Tokyo",
            Country = "Japan",
        };

        // fill in the command that will be passed to the mock with kaiba..
        var command = new CreatePanelMember.Command { PanelMember = panelMember };

        // mock setup
        // returns successful result when handling command
        _mediatorMock.Setup(x => x.Send(It.IsAny<CreatePanelMember.Command>(), default))
            .ReturnsAsync(Result<Unit>.Success(Unit.Value));

        // Act
        // call the create panel member method in the controller
        var result = await _panelMemberController.CreatePanelMember(panelMember);

        // Assert
        // Check if result is not null
        Assert.NotNull(result);
        // Ok
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeletePanelMember_Should_Delete_PanelMember()
    {
        // Arrange
        // generate random guid to use
        var panelMemberId = Guid.NewGuid();

        // Set up the mock mediator
        _mediatorMock.Setup(x => x.Send(It.IsAny<DeletePanelMember.Command>(), default))
            .ReturnsAsync(Result<Unit>.Success(Unit.Value));

        // Act
        // call the DeletePanelMember method in the controller to delete a panel member
        var result = await _panelMemberController.DeletePanelMember(panelMemberId);

        // Assert
        // check if return is ok
        Assert.IsType<OkObjectResult>(result);
    }
}
