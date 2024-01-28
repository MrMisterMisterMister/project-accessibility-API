using API.DTOs;
using Application.ChatHandlers;
using Application.Handlers.ChatHandlers;
using Application.MessageHandlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class ChatsController : BaseApiController
    {
        // Endpoint to retrieve chat history (messages) between two users
        [HttpGet("history/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetChatHistory(string user1Id, string user2Id)
        {
            return HandleResult(await Mediator.Send(new GetChatHistory.Query { User1Id = user1Id, User2Id = user2Id }));
        }

        [HttpGet("userChats/{userId}")]
        public async Task<IActionResult> GetUserChats(string userId)
        {
            return HandleResult(await Mediator.Send(new GetUserChats.Query { UserId = userId }));
        }
    }
}

