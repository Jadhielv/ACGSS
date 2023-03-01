using ACGSS.Domain.Entities;
using ACGSS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACGSS.Infrastructure.Repositories
{
    public class UserRepository : EFRepository<User>, IUserRepository
    {
        public UserRepository(DbSet<User> users) : base(users)
        {
        }
    }
}
