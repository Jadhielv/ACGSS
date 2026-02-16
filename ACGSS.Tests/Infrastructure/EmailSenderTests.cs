using ACGSS.Domain.Models;
using ACGSS.Infrastructure.Mail;
using Microsoft.Extensions.Options;
using Moq;

namespace ACGSS.Tests.Infrastructure
{
    [TestFixture]
    public class EmailSenderTests
    {
        private EmailSettings _emailSettings;

        [SetUp]
        public void SetUp()
        {
            _emailSettings = new EmailSettings
            {
                ApiKey = "test-api-key",
                FromAddress = "noreply@acgss.com",
                FromName = "ACGSS System"
            };
        }

        [Test]
        public void EmailSender_Should_InitializeWithSettings()
        {
            // Arrange
            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(o => o.Value).Returns(_emailSettings);

            // Act
            var emailSender = new EmailSender(optionsMock.Object);

            // Assert
            Assert.That(emailSender, Is.Not.Null);
        }

        [Test]
        public void EmailSender_Should_AcceptValidEmailSettings()
        {
            // Arrange
            var validSettings = new EmailSettings
            {
                ApiKey = "SG.valid-api-key",
                FromAddress = "test@example.com",
                FromName = "Test Sender"
            };
            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(o => o.Value).Returns(validSettings);

            // Act
            var emailSender = new EmailSender(optionsMock.Object);

            // Assert
            Assert.That(emailSender, Is.Not.Null);
        }

        [Test]
        public void EmailSender_Should_HandleEmptyApiKey()
        {
            // Arrange
            var invalidSettings = new EmailSettings
            {
                ApiKey = string.Empty,
                FromAddress = "test@example.com",
                FromName = "Test Sender"
            };
            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(o => o.Value).Returns(invalidSettings);

            // Act
            var emailSender = new EmailSender(optionsMock.Object);

            // Assert
            Assert.That(emailSender, Is.Not.Null);
        }

        [Test]
        public void EmailSender_Should_HandleNullFromAddress()
        {
            // Arrange
            var invalidSettings = new EmailSettings
            {
                ApiKey = "test-key",
                FromAddress = null,
                FromName = "Test Sender"
            };
            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(o => o.Value).Returns(invalidSettings);

            // Act
            var emailSender = new EmailSender(optionsMock.Object);

            // Assert
            Assert.That(emailSender, Is.Not.Null);
        }

        [Test]
        public void EmailSender_Should_HandleNullFromName()
        {
            // Arrange
            var invalidSettings = new EmailSettings
            {
                ApiKey = "test-key",
                FromAddress = "test@example.com",
                FromName = null
            };
            var optionsMock = new Mock<IOptions<EmailSettings>>();
            optionsMock.Setup(o => o.Value).Returns(invalidSettings);

            // Act
            var emailSender = new EmailSender(optionsMock.Object);

            // Assert
            Assert.That(emailSender, Is.Not.Null);
        }

        [Test]
        public void Email_Should_HaveRequiredProperties()
        {
            // Arrange & Act
            var email = new Email
            {
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // Assert
            Assert.That(email.To, Is.EqualTo("recipient@example.com"));
            Assert.That(email.Subject, Is.EqualTo("Test Subject"));
            Assert.That(email.Body, Is.EqualTo("Test Body"));
        }

        [Test]
        public void Email_Should_AllowEmptyBody()
        {
            // Arrange & Act
            var email = new Email
            {
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = string.Empty
            };

            // Assert
            Assert.That(email.Body, Is.Empty);
        }

        [Test]
        public void Email_Should_AllowLongBody()
        {
            // Arrange
            var longBody = new string('A', 10000);

            // Act
            var email = new Email
            {
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = longBody
            };

            // Assert
            Assert.That(email.Body.Length, Is.EqualTo(10000));
        }

        [Test]
        public void EmailSettings_Should_HaveAllProperties()
        {
            // Arrange & Act
            var settings = new EmailSettings
            {
                ApiKey = "test-api-key",
                FromAddress = "sender@example.com",
                FromName = "Test Sender"
            };

            // Assert
            Assert.That(settings.ApiKey, Is.EqualTo("test-api-key"));
            Assert.That(settings.FromAddress, Is.EqualTo("sender@example.com"));
            Assert.That(settings.FromName, Is.EqualTo("Test Sender"));
        }

        [Test]
        public void Email_Should_HandleSpecialCharactersInSubject()
        {
            // Arrange & Act
            var email = new Email
            {
                To = "recipient@example.com",
                Subject = "Test: Special & Characters <> \"Quotes\"",
                Body = "Test Body"
            };

            // Assert
            Assert.That(email.Subject, Does.Contain("Special & Characters"));
        }

        [Test]
        public void Email_Should_HandleMultipleRecipients()
        {
            // Arrange & Act
            var email = new Email
            {
                To = "recipient1@example.com, recipient2@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // Assert
            Assert.That(email.To, Does.Contain("recipient1@example.com"));
            Assert.That(email.To, Does.Contain("recipient2@example.com"));
        }

        [Test]
        public void Email_Should_HandleHtmlBody()
        {
            // Arrange & Act
            var email = new Email
            {
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "<html><body><h1>Hello</h1></body></html>"
            };

            // Assert
            Assert.That(email.Body, Does.Contain("<html>"));
            Assert.That(email.Body, Does.Contain("<h1>Hello</h1>"));
        }
    }
}