using Authenticator.Application.Peer.Logic.Operations.Converters;
using Authenticator.Application.Peer.Logic.Operations.Validators;
using Authenticator.Common.Enums;
using Authenticator.Common.Parameters;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Common.ParametersValidation;
using CommonCommunicationInterfaces;
using FluentValidation;
using ServerCommunicationHelper;

namespace Authenticator.Application.Peer.Logic.Operations
{
    public class RegisterOperationHandler : 
        IOperationRequestHandler<RegisterRequestParameters, RegisterResponseParameters>
    {
        private readonly IRegistrationService registrationService;
        private readonly IValidator<RegisterRequestParameters> registerParametersValidator;

        public RegisterOperationHandler(
            IRegistrationService registrationService)
        {
            this.registrationService = registrationService;

            registerParametersValidator = new RegisterParametersValidator();
        }

        public RegisterResponseParameters? Handle(
            MessageData<RegisterRequestParameters> messageData,
            ref MessageSendOptions sendOptions)
        {
            var registrationStatus = RegistrationStatus.Failed;

            var validationResult =
                registerParametersValidator.Validate(messageData.Parameters);
            if (validationResult.IsValid)
            {
                var account = AccountFactory.CreateAccount(
                    messageData.Parameters.Email,
                    messageData.Parameters.Password,
                    messageData.Parameters.FirstName,
                    messageData.Parameters.LastName);

                var accountCreationStatus =
                    registrationService.CreateAccount(account);

                registrationStatus =
                    accountCreationStatus.ToRegistrationStatus();
            }

            return new RegisterResponseParameters(
                registrationStatus,
                validationResult.ToErrorMessage());
        }
    }
}