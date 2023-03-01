using ACGSS.Domain.DTOs;
using FluentValidation;

namespace ACGSS.Infrastructure.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.CreatedDate).NotNull();
            RuleFor(x => x.ModifiedDate).NotNull();
        }
    }
}
