using ACGSS.Domain.Models;

namespace ACGSS.Domain.Services
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmail(Email email);
    }
}
