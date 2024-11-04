using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        // Database context class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // DbSet for Messages
        public DbSet<Message> Messages { get; set; }
        public DbSet<KeyBundle> KeyBundles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KeyBundle>()
                .HasKey(k => k.UserId);

            modelBuilder.Entity<KeyBundle>()
                .OwnsOne(k => k.PreKey);

            modelBuilder.Entity<KeyBundle>()
                .OwnsOne(k => k.SignedPreKey);

            base.OnModelCreating(modelBuilder);
        }
    }
}
