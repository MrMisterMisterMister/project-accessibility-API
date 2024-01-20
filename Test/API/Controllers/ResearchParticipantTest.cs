using API.Controllers;
using Application.ParticipantsHandlers;
using Test.API.TestData;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Application.Core;
using Application.Interfaces;

public class ResearchParticipantTest{
    private readonly ResearchParticipantsController _researchParticipantsController;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TestData _testData;
    private readonly Mock<IUserAccessor> _userAccessorMock;
    public ResearchParticipantTest(){
 _mediatorMock = new Mock<IMediator>();
 _userAccessorMock = new Mock<IUserAccessor>();
        _researchParticipantsController = new ResearchParticipantsController{
            ControllerContext = new ControllerContext{
                HttpContext = new DefaultHttpContext{
                    RequestServices = new ServiceCollection()
                        .AddSingleton<IMediator>(_mediatorMock.Object)
                        .BuildServiceProvider()
                }
            }  
        };
        _testData = new TestData();
    }
    [Fact]
    public async Task Should_Add_Participant_To_Research(){
        //Arrange
        var research = _testData.CreateRandomResearch();
        var panelmember = _testData.CreatePanelMember();
        var addParticipant = new AddResearchParticipant.Command{ResearchId = research.Id};
        //Setup off IUserAccessor to get id off panelmember
        _userAccessorMock.Setup(x => x.GetId()).Returns(panelmember.Id);

        //Setup off mediatorMock to return succes when succesfully adding participant
        _mediatorMock.Setup(x => x.Send(It.IsAny<AddResearchParticipant.Command>(), default))
        .ReturnsAsync(Result<Unit>.Success(Unit.Value));

        //Act 
        var result = await _researchParticipantsController.AddResearchParticipant(research.Id);

        //Assert, check if the result is not null and has the correct status response.
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        //Additional check if the command is send to the mediator exactly once
        _mediatorMock.Verify(x => x.Send(It.Is<AddResearchParticipant.Command>(c => 
        c.ResearchId == research.Id), default), Times.Once);
    }
    [Fact]
    public async Task Should_Delete_Participant_From_Research(){
        //Arrange
        var research = _testData.CreateRandomResearch();
        var participant = _testData.CreatePanelMember();

        _userAccessorMock.Setup(x => x.GetId()).Returns(participant.Id);
        //Setup off mediatorMock to return succes when succesfully deleting participant from research
        _mediatorMock.Setup(x => x.Send(It.IsAny<RemoveResearchParticipants.Command>(), default))
        .ReturnsAsync(Result<Unit>.Success(Unit.Value));

        //Act
        var result = await _researchParticipantsController.RemoveResearchParticipant(research.Id) as ObjectResult;

        //Assert, check if the result is not null and has the correct status response.
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
//Additional check if the command is send to the mediator exactly once
     _mediatorMock.Verify(x => x.Send(It.Is<RemoveResearchParticipants.Command>(c =>
        c.ResearchId == research.Id), default), Times.Once);
    }
}
   