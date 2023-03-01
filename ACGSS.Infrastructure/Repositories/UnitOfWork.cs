using ACGSS.Domain.Repositories;
using ACGSS.Infrastructure.Database;

namespace ACGSS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get; set; }
        private readonly EFContext _dbContext;

        public UnitOfWork(EFContext dbContext, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            UserRepository = userRepository;
        }

        public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

        public void Dispose() => _dbContext.Dispose();
    }
}
