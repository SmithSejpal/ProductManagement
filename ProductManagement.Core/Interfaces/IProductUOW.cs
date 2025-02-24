using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces.Repositories;

namespace ProductManagement.Core.Interfaces
{
    /// <summary>
    /// Unit of Work interface for managing product-related database operations.
    /// </summary>
    public interface IProductUOW
    {
        IEFRepository<Product> ProductRepository { get; }
        IEFRepository<ReusableProductId> ReusableProductIdRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
