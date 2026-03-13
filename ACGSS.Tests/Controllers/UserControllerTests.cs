using ACGSS.Domain.DTOs;
using ACGSS.Domain.Services;
using ACGSS.Web.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ACGSS.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IValidator<UserDto>> _validatorMock;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _userServiceMock = new Mock<IUserService>();
            _validatorMock = new Mock<IValidator<UserDto>>();
            _controller = new UserController(_userServiceMock.Object, _validatorMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller.Dispose();
        }

        [Test]
        public async Task Edit_Post_ReturnsNotFound_WhenIdDoesNotMatchUserDtoId()
        {
            // Arrange
            int id = 1;
            var userDto = new UserDto { Id = 2 };

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UserDto>(), default))
                .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            // Act
            var result = await _controller.Edit(id, userDto);

            // Assert
            Assert.That(result, Is.TypeOf<NotFoundResult>());
        }
    }
}
