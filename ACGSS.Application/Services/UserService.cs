using ACGSS.Domain.DTOs;
using ACGSS.Domain.Entities;
using ACGSS.Domain.Exceptions;
using ACGSS.Domain.Repositories;
using ACGSS.Domain.Services;
using AutoMapper;

namespace ACGSS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var exists = await _unitOfWork.UserRepository.AnyAsync(x => x.Id == userDto.Id);
            if (!exists)
                throw new ConflictException("The user doesn't exist.");

            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            var exists = await _unitOfWork.UserRepository.AnyAsync(x => x.Id == userId);
            if (!exists)
                throw new ConflictException("The user doesn't exist.");

            var user = await _unitOfWork.UserRepository.GetFirstAsync(x => x.Id == userId);
            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<UserDto> GetUser(int userId)
        {
            var exists = await _unitOfWork.UserRepository.AnyAsync(x => x.Id == userId);
            if (!exists)
                throw new ConflictException("The user doesn't exist.");

            var user = await _unitOfWork.UserRepository.GetFirstAsync(x => x.Id == userId);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }
    }
}
