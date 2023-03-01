using System.Linq.Expressions;

namespace ACGSS.Infrastructure.Helpers
{
    public static class RepositoryHelper
    {
        public static IQueryable<T> Filter<T>(this IQueryable<T> source, IEnumerable<Expression<Func<T, bool>>> predicates) where T : class
        {
            if (predicates != null && predicates.Any())
                foreach (var predicate in predicates)
                    source = source.Where(predicate);

            return source;
        }
    }
}
