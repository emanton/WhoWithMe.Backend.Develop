using FluentValidation;
using WhoWithMe.DTO.Model.Authorization;

namespace WhoWithMe.Services.Validators
{
    public class LoginDataValidator : AbstractValidator<LoginData>
    {
        public LoginDataValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
