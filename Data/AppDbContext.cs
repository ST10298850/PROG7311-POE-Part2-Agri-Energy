using AgriEnergyConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AgriEnergyConnect.Data
{
    /// <summary>
    /// Represents the database context for the AgriEnergyConnect application.
    /// This class extends IdentityDbContext to include custom user management.
    /// </summary>
    public class AppDbContext : IdentityDbContext<User>
    {
        /// <summary>
        /// Initializes a new instance of the AppDbContext.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Gets or sets the UserDetails DbSet.
        /// </summary>
        public DbSet<UserDetail> UserDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the FarmerApplications DbSet.
        /// </summary>
        public DbSet<FarmerApplication> FarmerApplications { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Farms DbSet.
        /// </summary>
        public DbSet<Farm> Farms { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Products DbSet.
        /// </summary>
        public DbSet<Product> Products { get; set; } = null!;

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types
        /// exposed in DbSet properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
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

            // User - Product (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // Avoid circular cascade issues
        }
    }
}