using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductManagement.Core.Entities
{
    [Table("Product")]
    public class Product
    {
        /// <summary>
        /// Unique 6-digit identifier for the product.
        /// This ID is auto-generated to ensure uniqueness across multiple instances.
        /// </summary>
        [Key]
        [Column(TypeName = "varchar(6)")]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Name of the product.
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Detailed description of the product.
        /// </summary>
        [Required]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Price of the product in decimal format.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The available stock quantity for this product.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Timestamp indicating when the product was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
