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
        public async Task SendMessageToUser(User sender, User receiver, string message)
        {
            try
            {
                var chat = await _context.FindOrCreateChat(sender.Id, receiver.Id, sender.Email, receiver.Email);
                await _context.AddMessage(sender.Id, message, chat.Id);

                if (UserConnections.TryGetValue(receiver.Id, out string? connectionId))
                {
                    await Clients.Client(connectionId).SendAsync("ReceiveMessage", sender, receiver, message, chat.Id);
                }
                await Clients.Caller.SendAsync("ReceiveMessage", sender, receiver, message, chat.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                // Send error details to the client if necessary
            }
        }

        public void RegisterUser(string idOfTheRegisteringUser)
        {
            try
            {
                if (string.IsNullOrEmpty(idOfTheRegisteringUser))
                {
                    throw new ArgumentException("User email cannot be null or empty.");
                }

                UserConnections[idOfTheRegisteringUser] = Context.ConnectionId;
            }
            catch (Exception ex)
            {
                // Handle the exception
                Console.WriteLine($"An error occurred in RegisterUser: {ex.Message}");
            }
        }
    }
}
