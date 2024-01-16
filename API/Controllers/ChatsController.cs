using System;
using System.Threading.Tasks;
using Application.ChatHandlers;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace API.Controllers
{
    public class ChatsController : BaseApiController
    {

        // Endpoint to retrieve chat history between two users
        [HttpGet("history/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetChatHistory(Guid user1Id, Guid user2Id)
        {
            return HandleResult(await Mediator.Send(new GetChatHistory.Query { User1Id = user1Id, User2Id = user2Id }));
        }

    }
}
