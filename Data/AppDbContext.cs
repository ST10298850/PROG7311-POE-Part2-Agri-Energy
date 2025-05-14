using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UserDetail> UserDetails { get; set; } = null!;
        public DbSet<FarmerApplication> FarmerApplications { get; set; } = null!;
        public DbSet<Farm> Farms { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - UserDetail (One-to-One)
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDetail)
                .WithOne(ud => ud.User)
                .HasForeignKey<UserDetail>(ud => ud.UserID);

            // Farm - User (Many-to-One)
            modelBuilder.Entity<Farm>()
                .HasOne(f => f.User)
                .WithMany(u => u.Farms)
                .HasForeignKey(f => f.UserID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Farm - Product (One-to-Many)
            modelBuilder.Entity<Farm>()
                .HasMany(f => f.Products)
                .WithOne(p => p.Farm)
                .HasForeignKey(p => p.FarmID)
                .OnDelete(DeleteBehavior.Cascade);

            // New: User - Product (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Optional: Avoid circular cascade issues
        }
    }
}