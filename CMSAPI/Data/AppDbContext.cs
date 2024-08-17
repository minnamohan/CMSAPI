using CMSAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMSAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();// Ensure Emails are unique

            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique(); // Ensure usernames are unique
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Add your connection string here");
        }
    }
}
