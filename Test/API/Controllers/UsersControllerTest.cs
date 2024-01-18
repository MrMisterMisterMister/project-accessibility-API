using API.Controllers;
using Application.Core;
using Application.Handlers.UserHandlers;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class UsersControllerTest
{
    private readonly UsersController _userController;
    private readonly Mock<IMediator> _mediatorMock;

    public UsersControllerTest()
    {
        // Initialize common dependencies in the constructor
        _mediatorMock = new Mock<IMediator>();

        // Create an instance of UsersController for testing
        _userController = new UsersController
        {
            // Setting up the ControllerContext with a mocked HttpContext
            // This section is crucial for testing, as it emulates the environment in which the controller operates.
            ControllerContext = new ControllerContext
            {
                // Creating a DefaultHttpContext with a mocked request services
                // This is necessary because the BaseApiController uses HttpContext.RequestServices
                // to obtain the IMediator instance. We want to replace the actual services with our mock during testing.
                HttpContext = new DefaultHttpContext
                {
                    // Setting up the RequestServices to provide the mocked IMediator
                    // This ensures that when BaseApiController retrieves IMediator from HttpContext.RequestServices,
                    // it gets our mock instead of the actual service, allowing us to control its behavior in the test.
                    RequestServices = new ServiceCollection()
                        .AddSingleton<IMediator>(_mediatorMock.Object)
                        .BuildServiceProvider()
                }
            }
        };
    }

    [Fact]
    public async Task GetUsers_Should_Return_Users()
    {
        // Arrange

        // Setting up the IMediator mock to return a successful result
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetUser.Query>(), default))
            .ReturnsAsync(Result<List<User>>.Success(new List<User>()));

        // Act

        // Invoking the GetUsers action on the UsersController
        var result = await _userController.GetUsers();

        // Assert

        // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Verifying that the value inside the OkObjectResult is of type List<User>
        var users = Assert.IsType<List<User>>(okResult.Value);
    }

    [Fact]
    public async Task GetUserById_Should_Return_User()
    {
        // Arrange

        // Setting up the IMediator mock to return a successful result
        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetUserById.Query>(), default))
            .ReturnsAsync(Result<User>.Success(new User()));

        // Act

        // Invoking the GetUserById action on the UsersController
        // Doesn't really matter what Id u pass here
        var result = await _userController.GetUserById(Guid.NewGuid());

        // Assert

        // Verifying that the result is of type OkObjectResult (HTTP Status 200 OK)
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Verifying that the value inside the OkObjectResult is of type User
        var user = Assert.IsType<User>(okResult.Value);
    }


}
