using System.Linq.Expressions;

namespace ACGSS.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicates);
        Task<IEnumerable<T>> GetAllAsync(IEnumerable<Expression<Func<T, bool>>> predicates);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicates);
    }
}
