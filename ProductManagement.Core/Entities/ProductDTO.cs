using System.ComponentModel.DataAnnotations;

namespace ProductManagement.Core.Entities
{
    /// <summary>
    /// Represents a data transfer object (DTO) for adding or updating a product.
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Name of the product.
        /// </summary>
        [Required(ErrorMessage = "Product name is required.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the product.
        /// </summary>
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Price of the product in decimal format.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        /// <summary>
        /// The available stock quantity for this product.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }
    }
}
