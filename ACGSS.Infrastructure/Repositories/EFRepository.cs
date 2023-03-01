using ACGSS.Domain.Repositories;
using ACGSS.Infrastructure.Helpers;
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

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicates) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicates);

        public async Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> predicates) => await _dbSet.AsNoTracking()
                                                                                                                        .Filter(predicates)
                                                                                                                        .ToListAsync();

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicates) => await _dbSet.AnyAsync(predicates);
    }
}
