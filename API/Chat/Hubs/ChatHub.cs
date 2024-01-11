using API.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace API.Chat.Hubs
{
    public class ChatHub : Hub
    {
        // This method can be used to notify all users in a specific room
        public async Task JoinRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined {conn.ChatRoom}.");
        }

        // Add a method to handle sending messages to a specific room
        public async Task SendMessageToRoom(string chatRoom, string message)
        {
            var username = Context.User.Identity.Name; // Assuming username is available in the User Identity
            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", username, message);
        }
    }
}
