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
      public async Task SendMessageToUser(string senderId, string receiverId, string message)
        {
            try
            {
                var user1 = await _context.Users.FindAsync(senderId);
                var user2 = await _context.Users.FindAsync(receiverId);

                if (user1 == null || user2 == null)
                {
                    throw new ArgumentException("Invalid sender or receiver id.");
                }

                var existingChat = _context.Chats
                    .FirstOrDefault(chat =>
                        (chat.User1Id == senderId && chat.User2Id == receiverId) ||
                        (chat.User1Id == receiverId && chat.User2Id == senderId));

                if (existingChat != null)
                {
                throw new InvalidOperationException("Chat already exist.");

                }

                if (senderId.Equals(receiverId))
                {
                    throw new InvalidOperationException("Chat can't contain the ID of the same users.");
                }

                var newChat = new Chat
                {
                    User1Id = senderId,
                    User2Id = receiverId,
                    User1Email = user1.Email,
                    User2Email = user2.Email
                };

                _context.Chats.Add(newChat);
                await _context.SaveChangesAsync();
                await Clients.Caller.SendAsync("ReceiveMessage", user1, user2, message, newChat.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void RegisterUser(string idOfTheRegisteringUser)
        {
            try
            {
                if (string.IsNullOrEmpty(idOfTheRegisteringUser))
                {
                    throw new ArgumentException("User ID cannot be null or empty.");
                }

                UserConnections[idOfTheRegisteringUser] = Context.ConnectionId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in RegisterUser: {ex.Message}");
            }
        }
    }
}