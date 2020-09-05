using Authenticator.API.Constants;
using Authenticator.API.Datas;
using FluentValidation;

namespace Authenticator.API.Validators
{
    public class LoginDataValidator : AbstractValidator<LoginData>
    {
        public LoginDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage(ErrorMessages.InvalidEmail);
            RuleFor(x => x.Password).NotEmpty().WithMessage(ErrorMessages.PasswordRequired);
        }
    }
}