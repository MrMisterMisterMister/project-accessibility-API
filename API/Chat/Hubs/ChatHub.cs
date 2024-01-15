using API.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace API.Chat.Hubs
{
    public class ChatHub : Hub
    {
        //Dictionary for the Users and ConnectionId, that's the plan...
        private static readonly ConcurrentDictionary<string, string> UsersConnections = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            // Optionally, you can track connected users here
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Optionally, handle disconnection and remove user from the dictionary
            UsersConnections.TryRemove(Context.UserIdentifier, out _);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToUser(string targetUserEmail, string message)
        {
            if (UsersConnections.TryGetValue(targetUserEmail, out string? connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", Context.UserIdentifier, message);
            }
            else
            {
                // Handle the case where the user is not found or not connected
            }
        }

        public void RegisterUser(string userEmail)
        {
            UsersConnections.TryAdd(userEmail, Context.ConnectionId);
        }
    }
}
