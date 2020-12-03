using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Authenticator.Domain.Aggregates.User;
using Authenticator.API.Converters;
using Authenticator.API.Constants;

namespace Authenticator.API.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly IRegistrationService registrationService;
        private readonly RegistrationDataValidator registrationDataValidator;
        private readonly LoginDataValidator loginDataValidator;

        public AuthController(
            ILoginService loginService,
            IRegistrationService registrationService)
        {
            this.loginService = loginService;
            this.registrationService = registrationService;

            loginDataValidator = new LoginDataValidator();
            registrationDataValidator = new RegistrationDataValidator();
        }

        [HttpPost]
        [Route("/login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AccountData> Login(LoginData loginData)
        {
            AccountData accountData;

            var validationResult = loginDataValidator.Validate(loginData);
            if (validationResult.IsValid)
            {
                var email = loginData.Email;
                var password = loginData.Password;
                var account = loginService.Authenticate(email, password);
                if (account != null)
                {
                    accountData = new AccountData()
                    {
                        Id = account.Id
                    };
                }
                else
                {
                    var errorData = new ErrorData()
                    {
                        ErrorMessages = new string[] { ErrorMessages.AccountNotFound }
                    };

                    return NotFound(errorData);
                }
            }
            else
            {
                var errorData = new ErrorData()
                {
                    ErrorMessages = validationResult.Errors.ConvertToErrorMessages()
                };

                return BadRequest(errorData);
            }

            return Ok(accountData);
        }

        [HttpPost]
        [Route("/register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Register(RegistrationData registrationData)
        {
            var validationResult =
                registrationDataValidator.Validate(registrationData);
            if (validationResult.IsValid)
            {
                var email = registrationData.Email;
                var password = registrationData.Password;
                var firstName = registrationData.FirstName;
                var lastName = registrationData.LastName;

                if (registrationService.CheckIfEmailExists(email))
                {
                    var errorData = new ErrorData()
                    {
                        ErrorMessages = new string[] { ErrorMessages.EmailAlreadyExists }
                    };

                    return BadRequest(errorData);
                }
                else
                {
                    var account =
                        Account.Create(email, password, firstName, lastName);

                    registrationService.CreateAccount(account);
                }
            }
            else
            {
                var errorData = new ErrorData()
                {
                    ErrorMessages = validationResult.Errors.ConvertToErrorMessages()
                };

                return BadRequest(errorData);
            }

            return Ok();
        }
    }
}