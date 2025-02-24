using ProductManagement.Core.Entities;
using ProductManagement.Core.Exceptions;
using ProductManagement.Core.Interfaces;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Core.ServicesResult;

namespace ProductManagement.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductUOW _productUOW;
        private readonly IEFRepository<Product> _productRepository;
        private readonly IAppLogger<ProductService> _logger;

        public ProductService(IProductUOW unitOfWork, IAppLogger<ProductService> logger)
        {
            _productUOW = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _productRepository = _productUOW.ProductRepository ?? throw new InvalidOperationException("ProductRepository cannot be null.");
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductAsync(string productId)
            => await GetExistingProduct(productId);

        public async Task<ProductTransactionServiceResult> AddProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            product.Id = await _productRepository.GetNextProductIdAsync();
            var existingProduct = await GetExistingProduct(product.Id);
            if (existingProduct != null) throw new ProductAlreadyExistsException($"Product ID {product.Id} already exists.");

            await _productRepository.AddAsync(product);
            await _productUOW.SaveChangesAsync();
            _logger.LogInformation($"Product ID {product.Id} added successfully.");
            return new ProductTransactionServiceResult { Product = product };
        }

        public async Task<ProductTransactionServiceResult> UpdateProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            var existingProduct = await GetExistingProduct(product.Id);
            if (existingProduct == null) throw new ProductNotFoundException($"Product ID {product.Id} not found.");

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;

            await _productUOW.SaveChangesAsync();
            _logger.LogInformation($"Product ID {product.Id} updated successfully.");
            return new ProductTransactionServiceResult { Product = product };
        }

        public async Task<ProductTransactionServiceResult> DeleteProductAsync(string productId)
        {
            var existingProduct = await GetExistingProduct(productId);
            if (existingProduct == null)
                throw new ProductNotFoundException($"Product ID {productId} not found.");

            // Add the deleted product's ID to the ReusableProductId table
            var reusableId = new ReusableProductId { Id = productId };
            await _productUOW.ReusableProductIdRepository.AddAsync(reusableId);

            _productRepository.Delete(existingProduct);
            await _productUOW.SaveChangesAsync();

            _logger.LogInformation($"Product ID {productId} deleted successfully.");
            return new ProductTransactionServiceResult();
        }

        public async Task<ProductTransactionServiceResult> AddToStockAsync(string productId, int quantity)
        {
            var product = await GetExistingProduct(productId);
            if (product == null)
                throw new ProductNotFoundException($"Product ID {productId} not found.");

            product.Stock += quantity;
            await _productUOW.SaveChangesAsync();
            _logger.LogInformation($"New quantity of product {productId}: {product.Stock}");
            return new ProductTransactionServiceResult { Product = product };
        }

        public async Task<ProductTransactionServiceResult> DecrementStockAsync(string productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

            var product = await GetExistingProduct(productId);
            if (product == null)
                throw new ProductNotFoundException($"Product ID {productId} not found.");

            ProductTransactionServiceResult result = new ProductTransactionServiceResult();

            if (product.Stock < quantity)
            {
                result.AddMessage($"Insufficient stock. Available: {product.Stock}.");
                return result; 
            }

            product.Stock -= quantity;
            await _productUOW.SaveChangesAsync();
            _logger.LogInformation($"New quantity of product {productId}: {product.Stock}");
            result.Product = product;
            return result;
        }

        private async Task<Product> GetExistingProduct(string productId)
           => await _productRepository.GetByIdAsync(productId);
    }
}