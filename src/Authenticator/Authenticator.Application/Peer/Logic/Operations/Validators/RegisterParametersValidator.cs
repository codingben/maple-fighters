using Authenticator.Common;
using FluentValidation;

namespace Authenticator.Application.Peer.Logic.Operations.Validators
{
    public class RegisterParametersValidator : AbstractValidator<RegisterRequestParameters>
    {
        public RegisterParametersValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }
    }
}