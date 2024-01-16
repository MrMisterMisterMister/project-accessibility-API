using Domain.Models.ChatModels;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Collections.Concurrent;

namespace API.ChatFunctionality.Hubs
{
    public class ChatHub : Hub
    {
        private readonly DataContext _context;

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
        public async Task SendMessageToUser(string senderEmail, string receiverEmail, string message)
        {
            var chat = await FindOrCreateChat(senderEmail, receiverEmail);
            var msg = new Message { SenderId = senderEmail, Content = message, ChatId = chat.Id };
            _context.Messages.Add(msg);
            await _context.SaveChangesAsync();

            if (UserConnections.TryGetValue(receiverEmail, out string connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", senderEmail, message);
            }
            await Clients.Caller.SendAsync("ReceiveMessage", receiverEmail, message);
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
        private async Task<Chat> FindOrCreateChat(string user1Email, string user2Email)
        {
            // Logic to find or create a chat session between two users
            // This should check if a chat already exists between these two users
            // If not, create a new chat session and return it
            // Example implementation:
            var chat = await _context.Chats
                            .FirstOrDefaultAsync(c =>
                                (c.User1Id == user1Email && c.User2Id == user2Email) ||
                                (c.User1Id == user2Email && c.User2Id == user1Email));

            if (chat == null)
            {
                chat = new Chat { User1Id = user1Email, User2Id = user2Email };
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
            }

            return chat;
        }

    }
}
