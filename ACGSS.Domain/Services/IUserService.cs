using ACGSS.Domain.DTOs;

namespace ACGSS.Domain.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUser(UserDto userDto);
        Task UpdateUser(UserDto userDto);
        Task DeleteUser(int userId);
        Task<UserDto> GetUser(int userId);
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
