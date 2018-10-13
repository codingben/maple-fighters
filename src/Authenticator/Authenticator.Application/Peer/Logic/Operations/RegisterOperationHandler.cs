using System;
using Authenticator.Common.Enums;
using Authenticator.Common.Parameters;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Authenticator.Application.Peer.Logic.Operations
{
    public class RegisterOperationHandler : 
        IOperationRequestHandler<RegisterRequestParameters, RegisterResponseParameters>
    {
        private readonly IRegistrationService registrationService;

        public RegisterOperationHandler(
            IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        public RegisterResponseParameters? Handle(
            MessageData<RegisterRequestParameters> messageData,
            ref MessageSendOptions sendOptions)
        {
            // TODO: Parameters validator
            var email = messageData.Parameters.Email;
            var password = messageData.Parameters.Password;
            var firstName = messageData.Parameters.FirstName;
            var lastName = messageData.Parameters.LastName;

            var account = AccountFactory.CreateAccount(
                email,
                password,
                firstName,
                lastName);

            var accountCreationStatus = registrationService.CreateAccount(account);
            RegistrationStatus registrationStatus;

            switch (accountCreationStatus)
            {
                case AccountCreationStatus.Succeed:
                {
                    registrationStatus = RegistrationStatus.Created;
                    break;
                }

                case AccountCreationStatus.EmailExists:
                {
                    registrationStatus = RegistrationStatus.EmailAlreadyInUse;
                    break;
                }

                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return new RegisterResponseParameters(registrationStatus);
        }
    }
}