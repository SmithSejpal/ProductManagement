using ProductManagement.Core.Entities;
using ProductManagement.Core.ServicesResult;

namespace ProductManagement.Core.Interfaces.Services
{
    /// <summary>
    /// Service interface for managing product-related operations.
    /// </summary>
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAllProductsAsync();
        public Task<Product> GetProductAsync(string productId);
        public Task<ProductTransactionServiceResult> AddProductAsync(Product product);
        public Task<ProductTransactionServiceResult> UpdateProductAsync(Product product);
        public Task<ProductTransactionServiceResult> DeleteProductAsync(string productId);
        public Task<ProductTransactionServiceResult> AddToStockAsync(string productId, int quantity);
        public Task<ProductTransactionServiceResult> DecrementStockAsync(string productId, int quantity);
    }
}
