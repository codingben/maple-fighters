using Authenticator.Common.Parameters;
using FluentValidation;

namespace Authenticator.Application.Peer.Logic.Operations.Validators
{
    public class LoginParametersValidator : AbstractValidator<LoginRequestParameters>
    {
        public LoginParametersValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress()
                .WithMessage("Invalid email address.");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}