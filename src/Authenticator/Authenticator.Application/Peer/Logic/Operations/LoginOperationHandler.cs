using Authenticator.Application.Peer.Logic.Operations.Converters;
using Authenticator.Application.Peer.Logic.Operations.Validators;
using Authenticator.Common.Enums;
using Authenticator.Common.Parameters;
using Authenticator.Domain.Aggregates.User.Services;
using Common.ParametersValidation;
using CommonCommunicationInterfaces;
using FluentValidation;
using ServerCommunicationHelper;

namespace Authenticator.Application.Peer.Logic.Operations
{
    public class LoginOperationHandler : 
        IOperationRequestHandler<LoginRequestParameters, LoginResponseParameters>
    {
        private readonly ILoginService loginService;
        private readonly IValidator<LoginRequestParameters> loginParametersValidator;

        public LoginOperationHandler(ILoginService loginService)
        {
            this.loginService = loginService;

            loginParametersValidator = new LoginParametersValidator();
        }

        public LoginResponseParameters? Handle(
            MessageData<LoginRequestParameters> messageData,
            ref MessageSendOptions sendOptions)
        {
            var loginStatus = LoginStatus.Failed;

            var validationResult =
                loginParametersValidator.Validate(messageData.Parameters);
            if (validationResult.IsValid)
            {
                var authenticationStatus =
                    loginService.Authenticate(
                        messageData.Parameters.Email,
                        messageData.Parameters.Password);

                loginStatus = authenticationStatus.ToLoginStatus();
            }

            return new LoginResponseParameters(
                loginStatus,
                validationResult.ToErrorMessage());
        }
    }
}