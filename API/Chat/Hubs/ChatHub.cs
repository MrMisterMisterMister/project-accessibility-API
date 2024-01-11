using API.Chat.Models;
using Microsoft.AspNetCore.SignalR;

namespace API.Chat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinChat(UserConnection conn)
        {
            await Clients.All
                .SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined.");
        }

        public async Task JoinRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined {conn.ChatRoom}.");
        }
    }
}
