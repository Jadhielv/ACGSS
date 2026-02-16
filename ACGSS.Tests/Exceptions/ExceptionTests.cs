using ACGSS.Domain.Exceptions;

namespace ACGSS.Tests.Exceptions
{
    [TestFixture]
    public class ExceptionTests
    {
        [Test]
        public void BaseException_Should_InitializeWithMessage()
        {
            // Arrange
            var message = "Test exception message";

            // Act
            var exception = new BaseException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void BaseException_Should_InitializeWithMessageAndInnerException()
        {
            // Arrange
            var message = "Test exception message";
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new BaseException(message, innerException);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
            Assert.That(exception.InnerException.Message, Is.EqualTo("Inner exception"));
        }

        [Test]
        public void BaseException_Should_BeThrowable()
        {
            // Arrange
            var message = "Test exception message";

            // Act & Assert
            Assert.Throws<BaseException>(() => throw new BaseException(message));
        }

        [Test]
        public void ConflictException_Should_InitializeWithMessage()
        {
            // Arrange
            var message = "Conflict occurred";

            // Act
            var exception = new ConflictException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.Null);
        }

        [Test]
        public void ConflictException_Should_InitializeWithMessageAndInnerException()
        {
            // Arrange
            var message = "Conflict occurred";
            var innerException = new ArgumentException("Inner exception");

            // Act
            var exception = new ConflictException(message, innerException);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.InnerException, Is.EqualTo(innerException));
        }

        [Test]
        public void ConflictException_Should_BeThrowable()
        {
            // Arrange
            var message = "Conflict occurred";

            // Act & Assert
            Assert.Throws<ConflictException>(() => throw new ConflictException(message));
        }

        [Test]
        public void ConflictException_Should_InheritFromBaseException()
        {
            // Arrange
            var message = "Conflict occurred";

            // Act
            var exception = new ConflictException(message);

            // Assert
            Assert.That(exception, Is.InstanceOf<BaseException>());
            Assert.That(exception, Is.InstanceOf<Exception>());
        }

        [Test]
        public void BaseException_Should_HandleEmptyMessage()
        {
            // Arrange
            var message = string.Empty;

            // Act
            var exception = new BaseException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void ConflictException_Should_HandleEmptyMessage()
        {
            // Arrange
            var message = string.Empty;

            // Act
            var exception = new ConflictException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
        }

        [Test]
        public void BaseException_Should_HandleLongMessage()
        {
            // Arrange
            var message = new string('A', 1000);

            // Act
            var exception = new BaseException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.Message.Length, Is.EqualTo(1000));
        }

        [Test]
        public void ConflictException_Should_HandleLongMessage()
        {
            // Arrange
            var message = new string('B', 1000);

            // Act
            var exception = new ConflictException(message);

            // Assert
            Assert.That(exception.Message, Is.EqualTo(message));
            Assert.That(exception.Message.Length, Is.EqualTo(1000));
        }

        [Test]
        public void BaseException_Should_HandleSpecialCharactersInMessage()
        {
            // Arrange
            var message = "Error: <special> & \"characters\" 'test'";

            // Act
            var exception = new BaseException(message);

            // Assert
            Assert.That(exception.Message, Does.Contain("<special>"));
            Assert.That(exception.Message, Does.Contain("&"));
        }

        [Test]
        public void ConflictException_Should_HandleSpecialCharactersInMessage()
        {
            // Arrange
            var message = "Conflict: <special> & \"characters\" 'test'";

            // Act
            var exception = new ConflictException(message);

            // Assert
            Assert.That(exception.Message, Does.Contain("<special>"));
            Assert.That(exception.Message, Does.Contain("&"));
        }

        [Test]
        public void BaseException_Should_PreserveStackTrace()
        {
            // Arrange & Act
            BaseException caughtException = null;
            try
            {
                throw new BaseException("Test exception");
            }
            catch (BaseException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.That(caughtException, Is.Not.Null);
            Assert.That(caughtException.StackTrace, Is.Not.Null);
            Assert.That(caughtException.StackTrace, Does.Contain("ExceptionTests"));
        }

        [Test]
        public void ConflictException_Should_PreserveStackTrace()
        {
            // Arrange & Act
            ConflictException caughtException = null;
            try
            {
                throw new ConflictException("Test conflict");
            }
            catch (ConflictException ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.That(caughtException, Is.Not.Null);
            Assert.That(caughtException.StackTrace, Is.Not.Null);
            Assert.That(caughtException.StackTrace, Does.Contain("ExceptionTests"));
        }

        [Test]
        public void BaseException_Should_BeCatchableAsException()
        {
            // Arrange
            var wasCaught = false;

            // Act
            try
            {
                throw new BaseException("Test exception");
            }
            catch (Exception)
            {
                wasCaught = true;
            }

            // Assert
            Assert.That(wasCaught, Is.True);
        }

        [Test]
        public void ConflictException_Should_BeCatchableAsBaseException()
        {
            // Arrange
            var wasCaught = false;

            // Act
            try
            {
                throw new ConflictException("Test conflict");
            }
            catch (BaseException)
            {
                wasCaught = true;
            }

            // Assert
            Assert.That(wasCaught, Is.True);
        }
    }
}