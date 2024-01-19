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

public class ResearchParticipantTest{
    private readonly ResearchParticipantsController _researchParticipantsController;
    private readonly Mock<IMediator> _mediatorMock;

    public ResearchParticipantTest(){
 _mediatorMock = new Mock<IMediator>();
        _researchParticipantsController = new ResearchParticipantsController{
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
    public async Task Should_Add_Participant(){
        
    }
}