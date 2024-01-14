using API.Chat.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Chat.Hubs
{
    public class ChatHub : Hub
    {
        //Dictionary for the Users and ConnectionId, that's the plan...

        // Context.ConnectionId is zelf gegeven door SignalR wanneer een gebruiker connect met SignalR
        public async Task JoinRoom(UserConnection conn)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);

            // Calls the ReceiveMessage function in the frontend
            await Clients.Group(conn.ChatRoom).SendAsync("ReceiveMessage", "System", $"{conn.Username} has joined {conn.ChatRoom}.");
        }
        
        public async Task SendMessageToRoom(string chatRoom, string username, string message)
        {
            await Clients.Group(chatRoom).SendAsync("ReceiveMessage", username, message);
        }
    }
}
