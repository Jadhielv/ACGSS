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
            Assert.That(async () => await service.UpdateUser(user), Throws.TypeOf<ConflictException>(), "The user doesn't exist.");
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
            Assert.That(async () => await service.GetUser(1), Throws.TypeOf<ConflictException>(), "The user doesn't exist.");
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
            Assert.That(async () => await service.DeleteUser(1), Throws.TypeOf<ConflictException>(), "The user doesn't exist.");
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
    }
}