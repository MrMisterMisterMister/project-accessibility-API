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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Participant> Participants { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creates a separate table instead of combining the properties in use
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<PanelMember>().ToTable("PanelMembers");
            modelBuilder.Entity<Research>().ToTable("Researches");
            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Participant>().ToTable("Participants");
 
 modelBuilder.Entity<Category>()
                .HasOne(c => c.Research)
                .WithMany(r => r.Categories)
                .HasForeignKey(c => c.ResearchId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}