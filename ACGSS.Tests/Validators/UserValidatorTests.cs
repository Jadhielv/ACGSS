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

        [Test]
        public void Should_have_error_when_FirstName_is_empty()
        {
            var model = new UserDto { FirstName = string.Empty };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.FirstName);
        }

        [Test]
        public void Should_have_error_when_LastName_is_empty()
        {
            var model = new UserDto { LastName = string.Empty };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.LastName);
        }

        [Test]
        public void Should_have_error_when_Email_is_empty()
        {
            var model = new UserDto { Email = string.Empty };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public void Should_not_have_error_when_FirstName_has_valid_length()
        {
            var model = new UserDto { FirstName = "John" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.FirstName);
        }

        [Test]
        public void Should_not_have_error_when_LastName_has_valid_length()
        {
            var model = new UserDto { LastName = "Doe" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.LastName);
        }

        [Test]
        public void Should_not_have_error_when_Address_is_null()
        {
            var model = new UserDto { Address = null };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.Address);
        }

        [Test]
        public void Should_not_have_error_when_PhoneNumber_is_null()
        {
            var model = new UserDto { PhoneNumber = null };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.PhoneNumber);
        }

        [Test]
        public void Should_validate_complete_valid_user()
        {
            var model = new UserDto
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                PhoneNumber = "555-1234",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Test]
        public void Should_fail_when_all_required_fields_are_null()
        {
            var model = new UserDto
            {
                FirstName = null,
                LastName = null,
                Email = null
            };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.FirstName);
            result.ShouldHaveValidationErrorFor(p => p.LastName);
            result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public void Should_not_have_error_when_Email_has_plus_sign()
        {
            var model = new UserDto { Email = "john.doe+test@example.com" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.Email);
        }

        [Test]
        public void Should_not_have_error_when_FirstName_has_special_characters()
        {
            var model = new UserDto { FirstName = "Jean-Pierre" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.FirstName);
        }

        [Test]
        public void Should_not_have_error_when_LastName_has_special_characters()
        {
            var model = new UserDto { LastName = "O'Brien" };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(p => p.LastName);
        }

        [Test]
        public void Should_have_error_when_CreatedDate_is_default()
        {
            var model = new UserDto { CreatedDate = default(DateTime) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.CreatedDate);
        }

        [Test]
        public void Should_have_error_when_ModifiedDate_is_default()
        {
            var model = new UserDto { ModifiedDate = default(DateTime) };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(p => p.ModifiedDate);
        }
    }
}