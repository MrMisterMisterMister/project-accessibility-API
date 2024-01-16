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
using Application.MessageHandlers;

namespace API.Controllers
{
    public class MessageController : BaseApiController{
      
        [HttpPost]
        //Endpoint to send a message to a user
        public async Task<IActionResult> sendMessage(MessageDTO messageDTO){
            return HandleResult(await Mediator.Send(new SendMessage.Command {ChatId = messageDTO.ChatId, Content = messageDTO.Content, SenderId = messageDTO.SenderId }));
        }
        
    }
}

