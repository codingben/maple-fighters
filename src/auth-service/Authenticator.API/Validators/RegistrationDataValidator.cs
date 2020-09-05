using Authenticator.API.Constants;
using Authenticator.API.Datas;
using FluentValidation;

namespace Authenticator.API.Validators
{
    public class RegistrationDataValidator : AbstractValidator<RegistrationData>
    {
        public RegistrationDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage(ErrorMessages.InvalidEmail);
            RuleFor(x => x.Password).NotEmpty().WithMessage(ErrorMessages.PasswordRequired);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.FirstNameRequired);
            RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.LastNameRequired);
        }
    }
}