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
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public async Task SendMessageToUser(string targetUserEmail, string message)
        {
            if (UserConnections.TryGetValue(targetUserEmail, out string? connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", connectionId, message);
            }
            else
            {
                // Handle the case where the user is not found or not connected
            }
        }

        public void RegisterUser(string userEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail))
                {
                    throw new ArgumentException("User email cannot be null or empty.");
                }

                UserConnections[userEmail] = Context.ConnectionId;
            }
            catch (Exception ex)
            {
                // Handle the exception
                // You might want to log the exception or take other appropriate actions
                // For example:
                Console.WriteLine($"An error occurred in RegisterUser: {ex.Message}");
                // Depending on your application's needs, you might also want to rethrow the exception
                // throw;
            }
        }

    }
}
