using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.Entities;

namespace ProductManagement.Infrastructure.Data
{
    /// <summary>
    /// Database context for the application, managing access to the Product entity.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        /// <summary>
        /// Represents the Product table in the database.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Represents the ResulableProductId table in the database.
        /// </summary>
        public DbSet<ReusableProductId> ReusableProductId { get; set; }
    }
}
