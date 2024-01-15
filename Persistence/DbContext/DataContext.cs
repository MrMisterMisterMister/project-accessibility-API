using Domain;
using Domain.Models.Disabilities;
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
        public DbSet<Disability> Disabilities { get; set; }
        public DbSet<ResearchParticipant> ResearchParticipants { get; set; }
        public DbSet<ExpertDisability> ExpertDisabilities { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Creates a separate table instead of combining the properties in use
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

            builder.Entity<ExpertDisability>(x => x.HasKey(a => new { a.Disabilityid, a.PanelMemberId }));

            builder.Entity<ExpertDisability>()
                .HasOne(x => x.PanelMember)
                .WithMany(p => p.Disabilities)
                .HasForeignKey(d => d.PanelMemberId);

            builder.Entity<ExpertDisability>()
                .HasOne(x => x.Disability)
                .WithMany(r => r.Experts)
                .HasForeignKey(e => e.Disabilityid);
        }
    }
}