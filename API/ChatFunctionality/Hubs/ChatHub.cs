using Domain;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Concurrent;

namespace API.ChatFunctionality.Hubs
{
    public class ChatHub : Hub
    {
        private DataContext _context;

        //Dictionary for the Users and ConnectionId, that's the plan...
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();

        public ChatHub(DataContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            // You can also handle any additional actions when a user connects here
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove the user from the connection dictionary on disconnect
            string userEmail = UserConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(userEmail))
            {
                UserConnections.TryRemove(userEmail, out _);
            }

            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessageToUser(string senderEmail, string senderId, string receiverEmail, string receiverId, string message, User sender)
        {
            try
            {
                var chat = await _context.FindOrCreateChat(senderId, receiverId);
                await _context.AddMessage(senderId, message, chat.Id);

                if (UserConnections.TryGetValue(receiverEmail, out string? connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", sender, message);
                }
                await Clients.Caller.SendAsync("ReceiveMessage", sender, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Send error details to the client if necessary
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
