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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("data source=KCT-L-472\\SQLEXPRESS;initial catalog=CMSDB;user id=sa;Password=kalki@123;Integrated Security=false;TrustServerCertificate=True;");
        }
    }
}
