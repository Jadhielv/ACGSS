using ACGSS.Domain.Entities;
using ACGSS.Domain.Enums;

namespace ACGSS.Tests.Domain
{
    [TestFixture]
    public class UserEntityTests
    {
        [Test]
        public void User_Should_InitializeWithDefaultValues()
        {
            // Act
            var user = new User();

            // Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Id, Is.EqualTo(0));
            Assert.That(user.IsActive, Is.EqualTo(UserStatus.Inactive));
        }

        [Test]
        public void User_Should_AllowSettingAllProperties()
        {
            // Arrange & Act
            var user = new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                Email = "john.doe@example.com",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsActive = UserStatus.Active
            };

            // Assert
            Assert.That(user.Id, Is.EqualTo(1));
            Assert.That(user.FirstName, Is.EqualTo("John"));
            Assert.That(user.LastName, Is.EqualTo("Doe"));
            Assert.That(user.Address, Is.EqualTo("123 Main St"));
            Assert.That(user.PhoneNumber, Is.EqualTo("555-1234"));
            Assert.That(user.Email, Is.EqualTo("john.doe@example.com"));
            Assert.That(user.IsActive, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void User_Should_AllowNullableFields()
        {
            // Arrange & Act
            var user = new User
            {
                FirstName = null,
                LastName = null,
                Address = null,
                PhoneNumber = null,
                Email = null
            };

            // Assert
            Assert.That(user.FirstName, Is.Null);
            Assert.That(user.LastName, Is.Null);
            Assert.That(user.Address, Is.Null);
            Assert.That(user.PhoneNumber, Is.Null);
            Assert.That(user.Email, Is.Null);
        }

        [Test]
        public void User_Should_AllowEmptyStrings()
        {
            // Arrange & Act
            var user = new User
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Email = string.Empty
            };

            // Assert
            Assert.That(user.FirstName, Is.Empty);
            Assert.That(user.LastName, Is.Empty);
            Assert.That(user.Email, Is.Empty);
        }

        [Test]
        public void User_CreatedDate_Should_StoreDateTime()
        {
            // Arrange
            var now = DateTime.Now;
            var user = new User
            {
                CreatedDate = now
            };

            // Assert
            Assert.That(user.CreatedDate, Is.EqualTo(now));
        }

        [Test]
        public void User_ModifiedDate_Should_StoreDateTime()
        {
            // Arrange
            var now = DateTime.Now;
            var user = new User
            {
                ModifiedDate = now
            };

            // Assert
            Assert.That(user.ModifiedDate, Is.EqualTo(now));
        }

        [Test]
        public void User_IsActive_Should_DefaultToInactive()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.That(user.IsActive, Is.EqualTo(UserStatus.Inactive));
        }

        [Test]
        public void User_IsActive_Should_AcceptActiveStatus()
        {
            // Arrange & Act
            var user = new User
            {
                IsActive = UserStatus.Active
            };

            // Assert
            Assert.That(user.IsActive, Is.EqualTo(UserStatus.Active));
        }

        [Test]
        public void User_Should_AllowLongNames()
        {
            // Arrange
            var longName = new string('A', 500);

            // Act
            var user = new User
            {
                FirstName = longName,
                LastName = longName
            };

            // Assert
            Assert.That(user.FirstName.Length, Is.EqualTo(500));
            Assert.That(user.LastName.Length, Is.EqualTo(500));
        }

        [Test]
        public void User_Should_AllowLongAddress()
        {
            // Arrange
            var longAddress = new string('B', 1000);

            // Act
            var user = new User
            {
                Address = longAddress
            };

            // Assert
            Assert.That(user.Address.Length, Is.EqualTo(1000));
        }

        [Test]
        public void User_Should_AllowSpecialCharactersInName()
        {
            // Arrange & Act
            var user = new User
            {
                FirstName = "Jean-Pierre",
                LastName = "O'Brien"
            };

            // Assert
            Assert.That(user.FirstName, Does.Contain("-"));
            Assert.That(user.LastName, Does.Contain("'"));
        }

        [Test]
        public void User_Should_AllowInternationalCharacters()
        {
            // Arrange & Act
            var user = new User
            {
                FirstName = "José",
                LastName = "Müller"
            };

            // Assert
            Assert.That(user.FirstName, Is.EqualTo("José"));
            Assert.That(user.LastName, Is.EqualTo("Müller"));
        }

        [Test]
        public void User_Should_HandleDateTimeMinValue()
        {
            // Arrange & Act
            var user = new User
            {
                CreatedDate = DateTime.MinValue,
                ModifiedDate = DateTime.MinValue
            };

            // Assert
            Assert.That(user.CreatedDate, Is.EqualTo(DateTime.MinValue));
            Assert.That(user.ModifiedDate, Is.EqualTo(DateTime.MinValue));
        }

        [Test]
        public void User_Should_HandleDateTimeMaxValue()
        {
            // Arrange & Act
            var user = new User
            {
                CreatedDate = DateTime.MaxValue,
                ModifiedDate = DateTime.MaxValue
            };

            // Assert
            Assert.That(user.CreatedDate, Is.EqualTo(DateTime.MaxValue));
            Assert.That(user.ModifiedDate, Is.EqualTo(DateTime.MaxValue));
        }

        [Test]
        public void UserStatus_Active_Should_EqualOne()
        {
            // Assert
            Assert.That((int)UserStatus.Active, Is.EqualTo(1));
        }

        [Test]
        public void UserStatus_Inactive_Should_EqualZero()
        {
            // Assert
            Assert.That((int)UserStatus.Inactive, Is.EqualTo(0));
        }

        [Test]
        public void UserStatus_Should_BeComparable()
        {
            // Arrange
            var active = UserStatus.Active;
            var inactive = UserStatus.Inactive;

            // Assert
            Assert.That(active, Is.Not.EqualTo(inactive));
            Assert.That(active > inactive, Is.True);
        }
    }
}