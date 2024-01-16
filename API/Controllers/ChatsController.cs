using System;
using System.Threading.Tasks;
using Application.ChatHandlers;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using API.DTOs;

namespace API.Controllers
{
    [AllowAnonymous]
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
        
    }
}

