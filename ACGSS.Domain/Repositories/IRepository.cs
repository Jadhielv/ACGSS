using System.Linq.Expressions;

namespace ACGSS.Domain.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task GetFirstAsync(Expression<Func<T, bool>> expression);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
    }
}
