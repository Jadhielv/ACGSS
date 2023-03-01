using ACGSS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ACGSS.Infrastructure.Repositories
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;
        public EFRepository(DbSet<T> dbSet) => _dbSet = dbSet;

        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task UpdateAsync(T entity) => await Task.Run(() => { _dbSet.Update(entity); });

        public async Task DeleteAsync(T entity) => await Task.Run(() => { _dbSet.Remove(entity); });

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> expression) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression) => await _dbSet.AsNoTracking().Where(expression).ToListAsync();
    }
}
