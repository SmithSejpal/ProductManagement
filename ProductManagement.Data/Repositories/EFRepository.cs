using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Infrastructure.Data;

namespace ProductManagement.Infrastructure.Repositories
{
    /// <summary>
    /// Generic repository implementation for Entity Framework Core.
    /// Provides basic CRUD operations and query methods.
    /// </summary>
    public class EFRepository<T> : IEFRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public EFRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(string id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();

        public async Task<string> GetNextProductIdAsync()
        {
            var reusableProductId = await _context.Set<ReusableProductId>().FirstOrDefaultAsync();

            if (reusableProductId != null)
            {
                _context.Set<ReusableProductId>().Remove(reusableProductId);
                await _context.SaveChangesAsync();
                return reusableProductId.Id;
            }

            var connection = _context.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT NEXT VALUE FOR ProductIdSequence";
            var result = await command.ExecuteScalarAsync();
            return ((int)result).ToString("D6"); 
        }
    }
}