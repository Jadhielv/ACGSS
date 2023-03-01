using ACGSS.Domain.Repositories;
using ACGSS.Infrastructure.Database;
using ACGSS.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ACGSS.Infrastructure.Extensions
{
    public static class UnitOfWorkExtension
    {
        public static IServiceCollection SetupUnitOfWork([NotNull] this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>(x =>
            {
                var scopeFactory = x.GetRequiredService<IServiceScopeFactory>();
                var context = x.GetService<EFContext>();
                return new UnitOfWork(context, new UserRepository(context.Users));
            });

            return serviceCollection;
        }
    }
}
