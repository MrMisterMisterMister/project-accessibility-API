using System.Net.Mail;
using Domain;
using Domain.Models.ChatModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<PanelMember> PanelMembers { get; set; }
        public DbSet<Chat> Chats {get;set;}
        public DbSet<Message> Messages {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creates a separate table instead of combining the properties in use
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<PanelMember>().ToTable("PanelMembers");
            modelBuilder.Entity<Message>().ToTable("Messages");
            modelBuilder.Entity<Chat>().ToTable("Chats");

            // Configure the one-to-one relationships in Chat
            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User1)
                .WithMany() // Assuming User class has no navigation property back to Chat
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Chat>()
                .HasOne(c => c.User2)
                .WithMany() // Same as above
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            modelBuilder.Entity<Message>()
            .Property(m => m.IsRead)
            .HasDefaultValue(false);
                        // Configure the one-to-many relationship between Chat and Message
            modelBuilder.Entity<Message>()
   
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId);
        }

        public async Task<Chat> FindOrCreateChat(string user1Email, string user2Email)
        {
            // Logic to find or create a chat session between two users
            // This should check if a chat already exists between these two users
            // If not, create a new chat session and return it
            var chat = await Chats
                            .FirstOrDefaultAsync(c =>
                                (c.User1Id == user1Email && c.User2Id == user2Email) ||
                                (c.User1Id == user2Email && c.User2Id == user1Email));

            if (chat == null)
            {
                chat = new Chat { User1Id = user1Email, User2Id = user2Email };
                Chats.Add(chat);
                await SaveChangesAsync();
            }

            return chat;
        }

        public async Task<Message> AddMessage(string senderId, string messageContent, int id)
        {
            var message = new Message { SenderId = senderId, Content = messageContent, ChatId = id };
            Messages.Add(message);
            await SaveChangesAsync();
            return message;
        }
    }
}