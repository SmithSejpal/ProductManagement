using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Infrastructure.Data;
using ProductManagement.Infrastructure.Repositories;

namespace Portal.Infrastructure.Data
{
    /// <summary>
    /// Implements the unit of work pattern for managing product-related database operations.
    /// </summary>
    public class ProductUOW : IProductUOW
    {
        private readonly AppDbContext _context;

        private readonly Lazy<IEFRepository<Product>> _productRepository;
        private readonly Lazy<IEFRepository<ReusableProductId>> _reusableProductIdRepository;

        public IEFRepository<Product> ProductRepository => _productRepository.Value;
        public IEFRepository<ReusableProductId> ReusableProductIdRepository => _reusableProductIdRepository.Value;

        public ProductUOW(AppDbContext context)
        {
            _context = context;
            _productRepository = new Lazy<IEFRepository<Product>>(() => new EFRepository<Product>(_context));
            _reusableProductIdRepository = new Lazy<IEFRepository<ReusableProductId>>(() => new EFRepository<ReusableProductId>(_context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}