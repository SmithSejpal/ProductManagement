using System.Linq.Expressions;

namespace ProductManagement.Core.Interfaces.Repositories
{
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    public interface IEFRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<int> SaveChangesAsync();
        Task<string> GetNextProductIdAsync();
    }
}
