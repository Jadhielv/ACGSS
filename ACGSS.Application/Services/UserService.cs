using ACGSS.Domain.DTOs;
using ACGSS.Domain.Entities;
using ACGSS.Domain.Enums;
using ACGSS.Domain.Exceptions;
using ACGSS.Domain.Models;
using ACGSS.Domain.Repositories;
using ACGSS.Domain.Services;
using AutoMapper;
using System.Linq.Expressions;

namespace ACGSS.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailSenderService emailSenderService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailSenderService = emailSenderService;
        }

        public async Task<UserDto> AddUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = userDto.Email,
                Body = "Your user has been created successfully.",
                Subject = "Welcome to ACGSS System"
            };

            await _emailSenderService.SendEmail(email);

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

            var email = new Email
            {
                To = userDto.Email,
                Body = userDto.IsActive == UserStatus.Active ? "Your user has been updated successfully."
                                                             : "Your user has been deleted successfully.",
                Subject = "Welcome to ACGSS System"
            };

            await _emailSenderService.SendEmail(email);
        }

        public async Task DeleteUser(int userId)
        {
            var exists = await _unitOfWork.UserRepository.AnyAsync(x => x.Id == userId);
            if (!exists)
                throw new ConflictException("The user doesn't exist.");

            var user = await _unitOfWork.UserRepository.GetFirstAsync(x => x.Id == userId);
            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var email = new Email
            {
                To = user.Email,
                Body = "Your user has been deleted successfully.",
                Subject = "Welcome to ACGSS System"
            };

            await _emailSenderService.SendEmail(email);
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
            var users = await _unitOfWork.UserRepository.GetAllAsync(new List<Expression<Func<User, bool>>>()
                                                                    { x => x.IsActive == UserStatus.Active });
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

            return usersDto;
        }
    }
}
