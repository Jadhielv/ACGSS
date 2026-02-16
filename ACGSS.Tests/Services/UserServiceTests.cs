using ACGSS.Application.Services;
using ACGSS.Domain.DTOs;
using ACGSS.Domain.Entities;
using ACGSS.Domain.Exceptions;
using ACGSS.Domain.Repositories;
using ACGSS.Domain.Services;
using AutoMapper;
using Moq;
using System.Linq.Expressions;

namespace ACGSS.Tests.Services
{
    public class UserServiceTests
    {
        [Test]
        public void UpdateUser_UserNotFound_Should_ThrowsConflictException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var user = new UserDto
            {
                Id = 1,
                FirstName = "Karl Matti",
                LastName = "Jablonski Karttunen",
                Email = "test@gmail.com"
            };

            var service = new UserService(unitOfWorkMock.Object, null, null);

            userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(false);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act => Assert
            Assert.ThrowsAsync<ConflictException>(() => service.UpdateUser(user), "The user doesn't exist.");
        }

        [Test]
        public async Task UpdateUser_UserFound_Should_UpdateUser()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();

            var user = new UserDto
            {
                Id = 1,
                FirstName = "Karl Matti",
                LastName = "Jablonski Karttunen",
                Email = "test@gmail.com"
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            userRepositoryMock.Setup(v => v.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true);

            unitOfWorkMock.Setup(v => v.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            await service.UpdateUser(user);

            // Assert
            userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()));
            unitOfWorkMock.Verify(x => x.SaveChangesAsync());
        }

        [Test]
        public void GetUser_UserNotFound_Should_ThrowsConflictException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var service = new UserService(unitOfWorkMock.Object, null, null);

            userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(false);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act => Assert
            Assert.ThrowsAsync<ConflictException>(() => service.GetUser(1), "The user doesn't exist.");
        }

        [Test]
        public async Task GetUser_UserFound_Should_ReturnsUser()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            const int userId = 1;

            var user = new User
            {
                Id = userId,
                FirstName = "Karl Matti",
                LastName = "Jablonski Karttunen",
                Email = "test@gmail.com"
            };

            var userDto = new UserDto();

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, null);

            userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true);

            userRepositoryMock.Setup(x => x.GetFirstAsync(x => x.Id == userId))
                .ReturnsAsync(user);

            mapperMock.Setup(x => x.Map<UserDto>(user))
                .Returns(userDto);

            unitOfWorkMock.Setup(v => v.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act 
            var result = await service.GetUser(userId);

            // Assert
            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public void DeleteUser_UserNotFound_Should_ThrowsConflictException()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();

            var service = new UserService(unitOfWorkMock.Object, null, null);

            userRepositoryMock.Setup(v => v.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(false);

            unitOfWorkMock.Setup(v => v.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act => Assert
            Assert.ThrowsAsync<ConflictException>(() => service.DeleteUser(1), "The user doesn't exist.");
        }

        [Test]
        public async Task DeleteUser_UserFound_Should_DeleteUser()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();
            const int userId = 1;

            var user = new User
            {
                Id = userId,
                FirstName = "Karl Matti",
                LastName = "Jablonski Karttunen",
                Email = "test@gmail.com"
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            userRepositoryMock.Setup(v => v.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true);

            userRepositoryMock.Setup(x => x.GetFirstAsync(x => x.Id == userId))
                .ReturnsAsync(user);

            unitOfWorkMock.Setup(v => v.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            await service.DeleteUser(userId);

            // Assert
            userRepositoryMock.Verify(x => x.DeleteAsync(user));
            unitOfWorkMock.Verify(x => x.SaveChangesAsync());
        }

        [Test]
        public async Task AddUser_ValidUser_Should_AddUserAndSendEmail()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();

            var userDto = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com"
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            mapperMock.Setup(x => x.Map<User>(userDto))
                .Returns(user);

            mapperMock.Setup(x => x.Map<UserDto>(user))
                .Returns(userDto);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            emailSenderServiceMock.Setup(x => x.SendEmail(It.IsAny<Domain.Models.Email>()))
                .ReturnsAsync(true);

            // Act
            var result = await service.AddUser(userDto);

            // Assert
            userRepositoryMock.Verify(x => x.AddAsync(user), Times.Once);
            unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            emailSenderServiceMock.Verify(x => x.SendEmail(It.Is<Domain.Models.Email>(
                e => e.To == userDto.Email &&
                     e.Subject == "Welcome to ACGSS System" &&
                     e.Body == "Your user has been created successfully."
            )), Times.Once);
            Assert.That(result, Is.EqualTo(userDto));
        }

        [Test]
        public async Task AddUser_ValidUser_Should_MapUserCorrectly()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();

            var userDto = new UserDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Address = "123 Main St",
                PhoneNumber = "555-1234"
            };

            var user = new User();

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            mapperMock.Setup(x => x.Map<User>(userDto))
                .Returns(user);

            mapperMock.Setup(x => x.Map<UserDto>(user))
                .Returns(userDto);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            await service.AddUser(userDto);

            // Assert
            mapperMock.Verify(x => x.Map<User>(userDto), Times.Once);
            mapperMock.Verify(x => x.Map<UserDto>(user), Times.Once);
        }

        [Test]
        public async Task GetUsers_Should_ReturnOnlyActiveUsers()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var users = new List<User>
            {
                new User { Id = 1, FirstName = "Active", LastName = "User", Email = "active@test.com", IsActive = Domain.Enums.UserStatus.Active },
                new User { Id = 2, FirstName = "Another", LastName = "Active", Email = "active2@test.com", IsActive = Domain.Enums.UserStatus.Active }
            };

            var usersDto = new List<UserDto>
            {
                new UserDto { Id = 1, FirstName = "Active", LastName = "User", Email = "active@test.com" },
                new UserDto { Id = 2, FirstName = "Another", LastName = "Active", Email = "active2@test.com" }
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, null);

            userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<List<Expression<Func<User, bool>>>>()))
                .ReturnsAsync(users);

            mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(users))
                .Returns(usersDto);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            var result = await service.GetUsers();

            // Assert
            Assert.That(result, Is.EqualTo(usersDto));
            Assert.That(result.Count(), Is.EqualTo(2));
            userRepositoryMock.Verify(x => x.GetAllAsync(It.Is<List<Expression<Func<User, bool>>>>(
                list => list.Count == 1
            )), Times.Once);
        }

        [Test]
        public async Task GetUsers_NoActiveUsers_Should_ReturnEmptyList()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();

            var emptyUsers = new List<User>();
            var emptyUsersDto = new List<UserDto>();

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, null);

            userRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<List<Expression<Func<User, bool>>>>()))
                .ReturnsAsync(emptyUsers);

            mapperMock.Setup(x => x.Map<IEnumerable<UserDto>>(emptyUsers))
                .Returns(emptyUsersDto);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            var result = await service.GetUsers();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task UpdateUser_ActiveUser_Should_SendCorrectEmailMessage()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();

            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsActive = Domain.Enums.UserStatus.Active
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            await service.UpdateUser(userDto);

            // Assert
            emailSenderServiceMock.Verify(x => x.SendEmail(It.Is<Domain.Models.Email>(
                e => e.Body == "Your user has been updated successfully."
            )), Times.Once);
        }

        [Test]
        public async Task UpdateUser_InactiveUser_Should_SendDeletionEmailMessage()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>();
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var emailSenderServiceMock = new Mock<IEmailSenderService>();

            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsActive = Domain.Enums.UserStatus.Inactive
            };

            var service = new UserService(unitOfWorkMock.Object, mapperMock.Object, emailSenderServiceMock.Object);

            userRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(true);

            unitOfWorkMock.Setup(x => x.UserRepository)
                .Returns(userRepositoryMock.Object);

            // Act
            await service.UpdateUser(userDto);

            // Assert
            emailSenderServiceMock.Verify(x => x.SendEmail(It.Is<Domain.Models.Email>(
                e => e.Body == "Your user has been deleted successfully."
            )), Times.Once);
        }
    }
}