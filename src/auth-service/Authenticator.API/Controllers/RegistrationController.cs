using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Authenticator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService registrationService;
        private readonly RegistrationDataValidator registrationDataValidator;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;

            registrationDataValidator = new RegistrationDataValidator();
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AccountCreationStatus> Signup(RegistrationData registrationData)
        {
            AccountCreationStatus accountCreationStatus;

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
            else
            {
                var errorMessage = validationResult.ToString();

                return BadRequest(errorMessage);
            }

            return Ok(accountCreationStatus);
        }
    }
}