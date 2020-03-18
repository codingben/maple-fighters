using Authenticator.API.Datas;
using FluentValidation;

namespace Authenticator.API.Validators
{
    public class RegistrationDataValidator : AbstractValidator<RegistrationData>
    {
        public RegistrationDataValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        }
    }
}