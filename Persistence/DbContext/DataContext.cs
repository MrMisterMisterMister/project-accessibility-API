using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Company> Companies { get; set; }
        public DbSet<PanelMember> PanelMembers { get; set; }
        public DbSet<Research> Researches { get; set; }
        public DbSet<ResearchParticipant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creates a separate table instead of combining the properties in use
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<PanelMember>().ToTable("PanelMembers");

            // Configure participants
            modelBuilder.Entity<ResearchParticipant>(x => x.HasKey(rp => new { rp.ResearchId, rp.PanelMemberId }));

            modelBuilder.Entity<ResearchParticipant>()
                .HasOne(r => r.Research)
                .WithMany(p => p.Participants)
                .HasForeignKey(rp => rp.ResearchId);

            modelBuilder.Entity<ResearchParticipant>()
                .HasOne(p => p.PanelMember)
                .WithMany(r => r.Researches)
                .HasForeignKey(rp => rp.PanelMemberId);
        }
    }
}