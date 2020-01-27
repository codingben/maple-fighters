using Authenticator.Application.Peer.Logic.Operations.Converters;
using Authenticator.Application.Peer.Logic.Operations.Validators;
using Authenticator.Common;
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
            var parameters = messageData.Parameters;

            var validationResult =
                registerParametersValidator.Validate(parameters);
            if (validationResult.IsValid)
            {
                var email = parameters.Email;
                var password = parameters.Password;
                var firstName = parameters.FirstName;
                var lastName = parameters.LastName;
                var account = 
                    AccountFactory.CreateAccount(email, password, firstName, lastName);
                var accountCreationStatus = GetAccountCreationStatus(account);

                registrationStatus = accountCreationStatus.ToRegistrationStatus();
            }

            var errorMessage = validationResult.ToErrorMessage();

            return new RegisterResponseParameters(registrationStatus, errorMessage);
        }

        private AccountCreationStatus GetAccountCreationStatus(Account account)
        {
            return registrationService.CreateAccount(account);
        }
    }
}