using Authenticator.API.Datas;
using FluentValidation;

namespace Authenticator.API.Validators
{
    public class AuthenticationDataValidator : AbstractValidator<AuthenticationData>
    {
        public AuthenticationDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}