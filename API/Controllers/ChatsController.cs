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
        // Endpoint to retrieve chat history between two users
        [HttpGet("history/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetChatHistory(string user1Id, string user2Id)
        {
            return HandleResult(await Mediator.Send(new GetChatHistory.Query { User1Id = user1Id, User2Id = user2Id }));
        }

        [HttpPost]
        //Endpoint to make chat between 2 users
        public async Task<IActionResult> createChat(ChatDTO chatDTO){
            return HandleResult(await Mediator.Send(new createChat.Command {User1 = chatDTO.User1Id, User2 = chatDTO.user2Id, Title = chatDTO.Title}));
        }

        [HttpGet("userChats/{userId}")]
        public async Task<IActionResult> GetUserChats(string userId)
        {
            return HandleResult(await Mediator.Send(new GetUserChats.Query { UserId = userId }));
        }
        [HttpPost]
        //Endpoint to send a message to a user
        public async Task<IActionResult> sendMessage(MessageDTO messageDTO){
            return HandleResult(await Mediator.Send(new SendMessage.Command {ChatId = messageDTO.ChatId, Content = messageDTO.Content, SenderId = messageDTO.SenderId }));
        }

    }
}

