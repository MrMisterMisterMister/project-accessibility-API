using Application.ChatHandlers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers{
    public class ChatController : BaseApiController{
        [HttpPost]
        public async Task<IActionResult> createChat(string user1, string user2){
            return HandleResult(await Mediator.Send(new createChat.Command {User1 = user1, User2 = user2 }));
        }
    }
}