using API.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Chat.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", conn.Username, $"{conn.Username} has joined {conn.ChatRoom}.");
        }

        public async Task SendMessageToRoom(string chatRoom, string username, string message)
        {
            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", username, message);
        }
    }
}
