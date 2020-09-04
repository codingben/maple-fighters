using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Authenticator.Domain.Aggregates.User;
using Authenticator.API.Converters;

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
        public ActionResult<AuthenticationStatus> Login(LoginData loginData)
        {
            AuthenticationStatus authenticationStatus;

            var validationResult = loginDataValidator.Validate(loginData);
            if (validationResult.IsValid)
            {
                var email = loginData.Email;
                var password = loginData.Password;

                authenticationStatus =
                    loginService.Authenticate(email, password);
            }
            else
            {
                var errorData = new ErrorData()
                {
                    ErrorMessages = validationResult.Errors.ConvertToErrorMessages()
                };

                return BadRequest(errorData);
            }

            return Ok(authenticationStatus);
        }

        [HttpPost]
        [Route("/register")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<AccountCreationStatus> Register(RegistrationData registrationData)
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
                var errorData = new ErrorData()
                {
                    ErrorMessages = validationResult.Errors.ConvertToErrorMessages()
                };

                return BadRequest(errorData);
            }

            return Ok(accountCreationStatus);
        }
    }
}