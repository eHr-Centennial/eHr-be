using FluentValidation;
using NetCore6.Bl.DTOs.User;

namespace NetCore6.Bl.Validations.User
{
    public class UserValidator: AbstractValidator<UserModelDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Username is required");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(8)
                .WithMessage("Password's length must be at least 8 characters");
        }
    }
}