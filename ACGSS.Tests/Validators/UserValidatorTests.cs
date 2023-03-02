using ACGSS.Domain.DTOs;
using ACGSS.Infrastructure.Validators;
using FluentValidation.TestHelper;

namespace ACGSS.Tests.Validators
{
    [TestFixture]
    public class UserValidatorTests
    {
        private UserValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new UserValidator();
        }

        [Test]
        public void Should_have_error_when_FirstName_is_null()
        {
            var model = new UserDto { FirstName = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.FirstName);
        }

        [Test]
        public void Should_not_have_error_when_FirstName_is_specified()
        {
            var model = new UserDto { FirstName = "Karl Matti" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.FirstName);
        }

        [Test]
        public void Should_have_error_when_LastName_is_null()
        {
            var model = new UserDto { LastName = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.LastName);
        }

        [Test]
        public void Should_not_have_error_when_LastName_is_specified()
        {
            var model = new UserDto { LastName = "Jablonski Karttunen" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.LastName);
        }

        [Test]
        public void Should_have_error_when_Email_is_null()
        {
            var model = new UserDto { Email = null };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public void Should_not_have_error_when_Email_is_specified()
        {
            var model = new UserDto { Email = "jadhielv@gmail.com" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public void Should_not_have_error_when_CreatedDate_is_specified()
        {
            var model = new UserDto { CreatedDate = DateTime.Now };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.CreatedDate);
        }

        [Test]
        public void Should_not_have_error_when_ModifiedDate_is_specified()
        {
            var model = new UserDto { ModifiedDate = DateTime.Now };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.ModifiedDate);
        }
    }
}
