using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;

namespace Authenticator.API.Controllers
{
    public class RegistrationController : IRegistrationController
    {
        private readonly IRegistrationService registrationService;
        private readonly RegistrationDataValidator registrationDataValidator;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;

            registrationDataValidator = new RegistrationDataValidator();
        }

        public AccountCreationStatus Register(RegistrationData registrationData)
        {
            var accountCreationStatus = AccountCreationStatus.Failed;

            // TODO: Get errors if invalid
            var validationResult =
                registrationDataValidator.Validate(registrationData);
            if (validationResult.IsValid)
            {
                var account = AccountFactory.CreateAccount(
                    registrationData.Email,
                    registrationData.Password,
                    registrationData.FirstName,
                    registrationData.LastName);

                accountCreationStatus =
                    registrationService.CreateAccount(account);
            }

            return accountCreationStatus;
        }
    }
}