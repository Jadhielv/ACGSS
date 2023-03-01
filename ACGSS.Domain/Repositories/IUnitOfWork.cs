namespace ACGSS.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
        IUserRepository UserRepository { get; set; }
    }
}
