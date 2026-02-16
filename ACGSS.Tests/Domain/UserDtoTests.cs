using ACGSS.Domain.DTOs;
using ACGSS.Domain.Enums;

namespace ACGSS.Tests.Domain
{
    [TestFixture]
    public class UserDtoTests
    {
        [Test]
        public void UserDto_Should_InitializeWithDefaultValues()
        {
            // Act
            var userDto = new UserDto();

            // Assert
            Assert.That(userDto, Is.Not.Null);
            Assert.That(userDto.Id, Is.EqualTo(0));
            Assert.That(userDto.IsActive, Is.EqualTo(UserStatus.Inactive));
        }

        [Test]
        public void UserDto_Should_AllowSettingAllProperties()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Oak Ave",
                PhoneNumber = "555-5678",
                Email = "jane.smith@example.com",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = UserStatus.Active
            };

            // Assert
            Assert.That(userDto.Id, Is.EqualTo(1));
            Assert.That(userDto.FirstName, Is.EqualTo("Jane"));
            Assert.That(userDto.LastName, Is.EqualTo("Smith"));
            Assert.That(userDto.Address, Is.EqualTo("456 Oak Ave"));
            Assert.That(userDto.PhoneNumber, Is.EqualTo("555-5678"));
            Assert.That(userDto.Email, Is.EqualTo("jane.smith@example.com"));
            Assert.That(userDto.IsActive, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void UserDto_Should_AllowNullableFields()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                FirstName = null,
                LastName = null,
                Address = null,
                PhoneNumber = null,
                Email = null
            };

            // Assert
            Assert.That(userDto.FirstName, Is.Null);
            Assert.That(userDto.LastName, Is.Null);
            Assert.That(userDto.Address, Is.Null);
            Assert.That(userDto.PhoneNumber, Is.Null);
            Assert.That(userDto.Email, Is.Null);
        }

        [Test]
        public void UserDto_Should_BeTransferableObject()
        {
            // Arrange
            var userDto1 = new UserDto
            {
                Id = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com"
            };

            // Act - Simulate transferring data
            var userDto2 = new UserDto
            {
                Id = userDto1.Id,
                FirstName = userDto1.FirstName,
                LastName = userDto1.LastName,
                Email = userDto1.Email
            };

            // Assert
            Assert.That(userDto2.Id, Is.EqualTo(userDto1.Id));
            Assert.That(userDto2.FirstName, Is.EqualTo(userDto1.FirstName));
            Assert.That(userDto2.LastName, Is.EqualTo(userDto1.LastName));
            Assert.That(userDto2.Email, Is.EqualTo(userDto1.Email));
        }

        [Test]
        public void UserDto_Should_HandleZeroId()
        {
            // Arrange & Act
            var userDto = new UserDto { Id = 0 };

            // Assert
            Assert.That(userDto.Id, Is.EqualTo(0));
        }

        [Test]
        public void UserDto_Should_HandleNegativeId()
        {
            // Arrange & Act
            var userDto = new UserDto { Id = -1 };

            // Assert
            Assert.That(userDto.Id, Is.EqualTo(-1));
        }

        [Test]
        public void UserDto_Should_HandleLargeId()
        {
            // Arrange & Act
            var userDto = new UserDto { Id = int.MaxValue };

            // Assert
            Assert.That(userDto.Id, Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void UserDto_Should_AllowComplexEmailFormats()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                Email = "user+tag@subdomain.example.com"
            };

            // Assert
            Assert.That(userDto.Email, Does.Contain("+"));
            Assert.That(userDto.Email, Does.Contain("@"));
        }

        [Test]
        public void UserDto_Should_AllowInternationalPhoneNumbers()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                PhoneNumber = "+1-555-123-4567"
            };

            // Assert
            Assert.That(userDto.PhoneNumber, Does.Contain("+"));
            Assert.That(userDto.PhoneNumber, Does.Contain("-"));
        }

        [Test]
        public void UserDto_CreatedDate_Should_BeIndependentOfModifiedDate()
        {
            // Arrange
            var created = DateTime.Now;
            var modified = created.AddDays(1);

            // Act
            var userDto = new UserDto
            {
                CreatedDate = created,
                ModifiedDate = modified
            };

            // Assert
            Assert.That(userDto.CreatedDate, Is.LessThan(userDto.ModifiedDate));
            Assert.That(userDto.ModifiedDate, Is.GreaterThan(userDto.CreatedDate));
        }

        [Test]
        public void UserDto_Should_AllowSameCreatedAndModifiedDate()
        {
            // Arrange
            var now = DateTime.Now;

            // Act
            var userDto = new UserDto
            {
                CreatedDate = now,
                ModifiedDate = now
            };

            // Assert
            Assert.That(userDto.CreatedDate, Is.EqualTo(userDto.ModifiedDate));
        }

        [Test]
        public void UserDto_Should_AllowMultilineAddress()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                Address = "123 Main St\nApt 4B\nNew York, NY 10001"
            };

            // Assert
            Assert.That(userDto.Address, Does.Contain("\n"));
            Assert.That(userDto.Address, Does.Contain("Apt 4B"));
        }

        [Test]
        public void UserDto_Should_HandleUnicodeCharacters()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                FirstName = "François",
                LastName = "Müller",
                Address = "北京市"
            };

            // Assert
            Assert.That(userDto.FirstName, Is.EqualTo("François"));
            Assert.That(userDto.LastName, Is.EqualTo("Müller"));
            Assert.That(userDto.Address, Is.EqualTo("北京市"));
        }

        [Test]
        public void UserDto_IsActive_Should_ToggleBetweenStates()
        {
            // Arrange
            var userDto = new UserDto
            {
                IsActive = UserStatus.Active
            };

            // Act
            userDto.IsActive = UserStatus.Inactive;

            // Assert
            Assert.That(userDto.IsActive, Is.EqualTo(UserStatus.Inactive));

            // Act again
            userDto.IsActive = UserStatus.Active;

            // Assert
            Assert.That(userDto.IsActive, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void UserDto_Should_AllowEmptyPhoneNumber()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                PhoneNumber = string.Empty
            };

            // Assert
            Assert.That(userDto.PhoneNumber, Is.Empty);
        }

        [Test]
        public void UserDto_Should_AllowEmptyAddress()
        {
            // Arrange & Act
            var userDto = new UserDto
            {
                Address = string.Empty
            };

            // Assert
            Assert.That(userDto.Address, Is.Empty);
        }
    }
}