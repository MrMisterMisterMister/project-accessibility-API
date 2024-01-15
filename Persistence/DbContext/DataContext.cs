using Domain;
using Domain.Models.Chat;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Creates a separate table instead of combining the properties in use
            modelBuilder.Entity<Company>().ToTable("Companies");
            modelBuilder.Entity<PanelMember>().ToTable("PanelMembers");
        }
    }
}