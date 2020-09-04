using Authenticator.API.Datas;
using Authenticator.API.Validators;
using Authenticator.Domain.Aggregates.User.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Authenticator.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SignInController : ControllerBase
    {
        private readonly ILoginService loginService;
        private readonly LoginDataValidator loginDataValidator;

        public SignInController(ILoginService loginService)
        {
            this.loginService = loginService;

            loginDataValidator = new LoginDataValidator();
        }

        [HttpPost]
        [Consumes("application/json")]
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
                var errorMessage = validationResult.ToString();

                return BadRequest(errorMessage);
            }

            return Ok(authenticationStatus);
        }
    }
}