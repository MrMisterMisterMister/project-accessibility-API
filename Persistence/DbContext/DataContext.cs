using System.Net.Mail;
using Domain;
using Domain.Models.ChatModels;
using Domain.Models.Disabilities;
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
        public DbSet<Research> Researches { get; set; }
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<ResearchParticipant> ResearchParticipants { get; set; }
        public DbSet<PanelMemberDisability> PanelMemberDisabilities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

             // when using inheritence
            builder.Entity<Company>().ToTable("Companies");
            builder.Entity<PanelMember>().ToTable("PanelMembers");

            builder.Entity<ResearchParticipant>(x => x.HasKey(a => new { a.ResearchId, a.PanelMemberId }));

            builder.Entity<ResearchParticipant>()
                .HasOne(x => x.PanelMember)
                .WithMany(p => p.Participations)
                .HasForeignKey(ps => ps.PanelMemberId);

            builder.Entity<ResearchParticipant>()
                .HasOne(x => x.Research)
                .WithMany(r => r.Participants)
                .HasForeignKey(p => p.ResearchId);

            builder.Entity<PanelMemberDisability>(x => x.HasKey(a => new { a.DisabilityId, a.PanelMemberId }));

            builder.Entity<PanelMemberDisability>()
                .HasOne(x => x.PanelMember)
                .WithMany(p => p.Disabilities)
                .HasForeignKey(d => d.PanelMemberId);

            builder.Entity<PanelMemberDisability>()
                .HasOne(x => x.Disability)
                .WithMany(r => r.PanelMembers)
                .HasForeignKey(e => e.DisabilityId);

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

        public async Task<Chat> FindOrCreateChat(string user1Id, string user2Id, string user1IdEmail, string user2IdEmail)
        {
            // Logic to find or create a chat session between two users
            // This should check if a chat already exists between these two users
            // If not, create a new chat session and return it
            var chat = await Chats
                            .FirstOrDefaultAsync(c =>
                                (c.User1Id == user1Id && c.User2Id == user2Id) ||
                                (c.User1Id == user2Id && c.User2Id == user1Id));

            if (chat == null)
            {
                chat = new Chat { User1Id = user1Id, User2Id = user2Id, User1Email = user1IdEmail, User2Email = user2IdEmail };
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